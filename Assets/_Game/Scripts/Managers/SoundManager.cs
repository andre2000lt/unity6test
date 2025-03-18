using System;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    [SerializeField] private List<SoundData> _soundDataList;


    private void Awake()
    {
        _instance = this;
    }



    public static void PlaySound(SoundName soundName)
    {
        if(PlayerDataManager.IsSoundOn() == false) return;

        var soundData = _instance._soundDataList.Find(soundData => soundName == soundData.SoundName);
        soundData.AudioSource.Play();
    }

}



[Serializable]
public class SoundData
{
    public SoundName SoundName;
    public AudioSource AudioSource;
}


public enum SoundName
{
    CellClick, 
    BlocksMerge, 
    GoalCompleted, 
    AcceptReward,
    CancelReward, 
    GameOver,
    CellClickError,
    NeutralClick
}
