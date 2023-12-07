using UnityEngine;
using UnityEngine.UI;
using Zenject;
using FitTheSize.GameServices;

namespace FitTheSize.Main
{
    public class RunTrigger : MonoBehaviour
    {
        [SerializeField] private Button restart;
        [SerializeField] private GameObject restartBg;

        private Text repText;
        private Text incomeText;

        [Inject] private readonly GameEventHandler events;

        private void Awake()
        {
            repText = restartBg.transform.GetChild(1).GetComponent<Text>();
            incomeText = restartBg.transform.GetChild(2).GetComponent<Text>();
        }

        private void OnEnable()
        {
            events.GameStateEvent += ChangeGameState;
            events.ResultEvent += ShowResult;

            restart.onClick.AddListener(Restart);
            ChangeGameState(true);
        }

        private void OnDisable()
        {
            events.ResultEvent -= ShowResult;
            events.GameStateEvent -= ChangeGameState;

            restart.onClick.RemoveListener(Restart);
            ChangeGameState(false);
        }

        private void Restart()
        {
            events.SwitchGameState(true);
        }

        private void ChangeGameState(bool isActive)
        {
            restartBg.SetActive(!isActive);
        }

        private void ShowResult(int rep, int money)
        {
            repText.text = rep.ToString();
            incomeText.text = money.ToString();
        }
    }
}
