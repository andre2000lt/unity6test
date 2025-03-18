using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorMatrix : GameMatrix
{
    public override void Init(int[,] matrixMap = null)
    {

        base.BaseInit();
        Cell.CellClicked_Event.AddListener(UpdateMatrixOnCellClick);

        GenerateCells(matrixMap);
        GenerateCellLinks();

        SelectActiveCell(5);
    }


    public void SelectActiveCell(int cellIndex)
    {
        int index = 0;
        for (int y = 0; y < _cellsLength; y++)
        {
            for (int i = 0; i < _cellsLength; i++)
            {
                var cell = _cells[y, i];

                if (index != cellIndex) 
                { 
                    cell.enabled = false;
                }


                index++;
            }
        }
    }


    protected override void UpdateMatrixOnCellClick(Cell clickedCell)
    {
        if (IsBisy) return;

        SoundManager.PlaySound(SoundName.CellClick);

        PlayerMadeMove_Event?.Invoke();

        clickedCell.Block.IncreaseIndex();

        StartCoroutine(MergeDuplicates(clickedCell));
    }
}
