using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CauldronHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private Color defaultPotionColor;
    [SerializeField] private Color spoiledPotionColor;

    [SerializeField] private int potionIterations = 10;
    [SerializeField] private float potionEventDelta = 10f;

    private MeshRenderer meshRenderer;
    private ParticleSystem.MainModule smokeMain;
    private System.Random rand = new System.Random();

    private int cauldronState = 0;

    [Inject] private readonly GlobalEventManager eventManager;
    [Inject] private readonly DataHandler dataHandler;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = defaultPotionColor;

        smokeMain = smoke.main;
        smokeMain.startColor = defaultPotionColor;
    }

    private void OnEnable()
    {
        ResetCauldron();
        eventManager.GameStateEvent += ActivateCauldron;
    }

    private void OnDisable()
    {
        eventManager.GameStateEvent -= ActivateCauldron;
    }

    private void ActivateCauldron(bool activate)
    {
        if (activate)
        {
            ResetCauldron();
            eventManager.ReagentEvent += UpdateCauldron;
            StartCoroutine(StartBoiling());
        }
        else
        {
            eventManager.ReagentEvent -= UpdateCauldron;
            StopAllCoroutines();
        }
    }

    private IEnumerator StartBoiling()
    {
        int iter = 0;
        while(iter < potionIterations)
        {
            yield return new WaitForSeconds(potionEventDelta);
            CheckCauldron();
            if (GetRandomBoolean())
            {
                UpdateCauldron(dataHandler.GetReagent(rand.Next(0, 4)));
            }
            iter++;
        }
        if(iter >= potionIterations)
        {
            ResetCauldron();
            int extraPotions = rand.Next(0, 7);
            eventManager.DoWin(extraPotions);
            dataHandler.UpdatePotions(extraPotions);
            eventManager.PlayPotions();
            eventManager.SwitchGameState(false);
        }        
    }

    private void CheckCauldron()
    {
        if (cauldronState != 0)
        {
            FailPotion();
        }
    }

    private void UpdateCauldron(Reagent reagent)
    {
        cauldronState += reagent.value;

        if(cauldronState == 0)
        {
            ResetCauldron();
        }
        else
        {
            Reagent currentState = dataHandler.GetReagentByValue(cauldronState);
            if (currentState == null)
            {
                FailPotion();
            }
            else
            {
                meshRenderer.material.DOColor(currentState.color, 0.1f);
                smokeMain.startColor = currentState.color;
            }
        }        
    }

    private void FailPotion()
    {
        meshRenderer.material.DOColor(spoiledPotionColor, 0.1f);
        smokeMain.startColor = spoiledPotionColor;
        eventManager.SwitchGameState(false);
    }

    private void ResetCauldron()
    {
        cauldronState = 0;
        meshRenderer.material.DOColor(defaultPotionColor, 0.1f);
        smokeMain.startColor = defaultPotionColor;
    }

    private bool GetRandomBoolean()
    {
        return rand.Next(0, 2) > 0;
    }
}
