using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Services : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
