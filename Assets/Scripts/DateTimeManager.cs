using System;
using UnityEngine;

namespace DateTimeManager
{
    public enum ProgressMode
    {
        Minute,
        Hour,
        Day,
        Manual
    }

    public class DateTimeManager : MonoBehaviour
    {
        public delegate void DateTimeEvent(DateTime dateTime);
        public static event DateTimeEvent OnMinutePass;
        public static event DateTimeEvent OnHourPass;
        public static event DateTimeEvent OnDayPass;
        public static event DateTimeEvent OnMonthPass;
        public static event DateTimeEvent OnYearPass;

        [SerializeField] ProgressMode progressMode = ProgressMode.Hour;
        DateTime startDateTime = new DateTime(1996, 11, 3, 9, 30, 0);
        DateTime currentDateTime;
        DateTime dateTimeInPreviousFrame;

        float speedModifier = 1f;
        float timeElapsedSinceTimeProgress = Mathf.Infinity;
        int currentHour;
        int currentDay;
        int currentMonth;
        int currentYear;
        bool stopped;

        const int DAYSTEP = 86400;
        const int HOURSTEP = 3600;
        const int MINUTESTEP = 60;

        private void Start()
        {
            currentDateTime = startDateTime;

            currentHour = currentDateTime.Hour;
            currentDay = currentDateTime.Day;
            currentMonth = currentDateTime.Month;
            currentYear= currentDateTime.Year;
        }

        private void Update()
        {
            if (speedModifier == 0f || stopped || progressMode == ProgressMode.Manual) { return; }

            if (timeElapsedSinceTimeProgress > 1 / speedModifier)
            {
                timeElapsedSinceTimeProgress = 0f;
                ProgressTime();
            }

            timeElapsedSinceTimeProgress += Time.deltaTime;

            Debug.Log(currentDateTime.ToString());
        }

        private void ProgressTime()
        {
            if (progressMode == ProgressMode.Minute)
            {
                currentDateTime = currentDateTime.AddSeconds(MINUTESTEP);
            }
            else if (progressMode == ProgressMode.Hour)
            {
                currentDateTime = currentDateTime.AddSeconds(HOURSTEP);
            }
            else if (progressMode == ProgressMode.Day)
            {
                currentDateTime = currentDateTime.AddSeconds(DAYSTEP);
            }

            HandleEvents(progressMode);
            dateTimeInPreviousFrame = currentDateTime;
        }

        public void ProgressTime(ProgressMode progressMode)
        {
            if (progressMode == ProgressMode.Minute)
            {
                currentDateTime = currentDateTime.AddSeconds(MINUTESTEP);
            }
            else if (progressMode == ProgressMode.Hour)
            {
                currentDateTime = currentDateTime.AddSeconds(HOURSTEP);
            }
            else if (progressMode == ProgressMode.Day)
            {
                currentDateTime = currentDateTime.AddSeconds(DAYSTEP);
            }

            HandleEvents(progressMode);
            dateTimeInPreviousFrame = currentDateTime;
        }

        private void HandleEvents(ProgressMode progressMode)
        {
            if (progressMode == ProgressMode.Day)
            {
                OnDayPass?.Invoke(currentDateTime);
            }
            else if (progressMode == ProgressMode.Hour)
            {
                OnHourPass?.Invoke(currentDateTime);

                if (dateTimeInPreviousFrame.Day != currentDateTime.Day)
                {
                    OnDayPass?.Invoke(currentDateTime);
                }
            }
            else if (progressMode == ProgressMode.Minute)
            {
                OnMinutePass?.Invoke(currentDateTime);

                if (dateTimeInPreviousFrame.Hour != currentDateTime.Hour)
                {
                    OnHourPass?.Invoke(currentDateTime);
                }
                if (dateTimeInPreviousFrame.Day != currentDateTime.Day)
                {
                    OnDayPass?.Invoke(currentDateTime);
                }
            }

            if (dateTimeInPreviousFrame.Year != currentDateTime.Year)
            {
                OnYearPass?.Invoke(currentDateTime);
            }
            if (dateTimeInPreviousFrame.Month != currentDateTime.Month)
            {
                OnMonthPass?.Invoke(currentDateTime);
            }
        }

        public void SetSpeedModifier(float speedModifier)
        {
            this.speedModifier = speedModifier;
        }

        public float GetSpeedModifier()
        {
            return speedModifier;
        }

        public void IncreaseSpeedModifier()
        {
            speedModifier += 1;
        }

        public void DecreaseSpeedModifier()
        {
            speedModifier -= 1;
        }

        public void StopTime()
        {
            stopped = false;
        }

        public void ResumeTime()
        {
            stopped = true;
        }

        public long GetUnixDateTime()
        {
            return ((DateTimeOffset)currentDateTime).ToUnixTimeSeconds();
        }

        public string GetFormattedDateTime(string format)
        {
            return currentDateTime.ToString(format);
        }
    }
}
