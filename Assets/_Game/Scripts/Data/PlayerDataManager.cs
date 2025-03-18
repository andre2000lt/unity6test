using System;
using UnityEngine;
using UnityEngine.Events;


public static class PlayerDataManager 
{
    public static UnityEvent DataChanged_Event;
    public static UnityEvent TargetChanged_Event;
    public static UnityEvent RecordChanged_Event;

    public static int MinTargetBlockIndex = 8;

    public static void Init()
    {
        DataChanged_Event = new UnityEvent();
        TargetChanged_Event = new UnityEvent();
        RecordChanged_Event = new UnityEvent();

        LoadData();
    }


    public static void SaveData()
    {
        if (GetScore() > Saver.SavesData.PlayerRecord)
        {
            Saver.SavesData.PlayerRecord = GetScore();

            RecordChanged_Event?.Invoke();
        }

        // TODO: YandexGame.SaveProgress();
        Saver.SaveProgress(StorageType.TextFile);
    }


    public static void LoadData()
    {
        // TODO: YandexGame.LoadProgress();
        Saver.LoadProgress(StorageType.TextFile);
    }


    public static void ResetData()
    {
        Saver.SavesData.PlayerData = new PlayerData();

        SaveData();
    }


    public static void ResetBackup()
    {
        Saver.SavesData.Backup = new Backup();

        SaveData();
    }


    public static void SaveBackup()
    {
        Saver.SavesData.Backup.PlayerData = (PlayerData)Saver.SavesData.PlayerData.Clone();
        Saver.SavesData.Backup.Record = Saver.SavesData.PlayerRecord;
    }


    public static void RollDataBack()
    {
        SetMovesToActivateRollback(5);

        Saver.SavesData.PlayerData = (PlayerData)Saver.SavesData.Backup.PlayerData.Clone();
        Saver.SavesData.PlayerRecord = Saver.SavesData.Backup.Record;

        SaveData();

        GameScenesController.LoadScene(GameScene.Main);
    }


    public static void ContinueAfterLoss()
    {
        Saver.SavesData.PlayerData.LiveCount = 5;
        SetTryCount(0);

        GameScenesController.LoadScene(GameScene.Main);
    }





    //Getters
    public static int GetScore()
    {
        return Saver.SavesData.PlayerData.Score;
    } 
    
    public static int GetBackupScore()
    {
        return Saver.SavesData.Backup.PlayerData.Score;
    }
    
    public static int GetLiveCount()
    {
        return Saver.SavesData.PlayerData.LiveCount;
    }
    
    
    public static int GetTargetBlockIndex()
    {
        return Saver.SavesData.PlayerData.TargetBlockIndex;
    }
    

    public static int GetMinTargetBlockIndex()
    {
        return MinTargetBlockIndex;
    }


    public static int GetReward()
    {
        int rang = GetTargetBlockIndex() - MinTargetBlockIndex;
        int reward = 0;
        int rangBonus = 50;

        for (int i = 0; i <= rang; i++)
        {
            reward += 100 + (rangBonus * i);
        }

        return reward;
    }


    public static int[,] GetMatrixMap()
    {
        return Saver.SavesData.PlayerData.MatrixMap;
    }


    public static int GetRecord()
    {
        return Saver.SavesData.PlayerRecord;
    }


    public static int GetMovesToActivateRollback()
    {
        return Saver.SavesData.MovesToActivateButton;
    }


    public static int GetTryCount()
    {
        return Saver.SavesData.TryCount;
    }


    public static bool IsSoundOn()
    {
        return Saver.SavesData.IsSoundOn;
    }



    //Setters
    public static void SetScore(int score)
    {
        Saver.SavesData.PlayerData.Score = score;

        DataChanged_Event?.Invoke();
    }
    
    
    public static void IncreaseScore(int value)
    {
        Saver.SavesData.PlayerData.Score += value;

        DataChanged_Event?.Invoke();
    }


    public static void SetLiveCount(int liveCount)
    {
        Saver.SavesData.PlayerData.LiveCount = liveCount;

        DataChanged_Event?.Invoke();
    }


    public static void IncreaseTargetBlockIndex()
    {
        Saver.SavesData.PlayerData.TargetBlockIndex += 1;

        TargetChanged_Event?.Invoke();
    }


    public static void SetMatrixMap(Cell[,] cells)
    {
        for(int y = 0; y < cells.GetLength(0); y++)
        {
            for (int i = 0; i< cells.GetLength(0); i++)
            {
                Saver.SavesData.PlayerData.MatrixMap[y, i] = cells[y, i].Block.Index;
            }
        }
    }

    public static void SetSoundOn()
    {
        Saver.SavesData.IsSoundOn = true;
        SaveData();
    } 
    
    
    public static void SetSoundOff()
    {
        Saver.SavesData.IsSoundOn = false;
        SaveData();
    }


    public static void SetTryCount(int tryCount)
    {
        Saver.SavesData.TryCount = tryCount;
        SaveData();
    }


    public static void SetMovesToActivateRollback(int count)
    {
        Saver.SavesData.MovesToActivateButton = count;
        SaveData();
    }


    public static void SetTutorialViewed()
    {
        Saver.SavesData.IsTutorialViewed = true;
        SaveData();
    }


    public static void MarkFirstMove()
    {
        Saver.SavesData.IsFirstMoveMade = true;
        SaveData();
    }
}





public class PlayerData : ICloneable
{
    public int Score = 0;
    public int LiveCount = 5;
    public int TargetBlockIndex = 6;

    public int[,] MatrixMap;

    public PlayerData() 
    {
        TargetBlockIndex = PlayerDataManager.MinTargetBlockIndex;
        MatrixMap = new int[5, 5];
    }


    public PlayerData(int score, int liveCount, int targetBlockIndex, int[,] matrixMap)
    {
        Score = score;
        LiveCount = liveCount;
        TargetBlockIndex = targetBlockIndex;

        MatrixMap = new int[5, 5];

        for (int y = 0; y < matrixMap.GetLength(0); y++)
        {
            for (int i = 0; i < matrixMap.GetLength(0); i++)
            {
                MatrixMap[y, i] = matrixMap[y, i];
            }
        }
    }

    public object Clone()
    {
        return new PlayerData(Score, LiveCount, TargetBlockIndex, MatrixMap);
    }
}


public class Backup
{
    public PlayerData PlayerData = new PlayerData();
    public int Record = 0;
}
