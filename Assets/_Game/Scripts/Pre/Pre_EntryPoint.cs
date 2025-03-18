using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;


public class Pre_EntryPoint : MonoBehaviour
{
    private bool _isLocalizationInited = false;
    private bool _isYandexGamesInited = false;

    private void Start()
    {
        StartCoroutine(CheckLoaclizationReadyRoutine());

        StartCoroutine(CheckAllServicesReadyRoutine());
    }


    private IEnumerator CheckAllServicesReadyRoutine()
    {
        bool isAllServisesReady = false;

        while (!isAllServisesReady)
        {
            isAllServisesReady = true;

            if (!_isLocalizationInited)
            {
                isAllServisesReady = false;
            }

            yield return null;
        }

        SetLang();



        PlayerDataManager.Init();

/*        Saver.SavesData.IsFirstMoveMade = false;
        Saver.SaveProgress(StorageType.TextFile);*/

        bool isTutorialViewed = Saver.SavesData.IsTutorialViewed;
        if(isTutorialViewed)
        {
            GameScenesController.LoadSceneAsync(GameScene.Main);
        } 
        else
        {
            GameScenesController.LoadSceneAsync(GameScene.Tutorial);
        }
        
    }


    private IEnumerator CheckLoaclizationReadyRoutine()
    {
        var loaclizationInitAsync = LocalizationSettings.InitializationOperation;
        while (!loaclizationInitAsync.IsDone)
        {
            yield return null;
        }

        _isLocalizationInited = true;
    }


    private void SetLang()
    {
        LocaleIdentifier ru_id = new LocaleIdentifier(SystemLanguage.Russian);
        LocaleIdentifier en_id = new LocaleIdentifier(SystemLanguage.English);

        LocaleIdentifier cur_id = ru_id;

        var lang = Application.systemLanguage;
        Debug.Log(lang);

        switch (lang)
        {
            case SystemLanguage.Russian:
                cur_id = ru_id;
                break;
            case SystemLanguage.English:
                cur_id = en_id;
                break;
            default:
                cur_id = en_id;
                break;
        }
        var locale = LocalizationSettings.AvailableLocales.GetLocale(cur_id);
        LocalizationSettings.SelectedLocale = locale;
    }
}
