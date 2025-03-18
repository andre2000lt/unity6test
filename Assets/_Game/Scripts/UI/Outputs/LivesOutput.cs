using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesOutput : MonoBehaviour
{
    [SerializeField] private List<Image> _livesOutput;

    private int _lives = 0;


    public void Init()
    {
        PlayerDataManager.DataChanged_Event.AddListener(UpdateLiveCount);
        UpdateLiveCount();
    }


    private void UpdateLiveCount()
    {
        int lives = PlayerDataManager.GetLiveCount();
        if (lives == _lives) return;

        _lives = lives;
        
        foreach (var live in _livesOutput)
        {
            live.enabled = false;
        }

        for (int i = 0; i < _lives; i++)
        {
            _livesOutput[i].enabled = true;
        }
    }
}
