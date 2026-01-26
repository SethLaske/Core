using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core.Scripts
{
    /// All data is saved at once, and can be loaded at anytime
    /// Data is sorted by save keys, and saving to the save key will overwrite any data previously saved there
    /// Data is stored at Application.persistent path
    public class SaveSingleton : SingletonBase<SaveSingleton>
    {
        private const string DEFAULT_SAVE_FILE = "Save1.json";
        private static string SavePath => Path.Combine(Application.persistentDataPath, DEFAULT_SAVE_FILE);

        private Dictionary<string, ISaveable> objectsToSave;

        private SaveFile activeSaveFile = null;

        public override void Awake()
        {
            base.Awake();

            activeSaveFile = new SaveFile();
            objectsToSave = new Dictionary<string, ISaveable>();

            LoadFromDisk();
        }

        private void LoadFromDisk()
        {
            if (File.Exists(SavePath))
            {
                try
                {
                    string json = File.ReadAllText(SavePath);
                    activeSaveFile.Deserialize(json);
                    CoreLogger.Log($"Save file loaded from {SavePath}:\n{json}");
                }
                catch (System.Exception e)
                {
                    CoreLogger.LogError($"Failed to load save file: {e.Message}");
                }
            }
            else
            {
                CoreLogger.Log($"No save file existed at {SavePath}");
            }
        }

        private void SaveDataToDisk(string argDataToSave)
        {
            try
            {
                File.WriteAllText(SavePath, argDataToSave);
                CoreLogger.Log($"Data saved successfully to {SavePath}:\n{argDataToSave}");
            }
            catch (System.Exception e)
            {
                CoreLogger.LogError($"Failed to save to disk: {e.Message}");
            }
        }

        public void AddToSaveList(ISaveable argObjectToSave)
        {
            objectsToSave[argObjectToSave.saveID] = argObjectToSave;
        }

        public void RemoveFromSaveList(ISaveable argObjectToNotSave)
        {
            // Save data before an object is removed
            SaveData();

            objectsToSave.Remove(argObjectToNotSave.saveID);
        }

        public void SaveData()
        {
            foreach (KeyValuePair<string, ISaveable> saveable in objectsToSave)
            {
                activeSaveFile.InsertSavedJson(saveable.Value.saveID, saveable.Value.SaveData());
            }

            SaveDataToDisk(activeSaveFile.Serialize());
        }

        public bool TryLoadSavedData(string argSaveKey, SaveObject argSaveObject)
        {
            if (activeSaveFile.TryGetSavedJson(argSaveKey, out string savedJson) == false)
            {
                return false;
            }

            argSaveObject.SetData(savedJson);
            return true;
        }

        // Unity Editor Menu Item
#if UNITY_EDITOR
        [MenuItem("Core/Clear Save File")]
        public static void ClearSaveFile()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                CoreLogger.Log("Save file deleted successfully.");
            }
            else
            {
                CoreLogger.Log("No save file found to delete.", LogType.Warning);
            }
        }
#endif
    }
}
