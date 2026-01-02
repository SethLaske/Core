using System.Collections;
using System.Collections.Generic;
using Core.Scripts;
using UnityEngine;

public class IntervalManager : ManagerBase<GameManager>, ISaveable
{
    private List<Interval> allIntervals = new List<Interval>();
    
    private List<Interval> newIntervals = new List<Interval>();
    private List<Interval> endedIntervals = new List<Interval>();

    public override void DoUpdate(TimeValues argTime)
    {
        if (newIntervals.Count > 0)
        {
            foreach (Interval newCountdown in newIntervals)
            {
                allIntervals.Add(newCountdown);
            }
            newIntervals.Clear();
        }

        if (endedIntervals.Count > 0) 
        {
            foreach (Interval endedCountdown in endedIntervals)
            {
                if (allIntervals.Contains(endedCountdown))
                {
                    allIntervals.Remove(endedCountdown);
                }
            }
            endedIntervals.Clear();
        }

        foreach (Interval countdown in allIntervals)
        {
            //countdown.UpdateTimer(delta);
        }
    }

    //New and ended countdowns are handled seperately to avoid errors with foreach loops
    public void AddNewCountdown(Interval argCountdown) 
    { 
        newIntervals.Add(argCountdown);
    }

    public void AddEndedCountdown(Interval argEndedCountdown)
    {
        endedIntervals.Add(argEndedCountdown);
    }

}
