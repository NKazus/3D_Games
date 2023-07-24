using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Zenject;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private Transform targetBoard;
    [SerializeField] private Transform pickBoard;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 startPosition;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject pickText;
    [SerializeField] private GameObject winPanel;


    [Inject] private readonly ResourceHandler resources;
    [Inject] private readonly EventManager eventManager;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        winPanel.SetActive(false);
        startButton.onClick.AddListener(Activate);
        startButton.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        DOTween.KillAll();
        StopAllCoroutines();
        ResetBoards();
        startButton.onClick.RemoveListener(Activate);
        pickText.SetActive(false);
    }

    private void Activate()
    {
        startButton.gameObject.SetActive(false);
        winPanel.SetActive(false);
        ResetBoards();
        CalculateColors();
        targetBoard.DOMove(targetPosition, 2f)
            .SetId(this)
            .OnComplete(() => { StartCoroutine(Wait()); });
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        targetBoard.DOMove(startPosition, 2f)
            .SetId(this)
            .OnComplete(() => { MoveCrystals(); });
    }

    private void MoveCrystals()
    {

    }

    private void CalculateColors()
    {
        
    }


    private void CheckResult(int id)
    {
        pickText.SetActive(false);
        
        if (id > 0)
        {
            eventManager.PlayBonus();
            winPanel.SetActive(true);
        }
        else
        {
            eventManager.PlayVibro();
        }

        startButton.gameObject.SetActive(true);
    }

    private void ResetBoards()
    {
        targetBoard.position = startPosition;
        pickBoard.position = new Vector3(startPosition.x, startPosition.y, -startPosition.z);
    }
}
