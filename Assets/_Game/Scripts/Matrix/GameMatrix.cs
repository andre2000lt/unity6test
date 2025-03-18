using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMatrix : MatrixBase
{
    [SerializeField] private float _blockSpeed = 4f;    

    public static bool IsBisy = false;
    public static int RollBackCounter = -1;


    private void Update()
    {

/*        Debug.Log("MatricMode: " + MatrixMode);
        Debug.Log("IsBisy: " + IsBisy);*/
    }
    public virtual void Init(int[,] matrixMap = null)
    {
        base.BaseInit();

        if (matrixMap == null)
        {
            MatrixMode = MatrixMode.GenerateMatrix;
        }

        Cell.CellClicked_Event.AddListener(UpdateMatrixOnCellClick);

        GenerateCells(matrixMap);

        GenerateCellLinks();

        if (MatrixMode == MatrixMode.GenerateMatrix)
        {
            StartCoroutine(UpdateMatrixRoutine());
        }
    }


    protected virtual void UpdateMatrixOnCellClick(Cell clickedCell)
    {
        if (MatrixMode != MatrixMode.PlayerMoves || IsBisy) return;

        SoundManager.PlaySound(SoundName.CellClick);

        PlayerMadeMove_Event?.Invoke();

        clickedCell.Block.IncreaseIndex();

        StartCoroutine(UpdateMatrixRoutine(clickedCell));
    }


    protected IEnumerator UpdateMatrixRoutine(Cell clickedCell = null)
    {
        IsBisy = true;
        BlocksMergeStarted_Event?.Invoke();
        if (clickedCell != null)
        {
            yield return StartCoroutine(MergeDuplicates(clickedCell));
        }
        
        bool isDuplicatesFinished = false;

        while (isDuplicatesFinished == false)
        {
            while (isDuplicatesFinished == false)
            {
                yield return StartCoroutine(MergeDuplicates());
                yield return StartCoroutine(MoveBlocksDown());
                isDuplicatesFinished = CheckDuplicatesFinished();
            }
            
            yield return StartCoroutine(GenerateNewBlocks());
            isDuplicatesFinished = CheckDuplicatesFinished(); 
        }

        AllBlocksMerged_Event?.Invoke(_cells);
        Debug.Log("SSOK");
        if (MatrixMode == MatrixMode.GenerateMatrix)
        {
            Debug.Log("OK");
            MatrixMode = MatrixMode.PlayerMoves;
        }

        Debug.Log("GOK");
        IsBisy = false;
    }



    // MergeDuplicates
    protected IEnumerator  MergeDuplicates(Cell clickedCell = null)
    {
        Cell cell;

        if(clickedCell == null)
        {
            for (int y = 0; y < _cellsLength; y++)
            {
                for (int i = 0; i < _cellsLength; i++)
                {
                    cell = _cells[y, i];
                    if (cell.IsActive == false) continue;

                    yield return StartCoroutine(CheckOneCell(cell));
                }
            }
        }
        else
        {
            yield return StartCoroutine(CheckOneCell(clickedCell));
        }

    }


    protected IEnumerator CheckOneCell(Cell cell)
    {
        bool isCellChecked = false;

        while (!isCellChecked)
        {
            _duplicates.Add(cell);
            SetFinalTarget(cell);

            CheckNearbyCells(cell);

            if (_duplicates.Count > 2)
            {
                foreach (var currentCell in _duplicates)
                {
                    if (currentCell.IsTarget == false)
                    {
                        currentCell.IsActive = false;
                    }
                }

                yield return StartCoroutine(KillBlocksAfterMerge());
                ClearDuplicates();
            }
            else
            {
                isCellChecked = true;
                ClearDuplicates();
            }
        }
    }





    protected void SetFinalTarget(Cell targetCell)
    {
        if (!_duplicates.Contains(targetCell)) return;

        foreach (var currentCell in _duplicates)
        {
            currentCell.IsTarget = false;
        }

        targetCell.IsTarget = true;  
    }


    protected IEnumerator KillBlocksAfterMerge()
    {
        List<Cell> emptyCells = new List<Cell>();
        Cell targetCell = null;

        for (var i = 0; i < _duplicates.Count; i++ )
        {
            if (_duplicates[i].IsActive == false)
            {
                _duplicates[i].Block.SetOrder(i);
                emptyCells.Add(_duplicates[i]);
                StartCoroutine(GoToTargetProcess(_duplicates[i]));
            } 
            else
            {
                targetCell = _duplicates[i];
            }
        }

        bool isAllBlocksArrived = false;
        while (!isAllBlocksArrived)
        {
            foreach (var currentCell in emptyCells)
            {
                if (currentCell.Block.transform.position != targetCell.Block.transform.position)
                {
                    isAllBlocksArrived = false;
                    break;
                }

                isAllBlocksArrived = true;
            }

            yield return null;
        }

        BlocksMerged_Event?.Invoke(_duplicates.Count, _duplicates[0]);

        KillDuplicates();

        while (GameMatrix.MatrixMode == MatrixMode.Pause || Block.IsAnimating)
        {
            yield return null;
        } 
    }


    protected IEnumerator GoToTargetProcess(Cell cell)
    {
        Cell blockFinalTargetCell = _duplicates.Find((cell) => cell.IsTarget);
        Block block = cell.Block;
        Cell blockTargetCell = cell.TargetCell;

        block.transform.parent = blockFinalTargetCell.transform;

        
        while (blockFinalTargetCell.transform.position != block.transform.position)
        { 
            if(block.transform.position == blockTargetCell.transform.position)
            {
                blockTargetCell = blockTargetCell.TargetCell;
            }

            block.transform.position = Vector3.MoveTowards(block.transform.position, blockTargetCell.transform.position, _blockSpeed * Time.deltaTime);
 
            yield return null;
        }
    }


    protected void KillDuplicates()
    {
        foreach (var currentCell in _duplicates)
        {
            if (currentCell.IsActive == false)
            {
                currentCell.KillBlock();
            } 
            else
            {
                currentCell.Block.IncreaseIndexRotateAnimation();
            }
        }
    }


    protected void ClearDuplicates()
    {
        foreach (var cell in _duplicates)
        {
            cell.IsTarget = false;
            cell.TargetCell = null;
        }

        _duplicates.Clear();
    }




    // MoveBlocksDown
    protected IEnumerator MoveBlocksDown()
    {
        Cell cell;

        for (int y = _cellsLength - 1; y >= 0; y--)
        {
            for (int i = 0; i < _cellsLength; i++)
            {
                cell = _cells[y, i];
                if (cell.IsActive == true) continue;

                CheckEmptyCell(cell);
            }
        }

        for (int y = _cellsLength - 1; y >= 0; y--)
        {
            for (int i = 0; i < _cellsLength; i++)
            {
                cell = _cells[y, i];
                if (cell.TargetCell == null) continue;

                StartCoroutine(MoveToBottomCellProcess(cell));
            }
        }

        bool isAllBlocksArrived = false;

        while (isAllBlocksArrived == false)
        {
            isAllBlocksArrived = true;

            for (int y = _cellsLength - 1; y >= 0; y--)
            {
                for (int i = 0; i < _cellsLength; i++)
                {
                    if(_cells[y, i].TargetCell != null)
                    {
                        isAllBlocksArrived = false;
                    }
                }
            }

            yield return null;
        }
    }


    /// <summary>
    /// ѕровер€ет пустую €чейку и все пустые €чейки над ней и напрвл€ет в них блоки с верхних €чеек
    /// </summary>
    protected void CheckEmptyCell(Cell cell) 
    { 
        Cell upperCell = cell.Top;
        
        while(true)
        {
            if(upperCell == null) break;

            if (upperCell.IsActive == false) 
            {
                upperCell = upperCell.Top;
                continue;
            }

            if(upperCell.IsActive == true)
            {
                upperCell.TargetCell = cell;
                upperCell.IsActive = false;
                break;
            }  
        }
    }


    protected IEnumerator MoveToBottomCellProcess(Cell cell)
    {
        Block block = cell.Block;
        Cell targetCell = cell.TargetCell;
        cell.Block = null;
        block.transform.parent = targetCell.transform;
        targetCell.Block = block;
        targetCell.IsActive = true;


        while(block.transform.position != targetCell.transform.position)
        {
            block.transform.position = Vector3.MoveTowards(block.transform.position, targetCell.transform.position, _blockSpeed * Time.deltaTime);
            yield return null;
        }

        cell.TargetCell = null;
    }



    // Generate new Blocks
    protected IEnumerator GenerateNewBlocks()
    {
        Cell cell;

        for (int y = _cellsLength - 1; y >= 0; y--)
        {
            for (int i = 0; i < _cellsLength; i++)
            {
                cell = _cells[y, i];

                if(cell.IsActive == false)
                {
                    cell.CreateBlock(new Vector3(cell.transform.position.x, cell.transform.position.y + y + cell.transform.position.y, 0), _blockSpeed);
                } 
            }
        }

        bool IsAllCellsActive = false;
        while (IsAllCellsActive == false)
        {
            IsAllCellsActive = true;

            for (int y = _cellsLength - 1; y >= 0; y--)
            {
                for (int i = 0; i < _cellsLength; i++)
                {
                    if (_cells[y, i].IsActive == false) 
                    {
                        IsAllCellsActive = false;
                    }
                }
            }

            yield return null;
        }
        
    }


    protected bool CheckDuplicatesFinished()
    {
        for (int y = 0; y < _cellsLength; y++)
        {
            for (int i = 0; i < _cellsLength; i++)
            {
                CheckNearbyCells(_cells[y, i]);

                if (_duplicates.Count > 2)
                {
                    ClearDuplicates();
                    return false;
                }

                ClearDuplicates();
            }
        }

        return true;
    }
}
