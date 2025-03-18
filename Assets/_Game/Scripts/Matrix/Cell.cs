using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


/// <summary>
/// Содержит информацию о соседних ячейках и своем блоке.
/// Может управлять блоком.
/// Ничего не знает о классах верхнего уровня.
/// При клике вызывает событие клика.
/// </summary>
public class Cell : MonoBehaviour, IPointerClickHandler
{
    public static UnityEvent<Cell> CellClicked_Event;

    [SerializeField] private Block _blockPrefab;

    private float _blockSpeed = 2f;


    public Block Block;

    public bool IsTarget = false;
    public Cell TargetCell = null;
    public bool IsActive = true;

    public Cell Top;
    public Cell Bottom;
    public Cell Left;
    public Cell Right;



    public static void InitStatics()
    {
        CellClicked_Event = new UnityEvent<Cell>();
    }


    public void Init(int blockIndex = 0)
    {
        CreateBlock(transform.position, _blockSpeed, blockIndex);
        
    }


    public void SetLinks (Cell top, Cell bottom, Cell left, Cell right)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }


    public void KillBlock()
    {
        Block.Kill();
        Block = null;
    }


    public void CreateBlock(Vector3 blockPosition, float blockSpeed, int blockIndex = 0)
    {
        _blockSpeed = blockSpeed;
        Block = Instantiate(_blockPrefab, transform);
        Block.Init(blockIndex);
        Block.transform.position = blockPosition;

        if(blockPosition != transform.position)
        {
            StartCoroutine(MoveBlockToCell());
        } 
    }



    private IEnumerator MoveBlockToCell()
    {
        while(Block.transform.position != transform.position)
        {
            Block.transform.position = Vector3.MoveTowards(Block.transform.position, transform.position, Time.deltaTime * _blockSpeed);
            yield return null;
        }

        IsActive = true;
    }


    //Listeners
    public void OnPointerClick(PointerEventData eventData)
    {
        int TargetBlockIndex = PlayerDataManager.GetTargetBlockIndex();
        if (IsActive == false) return;
        if (Block.Index >= TargetBlockIndex - 1)
        {
            SoundManager.PlaySound(SoundName.CellClickError);
            return;
        }

        CellClicked_Event?.Invoke(this);
    }
}

