using System;

namespace Core.Scripts
{
    public class CoreStats : IUpdateable, ISaveable
    {
        public string saveID => "CoreStats";

        private CoreStatsSaveObject loadedStats;

        private float gameTimeSinceLastSave;
        private DateTime timeOfLastSave;

        public void DoFirstUpdate()
        {
            SaveSingleton.instance.AddToSaveList(this);
            LoadData();
        }

        public void DoUpdate(TimeValues argTime)
        {
            gameTimeSinceLastSave += argTime.deltaTime;
        }

        public string SaveData()
        {
            CoreStatsSaveObject newSave = new CoreStatsSaveObject(loadedStats);
            newSave.realTime += (DateTime.UtcNow - timeOfLastSave).TotalSeconds;
            newSave.gameTime += gameTimeSinceLastSave;

            gameTimeSinceLastSave = 0;
            timeOfLastSave = DateTime.UtcNow;
            loadedStats = newSave;

            return newSave.GetData();
        }

        private void LoadData()
        {
            loadedStats = new CoreStatsSaveObject();
            SaveSingleton.instance.TryLoadSavedData(saveID, loadedStats);
            timeOfLastSave = DateTime.UtcNow;
        }

        [Serializable]
        public class CoreStatsSaveObject : SaveObject<CoreStatsSaveObject>
        {
            public double realTime;
            public float gameTime;

            public CoreStatsSaveObject()
            {
                realTime = 0;
                gameTime = 0;
            }

            public CoreStatsSaveObject(CoreStatsSaveObject argOther)
            {
                realTime = argOther.realTime;
                gameTime = argOther.gameTime;
            }
        }
    }
}
