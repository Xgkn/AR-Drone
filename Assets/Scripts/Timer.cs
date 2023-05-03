using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text _Text;
    public float _Time;

    public UnityEvent onTimerCompleted;

    void Update()
    {
        if (_Time > 0f)
        {
            _Time -= Time.deltaTime;

            if (_Time <= 0f)
            {
                onTimerCompleted?.Invoke();
                _Time = 0f;
            }

            DisplayTime(_Time);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
