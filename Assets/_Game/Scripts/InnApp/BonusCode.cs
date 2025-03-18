using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BonusCode : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Button _button;

    private List<string> _bonusCodes = new List<string>();


    private void Awake()
    {
        _button.onClick.AddListener(ButtonClickHandler);
    }


    private void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("bonus_codes");
        var codes = textAsset.text.Split(new char[] {'\n'});

        foreach (string code in codes)
        {
            var str = code.Trim();
            _bonusCodes.Add(str);
        }
    }




    // Listeners
    private void ButtonClickHandler()
    {
        string enteredCode = _input.text;
        if(_bonusCodes.Contains(enteredCode))
        {
            InAppData.DisableAds();
        }
        else
        {
            _input.text = "";
        }
    }
}
