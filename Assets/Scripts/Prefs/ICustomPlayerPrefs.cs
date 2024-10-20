namespace BallBlast
{
    public interface ICustomPlayerPrefs
    {
        bool HasKey(string key);

        int GetInt(string key);

        void SetInt(string key, int value);

        float GetFloat(string key);

        void SetFloat(string key, float value);

        string GetString(string key);

        void SetString(string key, string value);

        void DeleteKey(string key);

        void Save();
    }
}