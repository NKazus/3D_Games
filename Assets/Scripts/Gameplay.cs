using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private Rotator rotator;

    [SerializeField] private int objectsMin;
    [SerializeField] private int objectsMax;

    [SerializeField] private Text grabbingText;

    [SerializeField] private Button collapseB;
    [SerializeField] private Button leftB;
    [SerializeField] private Button rightB;

    private int hits;
    private int misses;
    private int objectsNumber;

    [Inject] private readonly Randomizer randomizer;
    [Inject] private readonly EventManager events;
    [Inject] private readonly GameData data;

    private void OnEnable()
    {
        data.UpdateMoney(0);
        data.UpdateCharges(0);

        events.GameStateEvent += SwitchGame;
    }

    private void OnDisable()
    {
        events.GameStateEvent -= SwitchGame;

        collapseB.onClick.RemoveListener(Collapse);
        spawner.StopSpawning();

        if (IsInvoking())
        {
            CancelInvoke("CheckStatus");
        }
    }

    private void SwitchGame(bool activate)
    {
        if (activate)
        {
            Debug.Log("Active");
            hits = 0;
            misses = 0;

            grabbingText.enabled = false;

            if (data.Charges > 0)
            {
                collapseB.onClick.AddListener(Collapse);
                collapseB.image.DOFade(1f, 0.4f);
            }
            else
            {
                collapseB.image.DOFade(0.5f, 0.4f);
            }

            leftB.onClick.AddListener(() => rotator.RotateView(false));
            rightB.onClick.AddListener(() => rotator.RotateView(true));

            events.MeteorEvent += CalculateMeteors;
            events.MeteorTriggerEvent += ShowGrabState;

            objectsNumber = randomizer.GenerateInt(objectsMin, objectsMax);
            Debug.Log("oNum:"+objectsNumber);
            spawner.StartSpawning(objectsNumber, data.TimeScale);

        }
        else
        {
            Debug.Log("Inactive");

            events.MeteorTriggerEvent -= ShowGrabState;
            events.MeteorEvent -= CalculateMeteors;

            leftB.onClick.RemoveAllListeners();
            rightB.onClick.RemoveAllListeners();
        }
    }

    private void Collapse()
    {
        collapseB.onClick.RemoveListener(Collapse);
        events.CollapseAll();
        data.UpdateCharges(-1);
        if (data.Charges > 0)
        {
            collapseB.onClick.AddListener(Collapse);
            return;
        }
        collapseB.image.DOFade(0.5f, 0.4f);
    }

    private void ShowGrabState(bool inactive)
    {
        grabbingText.enabled = !inactive;
    }

    private void CalculateMeteors(bool destroyed)
    {
        
        if (destroyed)
        {
            hits++;
        }
        else
        {
            misses++;
        }
        Debug.Log("collis:"+(hits+misses));
        if (hits + misses >= objectsNumber)
        {
            Finish();
        }
    }

    private void Finish()
    {
        data.UpdateTime(false);
        data.UpdateMoney((hits - misses));
        events.DoResult(hits, misses);
        events.SwitchGameState(false);
    }

}
