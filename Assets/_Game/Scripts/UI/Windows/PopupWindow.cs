using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PopupWindow : MonoBehaviour
{
    public static UnityEvent WindowOpened_Event;
    public static UnityEvent WindowClosed_Event;

    [SerializeField] protected float _windowScaleSpeed = 3f;

    protected UnityAction _onComplete_Action;


    public static void InitEvents()
    {
        WindowOpened_Event = new UnityEvent();
        WindowClosed_Event =  new UnityEvent();
    }


    protected virtual void Show()
    {
        EffectsManager.Scale(gameObject, _onComplete_Action);
    }


    private void OnEnable()
    {
        WindowOpened_Event?.Invoke();

        _onComplete_Action += DoActionAfterLoad;
        Show();
    }
    
    
    private void OnDisable()
    {
        WindowClosed_Event?.Invoke();
    }


    protected virtual void DoActionAfterLoad()
    {

    }

}
