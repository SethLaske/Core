using System.Collections.Generic;

namespace Core.Scripts
{
    public class SaveFile
    {
        private readonly Dictionary<string, object> savedData;

        public SaveFile()
        {
            savedData = new Dictionary<string, object>();
        }

        public void SaveString(string argKey, string argStringValue)
        {
            savedData[argKey] = argStringValue;
        }

        public string GetString(string argKey, string argDefault = "")
        {
            if (savedData.TryGetValue(argKey, out object stringValue))
            {
                return stringValue.ToString();
            }

            return argDefault;
        }

        public void SaveInt(string argKey, int argValue)
        {
            savedData[argKey] = argValue;
        }

        public int GetInt(string argKey, int argDefault = 0)
        {
            if (savedData.TryGetValue(argKey, out object value))
            {
                if (value is int intValue)
                {
                    return intValue;
                }
            }

            return argDefault;
        }
        
    }
}
