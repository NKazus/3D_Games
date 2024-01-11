using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private float initialTime;
    [SerializeField] private Text timerUI;

    private IEnumerator timerCoroutine;

    private System.Action TimeoutCallback;

    private float timeLeft;

    [Inject] private readonly EventHandler events;

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
            TimeoutCallback();
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

    public void Activate(float delay = 0f)
    {
        timeLeft = initialTime + delay;
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
    }

    public void Deactivate()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }

    public void Refresh()
    {
        timeLeft = initialTime;
        UpdateTimer(timeLeft);
    }

    public void SwitchVisibility(bool visible)
    {
        timerUI.enabled = visible;
    }

    public void SetCallback(System.Action callback)
    {
        TimeoutCallback = callback;
    }
}
