using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Rotator rotator;

    [SerializeField] private Text grabbingText;

    [SerializeField] private Button collapseB;
    [SerializeField] private Button leftB;
    [SerializeField] private Button rightB;

    private int hits;
    private int misses;

    private bool spawnEnabled;

    [Inject] private readonly Randomizer randomizer;
    [Inject] private readonly EventManager events;
    [Inject] private readonly GameData data;

    private void OnEnable()
    {
        events.GameStateEvent += SwitchGame;
    }

    private void OnDisable()
    {
        events.GameStateEvent -= SwitchGame;

        collapseB.onClick.RemoveListener(Collapse);
        StopAllCoroutines();

        if (IsInvoking())
        {
            CancelInvoke("CheckStatus");
        }
    }

    private void SwitchGame(bool activate)
    {
        if (activate)
        {
            hits = misses = 0;
            grabbingText.enabled = false;

            if (true)
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

            events.TimeOutEvent += TriggerTimeout;
            events.MeteorEvent += CalculateMeteors;
            events.MeteorTriggerEvent += ShowGrabState;

            spawnEnabled = true;
            spawner.StartSpawning();
            timer.Activate();

        }
        else
        {
            events.TimeOutEvent -= TriggerTimeout;
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
        //update data + check
        if (true)
        {
            collapseB.onClick.AddListener(Collapse);
            return;
        }
        collapseB.image.DOFade(0.5f, 0.4f);
    }

    private void TriggerTimeout()
    {
        Debug.Log("timeout");
        spawnEnabled = false;
        spawner.StopSpawning();
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

        Invoke("CheckStatus", 0.5f);
    }

    private void CheckStatus()
    {
        if (!spawnEnabled && spawner.CheckNumber())
        {
            //score and data
            events.DoResult(hits, misses);
            events.SwitchGameState(false);
        }
    }

}
