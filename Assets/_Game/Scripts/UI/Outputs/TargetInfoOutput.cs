using System;
using TMPro;
using UnityEngine;

public class TargetInfoOutput : MonoBehaviour
{
    [SerializeField] private TargetBlock _targetBlock;
    [SerializeField] private TMP_Text _rewardOutput;


    public void Init()
    {
        PlayerDataManager.TargetChanged_Event.AddListener(UpdateTarget);
        UpdateTarget();
    }



    //Listeners
    private void UpdateTarget()
    {
        int targetIndex = PlayerDataManager.GetTargetBlockIndex();
        _targetBlock.SetIndex(targetIndex);

        _rewardOutput.text = PlayerDataManager.GetReward().ToString();
    }
}
