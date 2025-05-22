using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixedSript : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            CameraScript.isFixed = true;
            CameraScript.fixedTransform = this.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraScript.isFixed = false;
        }
    }
}
