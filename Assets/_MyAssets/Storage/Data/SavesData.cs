using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesData 
{
    public string Language = "";
    public PlayerData PlayerData;
    public Backup Backup;
    public int PlayerRecord = 0;
    public bool IsSoundOn = true;
    public int MovesToActivateButton = 0;
    public int TryCount = 1;
    public bool IsTutorialViewed = false;
    public bool IsFirstMoveMade = false;
    public bool IsGameReadyInited = false;



    public SavesData()
    {
        PlayerData = new PlayerData();
        Backup = new Backup();
    }
}
