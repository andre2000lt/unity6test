using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrounCamera : MonoBehaviour
{
    public static BackgrounCamera Instance;
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

    }
}
