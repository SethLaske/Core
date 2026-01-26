namespace Core.Scripts
{
    public interface ISaveable
    {
        string saveID { get; }

        public string SaveData();
    }
}
