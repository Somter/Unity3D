using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnSripts : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") 
        {
            GameObject.Destroy(other.gameObject);   
            SceneManager.LoadScene(0);
        }
    }
}
