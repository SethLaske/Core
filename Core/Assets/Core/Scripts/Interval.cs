using System;
using UnityEngine;

namespace Core.Scripts
{
    public class Interval
    {
        private readonly float duration;
        private readonly float delay;
        
        private readonly Action onStarted;
        private readonly Action<float> onUpdate; // float is 0.0 to 1.0
        private readonly Action onFinished;

        private float totalTime;
        private bool isStarted;
        private bool isFinished;
        
        public Interval(float argDuration = 0, float argDelay = 0f, Action argOnStarted = null, Action<float> argOnUpdate = null, Action argOnFinished = null)
        {
            duration = Mathf.Max(argDuration, 0.001f); // Avoid division by zero
            delay = Mathf.Max(argDelay, 0);
            totalTime = 0f;

            onStarted = argOnStarted;
            onUpdate = argOnUpdate;
            onFinished = argOnFinished;
        }

        public bool UpdateInterval(float deltaTime)
        {
            if (isFinished)
            {
                return true;
            }

            totalTime += deltaTime;

            // Handle Delay Phase
            if (isStarted == false)
            {
                if (totalTime >= delay)
                {
                    isStarted = true;
                    onStarted?.Invoke();
                }
                else return false; 
            }

            // Handle Active Phase
            float activeTime = totalTime - delay;
            float percent = Mathf.Clamp01(activeTime / duration);
            
            onUpdate?.Invoke(percent);

            if (activeTime >= duration)
            {
                Finish();
                return true;
            }

            return false;
        }

        private void Finish()
        {
            if (isFinished)
            {
                return;
            }
            
            isFinished = true;
            onFinished?.Invoke();
        }

        public void Start()
        {
            IntervalManager.instance.AddInterval(this);
        }

        // Calling cancel will immediately trigger the callback if enabled
        public void Cancel(bool argShouldFinish)
        {
            if (isFinished)
            {
                return;
            }

            if (argShouldFinish)
            {
                onFinished?.Invoke();
            }
            isFinished = true; // Mark as finished so the manager removes it
        }
    }
}