using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/ColorList")]
public class ColorList : ScriptableObject
{
    [SerializeField] private Color[] _colors;


    public Color GetColorByIndex(int index)
    {
        //int limitedIndex = (index > 9) ? index - 10 : index - 1;
        if(index > 15) index = Random.Range(9, 16);
        var color = _colors[index - 1];
        var formatedColor = new Color(color.r, color.g, color.b);

        return formatedColor;
    }
}
