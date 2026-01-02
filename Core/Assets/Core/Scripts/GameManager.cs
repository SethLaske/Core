using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts
{
    public class GameManager : ManagerBase<GameManager>, ISaveable
    {
        private bool initialized = false;

        private SaveFile activeSaveFile;

        public override void Awake()
        {
            base.Awake();

            activeSaveFile = new SaveFile();
        }

        public void Update()
        {
            if (initialized == false)
            {
                LoadPlayerData();
                DoFirstUpdate();
                initialized = true;
            }

            DoUpdate(new TimeValues(Time.deltaTime, 1));
        }

        public override void DoUpdate(TimeValues argTime)
        {
            if (initialized == false)
            {
                Logger.LogError("Game Manager not initialized", true);
            }
        }

        public void LoadPlayerData()
        {

        }

        public void SavePlayerData()
        {

        }
    }
}
