
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Block ничего не знает о других классах и родителе.
/// Содердит индекс и цвет. И отображение.
/// Управляется из класса Cell.
/// </summary>
public class Block : MonoBehaviour
{
    [SerializeField] private TMP_Text _indexText;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private SpriteRenderer _image;
    [SerializeField] private Image _level2Image;
    [SerializeField] private ColorList _colorList;
    [SerializeField] private Animator _animator;

    public static bool IsAnimating = false;
    public int Index;


    public void Init(int index)
    {
        int max;

        if(index == 0)
        {
            if(GameMatrix.MatrixMode == MatrixMode.GenerateMatrix)
            {
                max = 6;
            } else
            {
                max = 7;
            }

            SetIndex(Random.Range(1, max));
        }
        else
        {
            SetIndex(index);
        }
    }


    public void IncreaseIndex()
    {
        SetIndex(Index + 1);
    }
    

    public void SetOrder(int order)
    {
        _image.sortingOrder = order;
        _canvas.sortingOrder = order;
    }

    public void Kill()
    {
        SetOrder(-5);
        _canvasGroup.alpha = 0;
        Destroy(gameObject);
    }


    private void SetIndex(int index)
    {
        Index = index;

        _indexText.text = Index.ToString();

        _image.color = _colorList.GetColorByIndex(index);

        if (Index >= PlayerDataManager.GetMinTargetBlockIndex())
        {
            _level2Image.enabled = true;
        }
    }



    //Animations
    public void IncreaseIndexAppearAnimation()
    {
        IsAnimating = true;
        _animator.SetTrigger("Appear");
    }    
    
    
    public void IncreaseIndexRotateAnimation()
    {
        IsAnimating = true;
        _animator.SetTrigger("Rotate");
    }


    public void EndAnimation()
    {
        IsAnimating = false;
    }
}
