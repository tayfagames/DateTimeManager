using UnityEngine;
using UnityEngine.UI;
using TayfaGames;
using System;

public class DemoUI : MonoBehaviour
{
    DateTimeManager dateTimeManager;
    Canvas canvas;
    Text dateTimeText;

    Button slowDownButton;
    Button speedUpButton;
    Button stopResumeButton;
    Button minuteProgressButton;
    Button hourProgressButton;
    Button dayProgressButton;

    private void OnEnable()
    {
        DateTimeManager.OnMinutePass += HandleMinutePass;
        DateTimeManager.OnHourPass += HandleHourPass;
        DateTimeManager.OnDayPass += HandleDayPass;
        DateTimeManager.OnMonthPass += HandleMonthPass;
        DateTimeManager.OnYearPass += HandleYearPass;
    }

    private void OnDisable()
    {
        DateTimeManager.OnMinutePass -= HandleMinutePass;
        DateTimeManager.OnHourPass -= HandleHourPass;
        DateTimeManager.OnDayPass -= HandleDayPass;
        DateTimeManager.OnMonthPass -= HandleMonthPass;
        DateTimeManager.OnYearPass -= HandleYearPass;
    }

    private void Start()
    {
        dateTimeManager = FindObjectOfType<DateTimeManager>();
        canvas = FindObjectOfType<Canvas>();

        dateTimeText = canvas.transform.Find("Date Time Text").GetComponent<Text>();
        slowDownButton = canvas.transform.Find("Slow Down Button").GetComponent<Button>();
        speedUpButton = canvas.transform.Find("Speed Up Button").GetComponent<Button>();
        stopResumeButton = canvas.transform.Find("Stop Resume Button").GetComponent<Button>();
        minuteProgressButton = canvas.transform.Find("Minute Progress Button").GetComponent<Button>();
        hourProgressButton = canvas.transform.Find("Hour Progress Button").GetComponent<Button>();
        dayProgressButton = canvas.transform.Find("Day Progress Button").GetComponent<Button>();

        slowDownButton.onClick.AddListener(SlowDown);
        speedUpButton.onClick.AddListener(SpeedUp);
        stopResumeButton.onClick.AddListener(ResumeStop);
        minuteProgressButton.onClick.AddListener(MinuteProgress);
        hourProgressButton.onClick.AddListener(HourProgress);
        dayProgressButton.onClick.AddListener(DayProgress);

        dateTimeManager.QueueJob(() => HappyBirthday("Enis"), dateTimeManager.GetUnixDateTime() + dateTimeManager.Timedelta(days: 3));
        dateTimeManager.QueueJob(() => HappyBirthday("Canberk"), dateTimeManager.GetUnixDateTime() + dateTimeManager.Timedelta(days: 6));
        dateTimeManager.QueueJob(() => HappyBirthday("Oguzhan"), dateTimeManager.GetUnixDateTime() + dateTimeManager.Timedelta(days: 4));
    }

    private void Update()
    {
        dateTimeText.text = dateTimeManager.GetFormattedDateTime("dd/MM/yyyy");
        dateTimeText.text += "\n" + dateTimeManager.GetFormattedDateTime("HH:mm");
        dateTimeText.text += "\n" + dateTimeManager.GetSpeedModifier().ToString() + "x";
    }

    private void SlowDown()
    {
        dateTimeManager.DecreaseSpeedModifier();
    }

    private void SpeedUp()
    {
        dateTimeManager.IncreaseSpeedModifier();
    }

    private void ResumeStop()
    {
        if (dateTimeManager.GetStopped())
        {
            dateTimeManager.ResumeTime();
            stopResumeButton.transform.Find("Text").GetComponent<Text>().text = "Stop";
        }
        else
        {
            dateTimeManager.StopTime();
            stopResumeButton.transform.Find("Text").GetComponent<Text>().text = "Resume";
        }
    }

    private void MinuteProgress()
    {
        dateTimeManager.SetProgressMode(ProgressMode.Minute);
    }

    private void HourProgress()
    {
        dateTimeManager.SetProgressMode(ProgressMode.Hour);
    }

    private void DayProgress()
    {
        dateTimeManager.SetProgressMode(ProgressMode.Day);
    }

    private void HandleMinutePass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + "- Minute passed event");
    }

    private void HandleHourPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + "- Hour passed event");
    }

    private void HandleDayPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + "- Day passed event");
    }

    private void HandleMonthPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + "- Month passed event");
    }

    private void HandleYearPass(DateTime dateTime)
    {
        Debug.Log(dateTime.ToString() + "- Year passed event");
    }

    public void HappyBirthday(string name)
    {
        Debug.Log("Happy Birthday " + name + "!");
    }
}
