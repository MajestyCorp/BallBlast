using UnityEngine;

namespace BallBlast
{
    public class DefaultPlayerPrefs : ICustomPlayerPrefs
    {
        void ICustomPlayerPrefs.DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        float ICustomPlayerPrefs.GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        int ICustomPlayerPrefs.GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        string ICustomPlayerPrefs.GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        bool ICustomPlayerPrefs.HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        void ICustomPlayerPrefs.Save()
        {
        }

        void ICustomPlayerPrefs.SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        void ICustomPlayerPrefs.SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        void ICustomPlayerPrefs.SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}