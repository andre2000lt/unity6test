using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsDisplay : MonoBehaviour
{
    public static FpsDisplay Instance;  

    [SerializeField] private TMP_Text _fpsOutput;
    private float _timePolling = 1f;
    private float _time = 0;

    int _frameCount;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //Application.targetFrameRate = 60;
    }

    void Update()
    {
        _time += Time.deltaTime;
        _frameCount++;

        if(_time >= _timePolling)
        {
            int fps = Mathf.RoundToInt(_frameCount / _timePolling);
            _fpsOutput.text = fps.ToString();
            _time = 0;
            _frameCount = 0;
        }
    }
}
