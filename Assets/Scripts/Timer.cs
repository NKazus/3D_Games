using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private int timeMin;
    [SerializeField] private int timeMax;
    [SerializeField] private Text timerUI;
    [SerializeField] private Text timerText;

    private float timeLeft;

    [Inject] private readonly EventManager events;
    [Inject] private readonly Randomizer rand;

    private void OnEnable()
    {
        timerText.enabled = false;
    }

    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
            yield return null;
        }
        if (timeLeft <= 0)
        {
            Debug.Log("timer out");
            timerText.enabled = false;
            events.TriggerTimeout();
        }
    }

    private void UpdateTimer(float timeLeft)
    {
        if (timeLeft < 0)
        {
            timeLeft = 0;
        }
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Activate()
    {
        timerText.enabled = true;
        timeLeft = rand.GenerateInt(timeMin, timeMax);
        StartCoroutine(StartTimer());
    }
}
