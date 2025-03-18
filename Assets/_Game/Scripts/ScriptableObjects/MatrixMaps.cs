using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/MatrixMaps")]
public class MatrixMaps : ScriptableObject
{
    public int[,] _firstTutorMatrixMap = new int[3,3]
    {
        { 1, 2, 4 },
        { 4, 4, 3 },
        { 5, 1, 1 },
   };

    public int[,] GetMatrixMap(int id)
    {
        return id switch
        {
            1 => _firstTutorMatrixMap,
            2 => _firstTutorMatrixMap,
            3 => _firstTutorMatrixMap,
            _ => _firstTutorMatrixMap
        };
    }
}
