using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{
    [SerializeField] private Material dayskybox;
    [SerializeField] private Material nightSkybox;
    void Start()
    {
        RenderSettings.skybox = GameState.isDay ? dayskybox : nightSkybox;
        GameState.AddListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(string fieldname)
    {
        if (fieldname == nameof(GameState.isDay))
        {
            RenderSettings.skybox = GameState.isDay ? dayskybox : nightSkybox;

        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
}
