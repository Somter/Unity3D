using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
   private static MusicScript instance;
   private AudioSource music;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        music = GetComponent<AudioSource>();
        GameState.AddListener(OnGameStateChanged);
        music.volume = GameState.musicVolume;
    }

    private void OnGameStateChanged(string fieldname)
    {
        if (fieldname == nameof(GameState.musicVolume))
            music.volume = GameState.musicVolume;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            GameState.RemoveListener(OnGameStateChanged);
            instance = null;
        }
    }

}
