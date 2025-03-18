using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float targetAspect = 9f / 16f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        float newWidth = targetAspect * Screen.height;
        float newHeight = Screen.height;
        float w = newWidth / Screen.width;
        float x = (1 - w) / 2;
        float h = 1;
        float y = 0;

        if(newWidth > Screen.width)
        {
            newHeight = Screen.width / targetAspect;
            newWidth = Screen.width;

            w = 1;
            x = 0;
            h = newHeight / Screen.height;
            y = (1 - h) / 2; ;
        }


        _camera.rect = new Rect(x, y, w, h);
    }
}
// w / currentHeigh = targetAspect , w = targetAspect * height
// currentWidth / h = targetAspect , h = currentWidth / targetAspect 