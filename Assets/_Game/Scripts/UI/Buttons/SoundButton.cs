
using UnityEngine;
using UnityEngine.UI;


public class SoundButton : MonoBehaviour
{
    [SerializeField] private Image _soundOnImage;
    [SerializeField] private Image _soundOffImage;

    private Button _soundButton;
    private bool _isSoundOn;


    private void Awake()
    {
        _soundButton = GetComponent<Button>();
        _soundButton.onClick.AddListener(ToggleSoundAndSave);

        _isSoundOn = PlayerDataManager.IsSoundOn();
        if(_isSoundOn)
        {
            EnableSound();
        } 
        else
        {
            DisableSound();
        }
    }


    private void EnableSound()
    {
        _soundOnImage.enabled = true;
        _soundOffImage.enabled= false;
        _isSoundOn = true;
    }
    
    
    private void DisableSound()
    {
        _soundOnImage.enabled = false;
        _soundOffImage.enabled= true;
        _isSoundOn = false;
    }


    private void ToggleSoundAndSave()
    {
        if(_isSoundOn)
        {
            SoundManager.PlaySound(SoundName.NeutralClick);
            DisableSound();

            PlayerDataManager.SetSoundOff();
        } 
        else
        {
            EnableSound();
            SoundManager.PlaySound(SoundName.NeutralClick);

            PlayerDataManager.SetSoundOn();
        }
    }
}
