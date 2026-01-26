using UnityEngine;

namespace Core.Scripts
{
    [System.Serializable]
    public abstract class SaveObject
    {
        public abstract string GetData();

        public abstract void SetData(string argJson);
    }

    public class SaveObject<T> : SaveObject
    {
        public override string GetData()
        {
            return JsonUtility.ToJson(this);
        }

        public override void SetData(string argJson)
        {
            JsonUtility.FromJsonOverwrite(argJson, this);
        }
    }
}
