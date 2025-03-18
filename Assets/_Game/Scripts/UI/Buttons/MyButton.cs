using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MyButton : Button
{
    private CanvasGroup _canvasGroup;


    protected override void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    public void DisableButton()
    {
        this.enabled = false;
        _canvasGroup.alpha = 0.3f;
    }


    public void EnableButton()
    {
        this.enabled = true;
        _canvasGroup.alpha = 1;
    }
}
