using UnityEngine;

namespace Core.Scripts
{
    public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
    {
        public static T instance { get; private set; }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
        }
    }
    
    public abstract class ManagerBase<T> : SingletonBase<T>, ISaveable, IUpdateable where T : ManagerBase<T>
    {
        public virtual void DoFirstUpdate()
        {
            SaveSingleton.instance.AddToSaveList(this);
            LoadData();
        }

        public virtual void DoUpdate(TimeValues argTime) { }

        public virtual string saveID => "DEFAULT";

        public virtual string SaveData()
        {
            return "";
        }
        
        protected virtual void LoadData() { }
    }
}
