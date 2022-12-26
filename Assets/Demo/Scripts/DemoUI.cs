using UnityEngine;
using UnityEngine.UI;
using TayfaGames.DateTimeManager;

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
    }

    private void Update()
    {
        dateTimeText.text = dateTimeManager.GetFormattedDateTime("dd/MM/yyyy") + "\n" + dateTimeManager.GetFormattedDateTime("HH:mm");
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
            dateTimeManager.StopTime();
            stopResumeButton.transform.Find("Text").GetComponent<Text>().text = "Resume";
        }
        else
        {
            dateTimeManager.ResumeTime();
            stopResumeButton.transform.Find("Text").GetComponent<Text>().text = "Stop";
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
}
