using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuildMode : MonoBehaviour
{
    [Tooltip("ID: 0 - 1 - 2 - ...")]
    [SerializeField] private Brick[] bricks;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private Transform bricksTransform;
    [SerializeField] private ScaleGenerator scaleGenerator;

    [SerializeField] private Text initialText;
    [SerializeField] private Text pickText;
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private string winText;
    [SerializeField] private string loseText;
    [SerializeField] private Button restartButton;

    private Text restartText;
    private Vector3 initialBricksPosition;

    private int currentId;
    private int currentRound;

    [Inject] private readonly GameData data;
    [Inject] private readonly GlobalEvents events;

    private void Awake()
    {
        initialBricksPosition = bricksTransform.localPosition;
        restartText = restartPanel.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        ActivateBricks(false);

        data.UpdateResourceValue(GameResource.Score, 0);
        data.UpdateResourceValue(GameResource.Walls, 0);
        Restart();
        restartButton.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(Restart);
        DOTween.Kill("build");
    }

    private void InitCallback(int id)
    {
        currentRound++;
        data.UpdateRoundsValue(currentRound);
        initialText.enabled = false;
        ActivateBricks(false);
        scaleGenerator.SetScale(bricks[id].GetScale());
        for (int i = 0; i < bricks.Length; i++)
        {
            bricks[i].SetCallback(PickCallback);
        }
        HideBricks();
    }

    private void PickCallback(int id)
    {
        ActivateBricks(false);
        currentRound++;
        data.UpdateRoundsValue(currentRound);
        if (id == currentId)
        {
            if (currentRound >= 10)
            {
                Finish(true);
            }
            else
            {
                HideBricks();
            }
        }
        else
        {
            Finish(false);
        }
    }

    private void ActivateBricks(bool active)
    {
        for (int i = 0; i < bricks.Length; i++)
        {
            bricks[i].ActivateBrick(active);
        }
    }

    private void HideBricks()
    {
        DOTween.Sequence()
            .SetId("build")
            .Append(bricksTransform.DOLocalMoveY(0.1f, 0.5f))
            .Append(bricksTransform.DOLocalMove(endPos, 1f))     
            .OnComplete(() => GenerateValues());
    }

    private void GenerateValues()
    {
        currentId = scaleGenerator.GenerateId(bricks.Length);
        for (int i = 0; i < bricks.Length; i++)
        {
            bricks[i].SetScale(scaleGenerator.GenerateDefault(), true);
        }
        bricks[currentId].SetScale(scaleGenerator.GetTarget(), false);
        ShowBricks();
    }

    private void ShowBricks()
    {
        bricksTransform.position = startPos;
        DOTween.Sequence()
            .SetId("build")
            .Append(bricksTransform.DOLocalMove(initialBricksPosition, 1f))
            .Append(bricksTransform.DOLocalMoveY(0.2f, 0.5f))
            .OnComplete(() => { ActivateBricks(true); pickText.enabled = true; });
    }

    private void ResetBricks()
    {
        for (int i = 0; i < bricks.Length; i++)
        {
            bricks[i].SetScale(scaleGenerator.GenerateDefault(), true);
            bricks[i].SetCallback(InitCallback);
        }
        bricksTransform.localPosition = initialBricksPosition;
    }

    private void Finish(bool win)
    {
        pickText.enabled = false;
        data.UpdateResourceValue(GameResource.Score, -5);
        if (win)
        {
            data.UpdateResourceValue(GameResource.Walls, 1);
            events.PlayWall(true);
        }
        else
        {
            events.PlayVibro();
        }
        restartText.text = win ? winText : loseText;
        restartPanel.SetActive(true);
    }

    private void Restart()
    {
        currentRound = 0;
        data.UpdateRoundsValue(currentRound);
        restartPanel.SetActive(false);
        ResetBricks();
        if(data.GameScore >= 5)
        {
            ActivateBricks(true);
            initialText.text = "Pick a shard!";
        }
        else
        {
            initialText.text = "Not enough points!";
        }
        initialText.enabled = true;
    }
}
