using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Scripts
{
    public class SaveFile
    {
        // We use a dictionary directly. Newtonsoft knows how to handle this.
        private Dictionary<string, string> savedJsons;

        public SaveFile()
        {
            savedJsons = new Dictionary<string, string>();
        }

        public void InsertSavedJson(string argSaveKey, string argSaveObject)
        {
            savedJsons[argSaveKey] = argSaveObject;
        }

        public bool TryGetSavedJson(string argSaveKey, out string saveObject)
        {
            return savedJsons.TryGetValue(argSaveKey, out saveObject);
        }

        public string Serialize()
        {
            // Use the full namespace to resolve the ambiguity
            return JsonConvert.SerializeObject(savedJsons, Newtonsoft.Json.Formatting.Indented);
        }

        public void Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            savedJsons = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}