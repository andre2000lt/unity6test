using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HandPointer: MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private RectTransform _rectTransform;
    
    private void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));
        
        var handPosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        transform.position = mousePosition;

        if(Input.GetMouseButtonDown(0)) {
            _animator.SetTrigger("Click");
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(Input.mousePosition);
        }
    }
}
