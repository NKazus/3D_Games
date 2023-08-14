using System;
using System.Collections;
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

    [SerializeField] private GameObject seedText;

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
        if (activate)
        {
            ResetGarden();
            isWindy = false;
            wind.enabled = false;
            plant.DoShake(false);

            currentState = GardenState.Normal;
            UpdateStateImages();

            if (data.Seeds <= 0)
            {
                seedText.SetActive(true);
                return;
            }            

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
            currentState = GardenState.Normal;
            UpdateStateImages();

            waterCan.onClick.RemoveListener(UseWaterCan);
            rake.onClick.RemoveListener(UseRake);
            prop.onClick.RemoveListener(UseProp);

            StopAllCoroutines();

            seedText.SetActive(false);
        }
    }

    private void UseWaterCan()
    {
        currentState = tools.DoWater(currentState);
        UpdateStateImages();
        eventManager.PlayAction(1);
    }

    private void UseRake()
    {
        currentState = tools.DoLoosening(currentState);
        UpdateStateImages();
        eventManager.PlayAction(2);
    }

    private void UseProp()
    {
        tools.DoProp();
        isWindy = false;
        plant.DoShake(false);
        wind.enabled = false;
        eventManager.PlayAction(0);

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
                    eventManager.PlayWeather(0);
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
            eventManager.PlayCoins();
            plant.Grow(plantIterations - 1);
            plant.DoFlower(true);
            int reward = rand.Next(5, 20);
            data.UpdateMoney(reward);
            data.UpdateSeeds(1);
            eventManager.DoWin(reward);
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
            case GardenState.Rain: weather.DoRain(); eventManager.PlayWeather(1); break;
            case GardenState.Heat: weather.DoHeat(); eventManager.PlayWeather(2); break;
            default: throw new NotSupportedException();
        }

        UpdateStateImages();
    }

    private void FailPlant()
    {
        eventManager.PlayVibro();
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
