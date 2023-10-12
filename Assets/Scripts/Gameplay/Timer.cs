using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float initialTime;

    private IEnumerator timerCoroutine;
    private float timeLeft;

    private bool isRunning;

    private System.Action timeOutCallback;

    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        if (timeLeft <= 0)
        {
            isRunning = false;
            timeOutCallback();
        }
    }

    public void SetCallback(System.Action callback)
    {
        timeOutCallback = callback;
    }

    public void Activate()
    {
        if (isRunning)
        {
            return;
        }

        timeLeft = initialTime;
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
        isRunning = true;
        Debug.Log("timer_act");
    }

    public void Deactivate()
    {
        if (timerCoroutine != null)
        {
            isRunning = false;
            StopCoroutine(timerCoroutine);
            Debug.Log("timer_off");
        }
    }
}
