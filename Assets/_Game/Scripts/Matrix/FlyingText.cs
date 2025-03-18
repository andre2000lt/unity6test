using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class FlyingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;



    public void Init(string textValue, FlyingTextType type)
    {
        StartCoroutine(AnimateRoutine(textValue, type));
    }


    private IEnumerator AnimateRoutine(string textValue, FlyingTextType type)
    {
        if(type == FlyingTextType.Points)
        {
            _text.text = "+" + textValue;
        } 
        else if (type == FlyingTextType.TargetReward)
        {
            transform.localScale = Vector3.one * 3f;
            _text.text = "+" + textValue;
        }

        var alfa = _text.color.a;
        Vector3 newPosition = new Vector3(_text.transform.position.x, _text.transform.position.y + 3, _text.transform.position.z);
        for (float i = 1; i > 0; i-= Time.deltaTime / 2)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alfa * i);
            _text.transform.position = Vector3.MoveTowards(_text.transform.position, newPosition, Time.deltaTime * 2);
            yield return null;
        }

        Destroy(gameObject);
    }
}



public enum FlyingTextType
{
    Points,
    TargetReward,
    Combo
}