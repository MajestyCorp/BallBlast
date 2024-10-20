using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    public static class CustomPlayerPrefs
    {
        private static ICustomPlayerPrefs _prefs;
        public static void Initialize(ICustomPlayerPrefs prefs)
        {
            _prefs = prefs;
        }

        public static bool HasKey(string key) => _prefs.HasKey(key);
        public static int GetInt(string key) => _prefs.GetInt(key);
        public static void SetInt(string key, int value) => _prefs.SetInt(key, value);
        public static float GetFloat(string key) => _prefs.GetFloat(key);
        public static void SetFloat(string key, float value) => _prefs.SetFloat(key, value);
        public static string GetString(string key) => _prefs.GetString(key);
        public static void SetString(string key, string value) => _prefs.SetString(key, value);
        public static void DeleteKey(string key) => _prefs.DeleteKey(key);
        public static void Save() => _prefs?.Save();
    }
}