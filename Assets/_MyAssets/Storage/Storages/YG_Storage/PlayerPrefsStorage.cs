using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerPrefsStorage : IStorage
{
    private const string DEFAULT_KEY = "PlayerPrefsStorage";
    private string _key;

    public PlayerPrefsStorage(string key = DEFAULT_KEY)
    {
        _key = key;
    }


    public void Save<T>(T data) where T : new()
    {
        string jsonData = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_key, jsonData);
    }


    public T Load<T>() where T : new()
    {
        var data = new T();

        if(PlayerPrefs.HasKey(_key))
        {
            string jsonData = PlayerPrefs.GetString(_key);
            data = JsonConvert.DeserializeObject<T>(jsonData);
        }

        return data;
    }


    public bool IsStorageEmpty(string key)
    {
        return !PlayerPrefs.HasKey(_key);
    }


    public void DeleteKey()
    {
        PlayerPrefs.DeleteKey(_key);
    }

    public string GetPath()
    {
        throw new System.NotImplementedException();
    }
}
