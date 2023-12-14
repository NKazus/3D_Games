using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timerUI;
    [SerializeField] private Text timerText;

    private float timeLeft;

    private IEnumerator timerCoroutine;

    [Inject] private readonly EventManager events;

    private void OnEnable()
    {
        timerText.enabled = false;
    }

    private IEnumerator StartTimer()
    {
        Debug.Log("Timer");
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
            yield return null;
        }
        if (timeLeft <= 0)
        {
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

    public void Activate(float time)
    {
        if(timerCoroutine != null)
        {
            Deactivate();
        }

        timerText.enabled = true;
        timeLeft = time;

        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
    }

    public void Deactivate()
    {
        StopCoroutine(timerCoroutine);
    }
}
