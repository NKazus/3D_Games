using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RunManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private HealthBar health;

    [SerializeField] private Route[] routes;

    [SerializeField] private Spawner spawner;
    [SerializeField] private Transform slowPlatform;
    [SerializeField] private Transform damagePlatform;

    [SerializeField] private Timer timer;
    [SerializeField] private int maxRounds;

    [SerializeField] private float activeSlowCoefficient;
    [SerializeField] private float activeBoostCoefficient;
    [SerializeField] private float activeDamageCoefficient;
    [SerializeField] private float activeHealCoefficient;

    [SerializeField] private float initialJumpTime;
    [SerializeField] private float speedModifyer;

    [SerializeField] private Button startButton;

    private MaterialInstance slowMat;
    private MaterialInstance damageMat;

    private bool finished;
    private int rounds;

    private float slowCoefficient;
    private float boostCoefficient;
    private float damageCoefficient;
    private float healCoefficient;

    private float jumpTime;

    private Vector3 jumpScale;

    [Inject] private readonly GlobalEventManager events;
    [Inject] private readonly DataHandler dataHandler;
    [Inject] private readonly RandomGenerator random;

    private void Awake()
    {
        slowMat = slowPlatform.GetComponent<MaterialInstance>();
        damageMat = damagePlatform.GetComponent<MaterialInstance>();

        jumpScale = new Vector3(0, 0.1f, 0);
    }

    private void OnEnable()
    {
        events.GameStateEvent += ChangeState;
        dataHandler.UpdateGlobalScore(0);

        startButton.onClick.AddListener(Play);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(Play);

        events.GameStateEvent -= ChangeState;
        DOTween.Kill("jump");
    }

    private void ChangeState(bool activate)
    {
        if (activate)
        {
            dataHandler.RefreshBuffs();
            timer.Refresh();

            finished = false;
            routes[0].ResetRoute();

            Vector3 pos = routes[0].GetCurrent();
            player.position = new Vector3(pos.x, player.position.y, pos.z);

            slowMat.ResetColor();
            damageMat.ResetColor();
            slowPlatform.position = routes[0].GetById(routes[0].GenerateId());
            damagePlatform.position = routes[0].GetById(routes[0].GenerateId());

            boostCoefficient = dataHandler.Boost ? activeBoostCoefficient : 1f;
            slowCoefficient = dataHandler.Slow ? activeSlowCoefficient : 1f;
            healCoefficient = dataHandler.Heal ? activeHealCoefficient : 1f;
            damageCoefficient = dataHandler.Damage ? activeDamageCoefficient : 1f;

            health.ResetHp();

            rounds = 0;
            dataHandler.UpdateRounds(rounds);

            jumpTime = initialJumpTime;

            startButton.gameObject.SetActive(true);

            events.BuffEvent += ApplyBuff;
            events.FailEvent += CrashPlayer;
            events.BuffMergeEvent += UpdateRoute;
        }
        else
        {
            spawner.StopSpawning();
            timer.Deactivate();

            events.BuffMergeEvent -= UpdateRoute;
            events.FailEvent -= CrashPlayer;
            events.BuffEvent -= ApplyBuff;
        }
    }

    private void Play()
    {
        startButton.gameObject.SetActive(false);
        Jump();
        timer.Activate();
        spawner.Initialize(routes[0]);
        spawner.StartSpawning();
    }

    private void Jump()
    {
        Vector3 cellPosition = routes[0].GetNext();
        DOTween.Sequence()
            .SetId("jump")
            .Append(player.DOJump(new Vector3(cellPosition.x, player.position.y, cellPosition.z), 0.2f, 1, jumpTime))
            .Join(player.DOShakeScale(jumpTime, jumpScale, 5, 90))
            .OnComplete(() => JumpCallback());
    }

    private void JumpCallback()
    {
        if (finished)
        {
            return;
        }
        if (routes[0].IsFinishing())
        {
            rounds++;
            dataHandler.UpdateRounds(rounds);
            if (rounds >= maxRounds)
            {
                dataHandler.RefreshBuffs(true);
                events.PlayReward();
                int reward = random.GenerateInt(25, 51);
                events.DoWin(reward);
                dataHandler.UpdateGlobalScore(reward);
                events.SwitchGameState(false);
                return;
            }
        }

        Jump();
    }

    private void CrashPlayer()
    {
        finished = true;
        dataHandler.RefreshBuffs(true);
        events.PlayVibro();
        events.SwitchGameState(false);
    }

    private void ApplyBuff(int id, bool state)
    {

        switch (id)
        {
            case 1: float newJumpTime = state ? (jumpTime - (speedModifyer * boostCoefficient))
                    : (jumpTime + (speedModifyer * slowCoefficient));
                events.PlayBuff(true);
                jumpTime = Mathf.Clamp(newJumpTime, 0.4f, 1.5f);
                break;
            case 2: health.UpdateHp((state ? 1f : -1f) * (state ? healCoefficient : damageCoefficient));
                events.PlayBuff(false);
                break;
            default: throw new NotSupportedException();
        }
    }

    private void UpdateRoute(int cellId)
    {
        routes[0].RemoveId(cellId);
    }
}
