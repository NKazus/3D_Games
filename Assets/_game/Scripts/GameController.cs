using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private Weather weather;
    [SerializeField] private Tools tools;
    [SerializeField] private Plant plant;
    [SerializeField] private float plantEventDelta = 5f;

    [SerializeField] private Button waterCan;
    [SerializeField] private Button rake;
    [SerializeField] private Button prop;

    [SerializeField] private Image rain;
    [SerializeField] private Image heat;
    [SerializeField] private Image wind;

    private State gardenState;
    private System.Random rand = new System.Random();
    private int plantIterations;
    private GardenState currentState;

    private bool isWindy;

    [Inject] private readonly GlobalEventManager eventManager;
    [Inject] private readonly DataHandler data;

    private void Awake()
    {
        gardenState = new State();
        plantIterations = plant.GetGrowIterations();
    }

    private void OnEnable()
    {
        eventManager.GameStateEvent += ActivateGarden;
    }

    private void OnDisable()
    {
        eventManager.GameStateEvent -= ActivateGarden;
    }

    private void ActivateGarden(bool activate)
    {
        if (activate && data.Seeds > 0)
        {
            ResetGarden();
            isWindy = false;
            wind.enabled = false;
            plant.DoShake(false);

            currentState = GardenState.Normal;
            UpdateStateImages();

            waterCan.onClick.AddListener(UseWaterCan);
            rake.onClick.AddListener(UseRake);

            if(data.Props > 0)
            {
                prop.onClick.AddListener(UseProp);
                prop.image.DOFade(1f, 0.5f);
            }
            else
            {
                prop.image.DOFade(0.5f, 0.5f);
            }
            StartCoroutine(StartGrowing());
            data.UpdateSeeds(-1);
        }
        else
        {
            waterCan.onClick.RemoveListener(UseWaterCan);
            rake.onClick.RemoveListener(UseRake);
            prop.onClick.RemoveListener(UseProp);

            StopAllCoroutines();
        }
    }

    private void UseWaterCan()
    {
        currentState = tools.DoWater(currentState);
        UpdateStateImages();
    }

    private void UseRake()
    {
        currentState = tools.DoLoosening(currentState);
        UpdateStateImages();
    }

    private void UseProp()
    {
        tools.DoProp();
        isWindy = false;
        plant.DoShake(false);
        wind.enabled = false;

        data.UpdateProps(-1);
        if(data.Props <= 0)
        {
            prop.onClick.RemoveListener(UseProp);
            prop.image.DOFade(0.5f, 0.5f);
        }
    }

    private void UpdateStateImages()
    {
        switch (currentState)
        {
            case GardenState.Rain: rain.enabled = true; heat.enabled = false; break;
            case GardenState.Heat: rain.enabled = false; heat.enabled = true; break;
            default: rain.enabled = false; heat.enabled = false; break;
        }
    }

    private IEnumerator StartGrowing()
    {
        int iter = 0;
        while (iter < plantIterations * 2)
        {
            yield return new WaitForSeconds(plantEventDelta);
            CheckGarden();

            if(iter < plantIterations * 2 - 1)
            {
                UpdateGarden();
                if (GetRandomBoolean())
                {
                    weather.DoWind();
                    plant.DoShake(true);
                    isWindy = true;
                    wind.enabled = true;
                }
            }
            
            plant.Grow(iter / 2);
            iter++;
        }
        if (iter >= plantIterations)
        {

            plant.Grow(plantIterations - 1);
            plant.DoFlower(true);
            Debug.Log("win");
            data.UpdateMoney(rand.Next(5, 10));
            data.UpdateSeeds(1);
            eventManager.SwitchGameState(false);
        }
    }

    private void CheckGarden()
    {
        if (currentState != GardenState.Normal || isWindy)
        {
            FailPlant();
        }
    }

    private void UpdateGarden()
    {
        currentState = gardenState.UpdateState(currentState);
        switch (currentState)
        {
            case GardenState.Normal: weather.DoNormal(); break;
            case GardenState.Rain: weather.DoRain(); break;
            case GardenState.Heat: weather.DoHeat(); break;
            default: throw new NotSupportedException();
        }

        UpdateStateImages();
    }

    private void FailPlant()
    {
        Debug.Log("lose");
        eventManager.SwitchGameState(false);
    }

    private void ResetGarden()
    {
        weather.DoNormal();
        weather.DoWind();
        plant.DoFlower(false);
        plant.Grow(0);
    }

    private bool GetRandomBoolean()
    {
        return rand.Next(0, 2) > 0;
    }
}
