using System.Linq;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;
    //public static bool isDay;

    void Start()
    {
        dayLights = GameObject
            .FindGameObjectsWithTag("Day")
            .Select(x => x.GetComponent<Light>())
            .ToArray();
        nightLights = GameObject
            .FindGameObjectsWithTag("Night")
            .Select(x => x.GetComponent<Light>())
            .ToArray();
        GameState.isDay = true;
        SetLights();
        GameState.AddListener(OnGameStateChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameState.isDay = !GameState.isDay;
        }
    }

    private void SetLights()
    {
        if (GameState.isDay)
        {
            foreach (Light light in dayLights)
            {
                light.intensity = 1.0f;
            }
            foreach (Light light in nightLights)
            {
                light.intensity = 0.0f;
            }
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
        }
        else
        {
            foreach (Light light in dayLights)
            {
                light.intensity = 0.0f;
            }
            if (!GameState.isFpv)
            {
                foreach (Light light in nightLights)
                {
                    light.intensity = 1.0f;
                }
            }
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
        }
    }

    private void OnGameStateChanged(string fieldname)
    {
        if (fieldname == nameof(GameState.isDay))
        {
            SetLights();
        }
        else if (fieldname == nameof(GameState.isFpv))
        {
            if (!GameState.isDay)
            {
                foreach (Light light in nightLights)
                {
                    light.intensity = GameState.isFpv ? 0.0f : 1.0f;
                }
            }
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
}