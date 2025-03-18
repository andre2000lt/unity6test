using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class MatrixBase : MonoBehaviour
{
    /// <summary>
    /// (duplicatesCount, duplicatesIndex)
    /// </summary>
    public static UnityEvent<int, Cell> BlocksMerged_Event;
    public static UnityEvent BlocksMergeStarted_Event;
    public static UnityEvent<Cell[,]> AllBlocksMerged_Event;
    public static UnityEvent PlayerMadeMove_Event;

    public static MatrixMode MatrixMode;

    [SerializeField] protected Cell _cellPrefab;


    protected Vector2 START_POS;
    protected const float CELL_STEP = 1f;
    protected Cell[,] _cells;
    protected int _cellsLength;

    protected List<Cell> _duplicates = new List<Cell>();



    public static void InitStatics()
    {
        MatrixMode = MatrixMode.PlayerMoves;

        BlocksMergeStarted_Event = new UnityEvent();
        BlocksMerged_Event = new UnityEvent<int, Cell>();
        AllBlocksMerged_Event = new UnityEvent<Cell[,]>();
        PlayerMadeMove_Event = new UnityEvent();
    }


    public void BaseInit()
    {
        float matrixSize = GetComponent<SpriteRenderer>().bounds.size.x;
        START_POS.x = -(matrixSize - 1) / 2;
        START_POS.y = (matrixSize - 1) / 2;

        _cellsLength = (int)matrixSize;
        _cells = new Cell[_cellsLength, _cellsLength];
    }


    protected void GenerateCells(int[,] matrixMap = null)
    {
        float xPos = START_POS.x;
        float yPos = START_POS.y;

        for (int y = 0; y < _cellsLength; y++)
        {
            for (int i = 0; i < _cellsLength; i++)
            {
                Cell cell = Instantiate(_cellPrefab, transform);
                cell.transform.localPosition = new Vector3(xPos, yPos, 0f);
                _cells[y, i] = cell;
                if (matrixMap == null)
                {
                    cell.Init(0);
                } 
                else
                {
                    cell.Init(matrixMap[y, i]);
                }
                
                xPos += CELL_STEP;
            }

            xPos = START_POS.x;
            yPos -= CELL_STEP;
        }
    }


    protected void GenerateCellLinks()
    {
        for (int y = 0; y < _cellsLength; y++)
        {

            for (int i = 0; i < _cellsLength; i++)
            {
                Cell Top = null;
                Cell Bottom = null;
                Cell Left = null;
                Cell Right = null;

                if (y - 1 >= 0)
                {
                    Top = _cells[y - 1, i];
                }

                if (y + 1 < _cellsLength)
                {
                    Bottom = _cells[y + 1, i];
                }

                if (i - 1 >= 0)
                {
                    Left = _cells[y, i - 1];
                }

                if (i + 1 < _cellsLength)
                {
                    Right = _cells[y, i + 1];
                }

                _cells[y, i].SetLinks(Top, Bottom, Left, Right);
            }
        }
    }


    protected void CheckNearbyCells(Cell cell)
    {
        if (cell.IsActive == false) return;

        if (cell.Right != null && !_duplicates.Contains(cell.Right))
        {
            if (cell.Right.Block?.Index == cell.Block.Index)
            {
                cell.Right.TargetCell = cell;
                _duplicates.Add(cell.Right);
                CheckNearbyCells(cell.Right);
            }
        }

        if (cell.Left != null && !_duplicates.Contains(cell.Left))
        {
            if (cell.Left.Block?.Index == cell.Block.Index)
            {
                cell.Left.TargetCell = cell;
                _duplicates.Add(cell.Left);
                CheckNearbyCells(cell.Left);
            }
        }

        if (cell.Top != null && !_duplicates.Contains(cell.Top))
        {
            if (cell.Top.Block?.Index == cell.Block.Index)
            {
                cell.Top.TargetCell = cell;
                _duplicates.Add(cell.Top);
                CheckNearbyCells(cell.Top);
            }
        }

        if (cell.Bottom != null && !_duplicates.Contains(cell.Bottom))
        {
            if (cell.Bottom.Block?.Index == cell.Block.Index)
            {
                cell.Bottom.TargetCell = cell;
                _duplicates.Add(cell.Bottom);
                CheckNearbyCells(cell.Bottom);
            }
        }
    }
}


public enum MatrixMode
{
    GenerateMatrix,
    PlayerMoves,
    Pause,
    GameOver
}
