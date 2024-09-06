using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

public class DailyTimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TMP_Text TimerText;

    [Header("Button")]
    [SerializeField] private Button Button;
    [SerializeField] private TMP_Text ButtonText;

    [Header("Animation")]
    [SerializeField] private GameObject Present;
    [SerializeField] private Image Chest;

    public int hour;
    public int minute;

    public void Start()
    {
        StartCoroutine(UpdateTime(hour, minute));
    }
    private Coroutine updateTimeCoroutine;

    private void StopUpdateTimeCoroutine()
    {
        if (updateTimeCoroutine != null)
        {
            StopCoroutine(updateTimeCoroutine);
        }
    }

    private IEnumerator UpdateTime(int _hour, int _minute)
    {
        while (true)
        {
            DateTime currentTime = DateTime.UtcNow.AddHours(3);

            DateTime nextUpdateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, _hour, _minute, 0);
            if (currentTime.Hour >= _hour || (currentTime.Hour == _hour && currentTime.Minute >= _minute))
            {
                nextUpdateTime = nextUpdateTime.AddDays(1);
            }

            TimeSpan remainingTime = nextUpdateTime - currentTime;

            string formattedTime = FormatTimeSpan(remainingTime);

            yield return new WaitForSeconds(1f);

            TimerText.text = formattedTime;

            if (currentTime.Hour == _hour && currentTime.Minute == _minute && currentTime.Second == 0)
            {
                Debug.LogWarning("Daily updated");
                ButtonText.text = "Open";
                TimerText.text = "You have a new Daily present!";
                Button.interactable = true;
                StopUpdateTimeCoroutine(); 
                yield break;
            }
            else
            {
                ButtonText.text = "Daily is not available";
                Button.interactable = false;
            }
        }
    }


    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    public void ActiveDaily()
    {
        ButtonText.text = "Take";
        Button.interactable = true;
        TimerText.text = "";
        Present.SetActive(true);
    }

    public void UpdateTimer()
    {
       
        Present.SetActive(false);
        DateTime currentTime = DateTime.UtcNow.AddHours(3).AddMilliseconds(-DateTime.UtcNow.AddHours(3).Millisecond);
        int hour = currentTime.Hour;
        int minute = currentTime.Minute;
        StartCoroutine(UpdateTime(hour, minute));
    }
}
