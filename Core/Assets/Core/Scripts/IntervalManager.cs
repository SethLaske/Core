using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts
{
    public class IntervalManager : ManagerBase<IntervalManager>
    {
        private List<Interval> runningIntervals = new List<Interval>();
        private List<Interval> queuedIntervals = new List<Interval>();

        public override void DoUpdate(TimeValues argTime)
        {
            if (queuedIntervals.Count > 0)
            {
                runningIntervals.AddRange(queuedIntervals);
                queuedIntervals.Clear();
            }

            List<Interval> finishedIntervals = new List<Interval>();
            
            foreach (Interval interval in runningIntervals)
            {
                if (interval.UpdateInterval(argTime.deltaTime))
                {
                    finishedIntervals.Add(interval);
                }
            }

            while (finishedIntervals.Count > 0)
            {
                runningIntervals.Remove(finishedIntervals[0]);
                finishedIntervals.RemoveAt(0);
            }
        }

        public void AddInterval(Interval interval)
        {
            queuedIntervals.Add(interval);
        }
    }
}