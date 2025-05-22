using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;


public class EffectsScript : MonoBehaviour
{
    private AudioSource KeyCollectSound;
    private AudioSource KeyCollectOfTimeSound;
    private AudioSource batteryCollectSound;
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        KeyCollectSound = audioSources[0];
        batteryCollectSound = audioSources[1];
        KeyCollectOfTimeSound = audioSources[2];
        GameEventSystem.Subscribe(OnGameEvent);
    }


    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.sound != null)
        {
            switch (gameEvent.sound)
            {
                case EffectSounds.batteryCollected:
                    batteryCollectSound.Play(); break;
                case EffectSounds.keyCollectedInTime:
                    KeyCollectSound.Play(); break;
                case EffectSounds.keyCollectedOutOfTime:
                    KeyCollectOfTimeSound.Play(); break;
                default:
                    Debug.LogError("Undefined sound: " + gameEvent.sound); break;
            }
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
    }
}
