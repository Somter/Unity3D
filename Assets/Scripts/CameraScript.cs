using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraScript : MonoBehaviour
{
    //[SerializeField]
    private Transform cameraAnchor;
    private Vector3 offset;

    private InputAction lookActiion;
    private float rotAngleY, rotAngleY0;
    private float rolSensitivityY = 10f;
    private float rotAngleX, rotAngleX0;
    private float rolSensitivityX = 5f;
    private float maxOffset = 10.0f;
    private float fpvRange = 1.5f;
    private float fpvOffset = 0.01f;
    //private float minAngleX = 40f;
    //private float maxAngleX = 90f;
    //private float minAngleFpvX = -10f;
    //private float maxAngleFpvX = 40f;

    public static bool isFixed = false;
    //public static bool isFpv = false;
    public static Transform fixedTransform = null;


    void Start()
    {
        cameraAnchor = GameObject.Find("Player").transform;   
        offset = cameraAnchor.position - transform.position;
        //lookActiion = InputSystem.actions.LookAt;
        rotAngleY = rotAngleY0 = transform.eulerAngles.y;
        rotAngleX = rotAngleX0 = transform.eulerAngles.x;
        GameState.isFpv = offset.magnitude < fpvRange;

    }

    void Update()
    {
        if (isFixed) 
        {
            this.transform.position = fixedTransform.position;
            this.transform.rotation = fixedTransform.rotation;  
        }
        else 
        {    
        // Приближение / отдаление
        Vector2 zoom = Input.mouseScrollDelta;
        if(zoom.y > 0 && offset.magnitude > fpvRange)    
        {
            offset *= 0.9f;
            if (offset.magnitude < fpvRange) 
            {
                offset *=  fpvOffset / offset.magnitude;
                    GameState.isFpv = true;
            }
        }
        else if (zoom.y < 0 && offset.magnitude < maxOffset)   
        {
            if (offset.magnitude < fpvRange) 
            {
                offset *= fpvRange / offset.magnitude;
                    GameState.isFpv = false;
            }
            offset *= 1.1f;
        }

        //if(zoom != Vector2.zero) 
        //{
        //    Debug.Log(zoom);
        //}

        // 
        Vector2 LookValue = // lookActiion.ReadValue<Vector2>();
        new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * Time.timeScale;
        rotAngleY += rolSensitivityY * LookValue.x;
        rotAngleX -= rolSensitivityX * LookValue.y; 

        transform.eulerAngles = new Vector3(rotAngleX, rotAngleY, 0f);
        //
        //

        //
        transform.position = cameraAnchor.position - // offset : без корекції на поворот
             Quaternion.Euler(rotAngleX - rotAngleX0, rotAngleY - rotAngleY0, 0f) * offset;
        }
    }
}