using UnityEngine;

namespace Core.Scripts
{
    public class GameManager : ManagerBase<GameManager>
    {
        private bool initialized = false;

        private CoreStats coreStats;

        public override void Awake()
        {
            base.Awake();

            coreStats = new CoreStats();
        }

        public override void DoFirstUpdate()
        {
            base.DoFirstUpdate();
            
            coreStats.DoFirstUpdate();
        }

        public void Update()
        {
            if (initialized == false)
            {
                DoFirstUpdate();
                initialized = true;
            }

            DoUpdate(new TimeValues(Time.deltaTime, 1));
        }

        public override void DoUpdate(TimeValues argTime)
        {
            if (initialized == false)
            {
                CoreLogger.LogError("Game Manager not initialized", true);
            }
            
            coreStats.DoUpdate(argTime);

            IntervalManager.instance.DoUpdate(argTime);

            if (Input.GetMouseButtonDown(0))
            {
                new Interval(1, 2, 
                    () => {CoreLogger.Log("Delay Finished"); }, 
                    (t) => {CoreLogger.Log($"Progress {t}"); },
                    () =>
                    {
                        CoreLogger.Log("Duration Finished");
                        SavePlayerData();
                    }).Start();
                
            }
        }

        public void SavePlayerData()
        {
            SaveSingleton.instance.SaveData();
        }
    }
}
