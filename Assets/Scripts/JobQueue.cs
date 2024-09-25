using System;
using System.Collections.Generic;
using UnityEngine;

namespace TayfaGames
{
    public class JobQueue : MonoBehaviour
    {
        List<long> jobTimestamps = new List<long>();
        List<Action> jobs = new List<Action>();

        private void OnEnable()
        {
            DateTimeManager.TimeProgressEvent += HandleTimeProgress;
        }

        private void OnDisable()
        {
            DateTimeManager.TimeProgressEvent -= HandleTimeProgress;
        }

        private void HandleTimeProgress(DateTime dateTime)
        {
            for (int i = 0; i < jobTimestamps.Count; i++)
            {
                if (((DateTimeOffset)dateTime).ToUnixTimeSeconds() >= jobTimestamps[i])
                {
                    Action job = jobs[i];
                    job.Invoke();
                    jobs.RemoveAt(i);
                    jobTimestamps.RemoveAt(i);
                    i--;
                }
            }
        }

        public void QueueJob(Action function, long timestamp)
        {
            jobTimestamps.Add(timestamp);
            jobs.Add(function);
        }

        public void ClearQueue()
        {
            jobs.Clear();
            jobTimestamps.Clear();
        }

        public void AddJobsToQueue(List<Tuple<Action, long>> jobsToAdd)
        {
            for (int i = 0; i < jobsToAdd.Count; i++)
            {
                jobs.Add(jobsToAdd[i].Item1);
                jobTimestamps.Add(jobsToAdd[i].Item2);
            }
        }

        public List<Tuple<Action, long>> GetJobs()
        {
            List<Tuple<Action, long>> result = new List<Tuple<Action, long>>();
            for (int i = 0; i < jobs.Count; i++)
            {
                result.Add(Tuple.Create(jobs[i], jobTimestamps[i]));
            }

            return result;
        }
    }
}
