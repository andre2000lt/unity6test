using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleWindow : MonoBehaviour
{
    public static ConsoleWindow Instance;

    [SerializeField] private TMP_Text _outputLine;


    private void Awake()
    {
        Instance = this;
    }


    public static void Log(string text)
    {
        Instance._outputLine.text = text;
    }
}
