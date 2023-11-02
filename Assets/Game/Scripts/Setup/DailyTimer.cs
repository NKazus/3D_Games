using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyTimer : MonoBehaviour
{
    [SerializeField] private Text timerText;

    private float timeLeft;
    private IEnumerator timerCoroutine;

    private System.Action TimeOutCallback;

    private IEnumerator DoTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft --;
            UpdateTimer(timeLeft);
            yield return new WaitForSeconds(1f);
        }
        if(timeLeft <= 0)
        {
            ResetTimer();
            TimeOutCallback();
            Debug.Log("timer_out");
        }
    }

    private void UpdateTimer(float timeLeft)
    {
        if (timeLeft < 0)
        {
            timeLeft = 0;
        }
        int hours = Mathf.FloorToInt(timeLeft / 3600);
        int seconds = Mathf.FloorToInt(timeLeft % 3600);
        int minutes = Mathf.FloorToInt(seconds / 60);
        seconds = Mathf.FloorToInt(seconds % 60);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void ResetTimer()
    {
        UpdateTimer(0);
    }

    public void StartTimer(int secondsRemaining)
    {
        timeLeft = secondsRemaining;
        timerCoroutine = DoTimer();
        StartCoroutine(timerCoroutine);
        Debug.Log("timer_on");
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            Debug.Log("timer_off");
        }
    }

    public void SetCallback(System.Action callback)
    {
        TimeOutCallback = callback;
    }
}
