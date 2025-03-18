using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Saver
{
    private static string _key = "SavesData";

    private static PlayerPrefsStorage PlayerPrefs_Storage = new PlayerPrefsStorage(_key);
    private static TextFileStorage TextFile_Storage = new TextFileStorage(_key);

    public static SavesData SavesData = new SavesData();


     public static void SaveProgress(StorageType strorageType) 
     {
        switch (strorageType)
        {
            case StorageType.PlayerPrefs:
                PlayerPrefs_Storage.Save(SavesData);
                break;

            case StorageType.TextFile:
                TextFile_Storage.Save(SavesData);
                break;
            default:
                break;
        }
    }


    public static void LoadProgress(StorageType strorageType)
    {
        switch (strorageType)
        {
            case StorageType.PlayerPrefs:
                SavesData = PlayerPrefs_Storage.Load<SavesData>();
                break;
            case StorageType.TextFile:
                SavesData = TextFile_Storage.Load<SavesData>();
                break;
            default:
                SavesData = TextFile_Storage.Load<SavesData>();
                break;
        }

      
    }
}



public enum StorageType
{
    PlayerPrefs, 
    TextFile,
    GamePush,
    Yandex
}
