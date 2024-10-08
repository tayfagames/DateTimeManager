using System;
using System.Globalization;
using UnityEngine;

namespace TayfaGames
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
        public static event DateTimeEvent TimeProgressEvent;
        public static event DateTimeEvent OnMinutePass;
        public static event DateTimeEvent OnHourPass;
        public static event DateTimeEvent OnDayPass;
        public static event DateTimeEvent OnMonthPass;
        public static event DateTimeEvent OnYearPass;

        [SerializeField] ProgressMode progressMode = ProgressMode.Hour;
        [SerializeField] string startDateTime;
        [SerializeField] bool startOnAwake = true;

        DateTime startDateTimeObject;
        DateTime currentDateTime;
        DateTime dateTimeInPreviousFrame;
        JobQueue jobQueue;

        float speedModifier = 1f;
        float timeElapsedSinceTimeProgress = Mathf.Infinity;
        bool stopped;

        const int WEEKSTEP = 604800;
        const int DAYSTEP = 86400;
        const int HOURSTEP = 3600;
        const int MINUTESTEP = 60;

        private void Start()
        {
            startDateTimeObject = DateTime.ParseExact(startDateTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            jobQueue = GetComponent<JobQueue>();
            stopped = !startOnAwake;
            dateTimeInPreviousFrame = startDateTimeObject;
            currentDateTime = startDateTimeObject;
        }

        private void Update()
        {
            if (speedModifier <= 0f || stopped || progressMode == ProgressMode.Manual) { return; }

            if (timeElapsedSinceTimeProgress > 1 / speedModifier)
            {
                timeElapsedSinceTimeProgress = 0f;
                ProgressTime();
            }

            timeElapsedSinceTimeProgress += Time.deltaTime;
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

        public void ProgressTime(DateTime nextDateTime)
        {
            if (nextDateTime < currentDateTime) { return; }

            currentDateTime = nextDateTime;
            HandleEvents(ProgressMode.Manual);
            dateTimeInPreviousFrame = currentDateTime;
        }

        private void HandleEvents(ProgressMode progressMode)
        {
            TimeProgressEvent?.Invoke(currentDateTime);

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

        public void SetProgressMode(ProgressMode progressMode)
        {
            this.progressMode = progressMode;
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

        public void IncreaseSpeedModifier(float amount)
        {
            speedModifier += amount;
        }

        public void DecreaseSpeedModifier()
        {
            speedModifier -= 1;
        }

        public void DecreaseSpeedModifier(float amount)
        {
            speedModifier -= amount;
        }

        public void StopTime()
        {
            stopped = true;
        }

        public void ResumeTime()
        {
            stopped = false;
        }

        public bool GetStopped()
        {
            return stopped;
        }

        public void SetStopped(bool stopped)
        {
            this.stopped = stopped;
        }

        public long GetUnixDateTime()
        {
            return ((DateTimeOffset)currentDateTime).ToUnixTimeSeconds();
        }

        public string GetFormattedDateTime(string format)
        {
            return currentDateTime.ToString(format);
        }

        public DateTime GetDateTime()
        {
            return currentDateTime;
        }

        public void SetDateTime(DateTime currentDateTime)
        {
            this.currentDateTime = currentDateTime;
        }

        public void QueueJob(Action function, long timestamp)
        {
            if (timestamp < GetUnixDateTime()) { return; }

            jobQueue.QueueJob(function, timestamp);
        }

        public long Timedelta(int weeks = 0, int days = 0, int hours = 0, int minutes = 0)
        {
            return (long) weeks * WEEKSTEP + days * DAYSTEP + hours * HOURSTEP + minutes * MINUTESTEP;
        }
    }
}
