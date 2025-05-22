using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class TosterScript : MonoBehaviour
{
    private static TosterScript instance;
    public GameObject content;
    private TMPro.TextMeshProUGUI text;
    private CanvasGroup canvasGroup;
    private float showTime = 3.0f; // time of show
    private float timeout;
    private Queue<ToastMassehe> meaageQueue = new Queue<ToastMassehe>();
    private float deltaTime = 0f;

    void Start()
    {
        instance = this;
        Transform t = this.transform.Find("Content");
        content = t.gameObject;
        canvasGroup = content.GetComponent<CanvasGroup>();
        text = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        content.SetActive(false);
        GameState.AddListener(OnGameStateChanged);
        GameEventSystem.Subscribe(OnGameEvent);
        Debug.Log($"FPS: {Application.targetFrameRate}, vSync: {QualitySettings.vSyncCount}, SFR: {Screen.currentResolution.refreshRate}");

    }

    void Update()
    {
        if(deltaTime == 0f && Time.deltaTime != 0f) 
        {
            deltaTime = Time.deltaTime;
        }
        if (timeout > 0) 
        {
            canvasGroup.alpha = Mathf.Clamp01(timeout * 2.0f);
            float dt = Time.timeScale > 0.0f ? Time.deltaTime
                : this.deltaTime > 0f ? Time.deltaTime
                : QualitySettings.vSyncCount < 0 ? QualitySettings.vSyncCount / (float)Screen.currentResolution.refreshRate 
                : Application.targetFrameRate > 0 ? 1.0f / Application.targetFrameRate
                : 0.016f;

            //Debug.Log(dt);  
            timeout -= dt;
            if (timeout <= 0f) 
            {
                content.SetActive(false);   
            }
        }
        else if (meaageQueue.Count > 0)
        {
            var message = meaageQueue.Dequeue();
            content.SetActive(true);
            text.text = message.text;
            timeout = message.time; 
            
        }
    }

    

    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
        {
            Toast("You find a Key #1. You may open blue gate");
            Toast(GameState.isDay
                    ? "День"
                    : "Ночь");
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.toast != null)
        {
            Toast(gameEvent.toast);
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
        GameEventSystem.Unsubscribe(OnGameEvent);
    }

    public static void Toast(string message, float time = 0.0f) 
    {
        //instance.content.SetActive(true);  
        //instance.text.text = message;
        //instance.timeout = time == 0.0f ? instance.showTime : time;   
        instance.meaageQueue.Enqueue(new ToastMassehe
        {
            text = message,
            time = time == 0.0f ? instance.showTime : time
        });
    }

    public class ToastMassehe 
    {
        public string text { get; set; }
        public float time { get; set; }
    }
}
