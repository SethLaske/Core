namespace Core.Scripts
{
    public interface ISaveable
    {
        public void SaveData(SaveFile argSaveFile);

        public void LoadData(SaveFile argSaveFile);
    }
}
