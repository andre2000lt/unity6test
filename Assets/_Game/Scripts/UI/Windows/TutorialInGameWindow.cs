using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialInGameWindow : PopupWindow
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _pointerImage;

    private void Awake()
    {
        _closeButton.onClick.AddListener(CloseWindow);
    }


    //Listeners
    private void CloseWindow()
    {
        gameObject.SetActive(false);
    }


    protected override void DoActionAfterLoad()
    {
        _pointerImage.enabled = true;
    }
}
