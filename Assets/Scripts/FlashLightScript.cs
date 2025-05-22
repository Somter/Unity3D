using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;
    private float charge;
    private float chargeLifeTime = 10.0f;

    private bool isActive => !GameState.isDay && GameState.isFpv;
    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("FlashlightScript: Player not found!");
            return;
        }
        _light = this.GetComponent<Light>();
        charge = 1.0f;
    }

    void Update()
    {
        if (player == null) return;
        this.transform.position = player.transform.position;
        this.transform.forward = Camera.main.transform.forward;
        if (isActive)
        {
            _light.intensity = Mathf.Clamp01(charge);
            charge = charge < 0 ? 0.0f : charge - Time.deltaTime / chargeLifeTime;       
        }
        else
        {
            _light.intensity = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.gameObject.CompareTag("Battery")) 
        {
           
            charge += 1.0f;
            GameEventSystem.EmitEvent(new GameEvent
            {
                type = "Battery",
                toast = $"Ви знайшли батарейку. Заряд ліхтарика поповнено до {charge:F1}",
                sound = EffectSounds.batteryCollected,
            });
            GameObject.Destroy(other.gameObject);
        }
    }
}
