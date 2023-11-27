using _SaveSystem.Data;

namespace _SaveSystem
{
    public interface IDataPersistence
    {
        void SaveData(GameData data);
        void LoadData(GameData data);
    }
}
