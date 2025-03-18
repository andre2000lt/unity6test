
using System.Collections;
using UnityEngine;


public class TutorManager : MonoBehaviour
{
    [SerializeField] MatrixMaps _matrixMaps;
    [SerializeField] TutorMatrix _tutorMatrix;
    [SerializeField] GameObject  _pointer;
    [SerializeField] GameObject  _tutorDialog;
    [SerializeField] GameObject _endTutorDialog;

    private void Awake()
    {
        TutorMatrix.InitStatics();
        Cell.InitStatics();

        TutorMatrix.BlocksMerged_Event.AddListener(OnBlockMerge);
        TutorMatrix.PlayerMadeMove_Event.AddListener(OnPlayersClick);

        var matrixMap = _matrixMaps.GetMatrixMap(1);
        _tutorMatrix.Init(matrixMap);
    }





    //Listeners
    private void OnPlayersClick()
    {
        _pointer.SetActive(false);
    }


    private void OnBlockMerge(int arg0, Cell cell)
    {
        SoundManager.PlaySound(SoundName.BlocksMerge);
        _tutorMatrix.SelectActiveCell(1000);

        StartCoroutine(GoalCompletedRoutine());
    }


    private IEnumerator GoalCompletedRoutine()
    {
        yield return new WaitForSeconds(0.5f);
     
        SoundManager.PlaySound(SoundName.GoalCompleted);
        _tutorDialog.SetActive(false);
        _endTutorDialog.SetActive(true);
    }
}
