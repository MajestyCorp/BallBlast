using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BallBlast
{
    public class WebGLPlayerPrefs : ICustomPlayerPrefs
    {
        private Dictionary<string, ValueData> _data = new();
        private string _path = "idbfs/SpaceTurretRunner/data";
        private string _folder = "idbfs/SpaceTurretRunner/";

        public WebGLPlayerPrefs()
        {
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            if (File.Exists(_path))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    using (FileStream fs = File.Open(_path, FileMode.Open))
                    {
                        _data = (Dictionary<string, ValueData>)bf.Deserialize(fs);
                    }
                }
                catch
                {
                    _data = new();
                }
            }
        }

        void ICustomPlayerPrefs.DeleteKey(string key)
        {
            if (_data.ContainsKey(key))
                _data.Remove(key);
        }

        float ICustomPlayerPrefs.GetFloat(string key)
        {
            if (_data.TryGetValue(key, out ValueData value))
                return value.FloatValue;
            else
                return 0f;
        }

        int ICustomPlayerPrefs.GetInt(string key)
        {
            if (_data.TryGetValue(key, out ValueData value))
                return value.IntValue;
            else
                return 0;
        }

        string ICustomPlayerPrefs.GetString(string key)
        {
            if (_data.TryGetValue(key, out ValueData value))
                return value.StringValue;
            else
                return null;
        }

        bool ICustomPlayerPrefs.HasKey(string key)
        {
            return _data.ContainsKey(key);
        }

        void ICustomPlayerPrefs.Save()
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream fs = File.Create(_path))
            {
                bf.Serialize(fs, _data);
            }

            PlayerPrefs.SetString("forceSave", string.Empty);
            PlayerPrefs.Save();
        }

        void ICustomPlayerPrefs.SetFloat(string key, float value)
        {
            if (!_data.TryGetValue(key, out var valueData))
                _data[key] = new ValueData(value);
            else
                valueData.FloatValue = value;
        }

        void ICustomPlayerPrefs.SetInt(string key, int value)
        {
            if (!_data.TryGetValue(key, out var valueData))
                _data[key] = new ValueData(value);
            else
                valueData.IntValue = value;
        }

        void ICustomPlayerPrefs.SetString(string key, string value)
        {
            if (!_data.TryGetValue(key, out var valueData))
                _data[key] = new ValueData(value);
            else
                valueData.StringValue = value;
        }

        [System.Serializable]
        private class ValueData
        {
            public float FloatValue;
            public string StringValue;
            public int IntValue;

            public ValueData(float value)
            {
                FloatValue = value;
                StringValue = null;
                IntValue = 0;
            }

            public ValueData(string value)
            {
                FloatValue = 0f;
                StringValue = value;
                IntValue = 0;
            }

            public ValueData(int value)
            {
                FloatValue = 0f;
                StringValue = null;
                IntValue = value;
            }
        }
    }
}