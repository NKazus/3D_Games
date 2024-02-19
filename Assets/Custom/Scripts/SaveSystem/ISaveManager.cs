namespace CMGame.Saves
{
    public interface ISaveManager
    {
        public bool SaveData<T>(string relativePath, T data, bool encrypted);

        public T LoadData<T>(string relativePath, bool encrypted);
    }
}
