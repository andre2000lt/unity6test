using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsManager : MonoBehaviour
{
    private static EffectsManager _instance;

    [SerializeField] private float _scaleSpeed = 3;


    private void Awake()
    {
        _instance = this;
    }


    public static  void Scale(GameObject _gameObject, UnityAction OnComplete)
    {
        _instance.StartCoroutine(_instance.ScaleRoutine(_gameObject, OnComplete));
    }


    private IEnumerator ScaleRoutine(GameObject _gameObject, UnityAction OnComplete)
    {
        _gameObject.transform.localScale = Vector3.zero;

        while (true)
        {
            _gameObject.transform.localScale = Vector3.MoveTowards(_gameObject.transform.localScale, Vector3.one, Time.deltaTime * _scaleSpeed);
            yield return null;

            if (_gameObject.transform.localScale.x == 1) break;
        }

        OnComplete?.Invoke();
    }
}
