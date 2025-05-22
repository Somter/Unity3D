using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScripts : MonoBehaviour
{
    [SerializeField]
    private Image[] bagImage;

    private GameObject content;
    private Slider musicSlider;
    private Slider effectsSlider;
    private Toggle muteToggle
        ;
    private float defaultEffectsVolume;
    private float defaultMusicVolume;
    private bool defaultIsMuted;    

    void Start()
    {
        defaultEffectsVolume = GameState.effectsVolume;
        defaultMusicVolume = GameState.musicVolume;

        content = transform.Find("Content").gameObject;
        musicSlider = transform.Find("Content/Sounds/MusicSlider").GetComponent<Slider>();
        effectsSlider = transform.Find("Content/Sounds/EffectsSlider").GetComponent<Slider>();
        muteToggle = transform.Find("Content/Sounds/MuteToggle").GetComponent<Toggle>();

        defaultIsMuted = muteToggle.isOn; 

        //muteToggle.SetIsOnWithoutNotify(false);

        LoadPreference();

        OnMuteChanged(muteToggle.isOn);
        Hide();
    }

    private void LoadPreference() 
    {
        if (PlayerPrefs.HasKey(nameof(muteToggle))) 
        {
            muteToggle.isOn = PlayerPrefs.GetInt(nameof(muteToggle)) ==  1; 
        }
        else 
        {
            muteToggle.SetIsOnWithoutNotify(false); 
        }

        if (PlayerPrefs.HasKey(nameof(GameState.effectsVolume)))
        {
            effectsSlider.value = GameState.effectsVolume =
                PlayerPrefs.GetFloat(nameof(GameState.effectsVolume));
        }
        else
        {
            effectsSlider.value = GameState.effectsVolume;
        }


        if (PlayerPrefs.HasKey(nameof(GameState.musicVolume)))
        {
            musicSlider.value = GameState.musicVolume =
                PlayerPrefs.GetFloat(nameof(GameState.musicVolume));
        }
        else
        {
            musicSlider.value = GameState.musicVolume;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (content.activeInHierarchy) Hide(); else Show();
        }
    }

    private void Hide()
    {
        content.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void Show()
    {
        content.SetActive(true);
        Time.timeScale = 0.0f;
        for (int i = 0; i < bagImage.Length; i++) 
        {
            if (GameState.bag.ContainsKey($"Key{i + 1}")) 
            {
                bagImage[i].enabled = true;
            }
            else
            {
                bagImage[i].enabled = false;    
            }
        }
    }

    public void OnExitClick() 
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#endif

#if UNITY_STANDALONE

        Application.Quit();

#endif 
    }

    public void OnDefaultClick() 
    {
        muteToggle.isOn = defaultIsMuted;
        effectsSlider.value = defaultEffectsVolume;
        GameState.effectsVolume = (muteToggle.isOn ? 0.0f : defaultEffectsVolume);
        musicSlider.value = GameState.musicVolume = defaultEffectsVolume;
        GameState.musicVolume = muteToggle.isOn ? 0.0f : defaultMusicVolume;
    }

    public void OnContinue() 
    {
        Hide();
    }

    public void OnEffectsVoumneChanged(float volumne) 
    {
        GameState.effectsVolume = volumne;
    }

    public void OnMusicVolumeChanged(float volume)
    {
        if (!muteToggle.isOn) GameState.musicVolume = volume;
    }

    public void OnMuteChanged(bool isMuted) 
    {
        if (isMuted) 
        {
            GameState.musicVolume = 0f;
        }
        else
        {
            GameState.musicVolume = musicSlider.value;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(nameof(GameState.effectsVolume), effectsSlider.value);
        PlayerPrefs.SetFloat(nameof(GameState.musicVolume), musicSlider.value);
        PlayerPrefs.SetInt(nameof(muteToggle), muteToggle.isOn ? 1 : 0);
        PlayerPrefs.Save(); 
    }
}
