using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//
//  Copyright © 2022 Kyo Matias & Nate Florendo. All rights reserved.
//  


public class MainMenu : MonoBehaviour
{
    private int _selectedCharacter;
    [SerializeField] private GameObject _loadButton;
    [SerializeField] private GameObject _achievementContent;
    [SerializeField] private GameObject _achievementPanel;
    [SerializeField] private TextMeshProUGUI BGMtext;
    [SerializeField] private TextMeshProUGUI SFXtext;
    
    [SerializeField] private Color _locked;
    [SerializeField] private Color _unlocked;

    private bool _isMutedBGM = false;
    private bool _isMutedSFX = false;

    private static bool _isLoaded;

    private void Start()
    {
        AudioManager.Instance.PlayLoop(AudioManager.Sounds.MainMenuBGM);
        Debug.Log($"Isnewgame: {SaveManager.Instance.IsNewGame}");
        CheckNewGame();
        ObjectPool.Instance._objectsPoolUI.Clear();
        ObjectPool.Instance.Dispose(ObjectPool.Instance._objectsPoolUI);
        _isLoaded = false;
    }
    
    public void Init(int value)  //initializes value of selected character before selecting
    {
        _selectedCharacter = value;
        SaveManager.Instance.UpdateSelected(_selectedCharacter);
    }
    
    public void NextOption()
    {
        _selectedCharacter += 1;
        if (_selectedCharacter == SaveManager.Instance.GetChartacterCount())
        {
            _selectedCharacter = 0;
        }

        SaveManager.Instance.UpdateSelected(_selectedCharacter);
    }
    
    public void BackOption()
    {
        _selectedCharacter -= 1;
        if (_selectedCharacter < 0)
        {
            _selectedCharacter = SaveManager.Instance.GetChartacterCount()-1;
        }

        SaveManager.Instance.UpdateSelected(_selectedCharacter);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void CheckNewGame()
    {
        if (SaveManager.Instance.IsNewGame)
        {
            _loadButton.GetComponent<Button>().interactable = false;
        }
    }

    public void MuteUnmuteBGM()
    {
        if (!_isMutedBGM)
        {
            BGMtext.text = "BGM: Off";
            AudioManager.Instance.SoundBGMusic.mute = true;
            _isMutedBGM = true;
        }
        else
        {
            BGMtext.text = "BGM: On";
            AudioManager.Instance.SoundBGMusic.mute = false;
            _isMutedBGM = false;
        }
    }
    
    public void MuteUnmuteSFX()
    {
        if (!_isMutedSFX)
        {
            SFXtext.text = "SFX: Off";
            AudioManager.Instance.SoundSfx.mute = true;
            _isMutedSFX = true;
        }
        else
        {
            SFXtext.text = "SFX: On";
            AudioManager.Instance.SoundSfx.mute = false;
            _isMutedSFX = false;
        }
    }
    
    public void ClickSound() //Referenced on EventTrigger component of UI buttons
    {
        AudioManager.Instance.PlayOnce(AudioManager.Sounds.ButtonClick);
    }
    
    public void LoadAchievementsPanel() // DISPLAYS ACHIEVEMENTS ON MAIN MENU USING THE LIST FROM ACHIEVEMENT MANAGER
    {
        if (!_isLoaded)
        {
            foreach (var t in SaveManager.Achievements)
            {
                //GameObject obj = Instantiate(_achievementContent, _achievementPanel.transform);
                GameObject obj =
                    ObjectPool.Instance.GetObjectUI(_achievementContent, _achievementPanel.transform);
                obj.transform.SetParent(_achievementPanel.transform);
                obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = t.title;
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = t.description;
                if (!t.achieved)
                {
                    obj.GetComponent<Image>().color = _locked;
                    obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "✘";
                }
                else
                {
                    obj.GetComponent<Image>().color = _unlocked;
                    obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "✔";
                }
            }
            Debug.Log($"Achievement count: {SaveManager.Achievements}");
            Debug.Log($"ObjectPoolUI count: {ObjectPool.Instance._objectsPoolUI.Count}");
            _isLoaded = true;
        }
    }
}
