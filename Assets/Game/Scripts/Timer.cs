using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private float initialTime;
    [SerializeField] private Text timerUI;

    private float timeLeft;
    private IEnumerator timerCoroutine;

    [Inject] private readonly GlobalEventManager events;

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
            events.TriggerFail();
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
        timeLeft = initialTime;
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
    }

    public void Deactivate()
    {
        StopCoroutine(timerCoroutine);
    }
}
