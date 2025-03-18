
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberOutput;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private ColorList _colorList;

    private int Index = 9;


    public void SetIndex(int index)
    {
        Index = index;
        _numberOutput.text = Index.ToString();
        _backgroundImage.color = _colorList.GetColorByIndex(index);
    }
}
