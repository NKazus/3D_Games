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

        private Text pathText;
        private Text scaleText;
        private Text totalText;

        [Inject] private readonly GameEventHandler events;

        private void Awake()
        {
            pathText = restartBg.transform.GetChild(1).GetComponent<Text>();
            scaleText = restartBg.transform.GetChild(2).GetComponent<Text>();
            totalText = restartBg.transform.GetChild(3).GetComponent<Text>();
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

        private void ShowResult(int path, int scale, int total)
        {
            pathText.text = path.ToString();
            scaleText.text = scale.ToString();
            totalText.text = total.ToString();
        }
    }
}
