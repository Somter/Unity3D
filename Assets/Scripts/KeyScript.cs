using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private int keyNumber = 1;
    [SerializeField] private float timeout = 10.0f;  // in seconds
    [SerializeField] private string gatesDescription = "соответственные";

    private GameObject content;
    private Image timeoutImage;
    private float timeLeft;
    private bool isKeyInTime = true;
    private bool isTimerActive;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        timeoutImage = transform.Find("indicator/Canvas/Foreground").GetComponent<Image>();
        timeoutImage.fillAmount = 1.0f;
        timeLeft = timeout;
        isTimerActive = keyNumber == 1;
        GameEventSystem.Subscribe(OnGameEvent);
    }

    void Update()
    {
        if (isTimerActive && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeoutImage.fillAmount = Mathf.Clamp01(timeLeft / timeout);
            // (00 ff 00) - (ff ff 00) - (ff 00 00)
            timeoutImage.color = new Color(
                Mathf.Clamp01(2.0f * (1.0f - timeoutImage.fillAmount)),
                Mathf.Clamp01(2.0f * timeoutImage.fillAmount),
                0f,
                timeoutImage.color.a
            );
            if (timeLeft <= 0)
            {
                // GameState.isKey1InTime = false;
                // GameState.SetProperty($"isKey{keyNumber}InTime", false);
                isKeyInTime = false;
            }
        }
        content.transform.Rotate(0f, Time.deltaTime * 30f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameState.bag.Add($"Key{keyNumber}", 1);

            GameEventSystem.EmitEvent(new GameEvent
            {
                type = $"Key{keyNumber}Collected",
                payload = isKeyInTime,
                toast = $"You find a Key {keyNumber}. You may open {gatesDescription} gate",
                sound = isKeyInTime
                ? EffectSounds.keyCollectedInTime
                : EffectSounds.keyCollectedOutOfTime,


            });
            Destroy(this.gameObject);
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.type == $"Gate{keyNumber - 1}Opening")
        {
            isTimerActive = true;
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
    }

}
