using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Image _background;

    private static LoadingScreen _instance;

    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public static void Show()
    {
        _instance._background.gameObject.SetActive(true);
    }
    

    public static void Hide()
    {
        _instance._background.gameObject.SetActive(false);
    }


    public static void SetProgressBarValue(float value)
    {
        _instance._progressBar.value = value;
    }
}
