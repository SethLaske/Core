using UnityEngine;

namespace Core.Scripts
{
    public abstract class ManagerBase<T> : MonoBehaviour where T : ManagerBase<T>, ISaveable
    {
        public static T instance;

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
        }

        protected virtual void DoFirstUpdate()
        {

        }

        public virtual void DoUpdate(TimeValues argTime)
        {

        }

        public virtual void SaveData(SaveFile argSaveFile)
        {

        }

        public virtual void LoadData(SaveFile argSaveFile)
        {

        }
    }
}
