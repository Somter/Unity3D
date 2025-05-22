using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") 
        {
            GameObject.DontDestroyOnLoad(other.gameObject);



            SceneManager.LoadScene(1);  
        }
    }
}
