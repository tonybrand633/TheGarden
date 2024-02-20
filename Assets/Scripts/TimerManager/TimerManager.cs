using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerManager : MonoBehaviour
{
    private static TimerManager instance;
    private Dictionary<string, float> timers = new Dictionary<string, float>();

    public static TimerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimerManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "TimerManager";
                    instance = obj.AddComponent<TimerManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartTimer(string timerName, float duration)
    {
        if (!timers.ContainsKey(timerName))
        {
            timers.Add(timerName, duration);
            StartCoroutine(CountdownTimer(timerName));
        }
    }

    private IEnumerator CountdownTimer(string timerName)
    {
        while (timers.ContainsKey(timerName) && timers[timerName] > 0f)
        {
            timers[timerName] -= Time.deltaTime;
            yield return null;
        }
        timers.Remove(timerName);
    }

    public float GetRemainingTime(string timerName)
    {
        if (timers.ContainsKey(timerName))
        {
            return timers[timerName];
        }
        else
        {
            return 0f;
        }
    }
}
