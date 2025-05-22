using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;

    private static PlayerScript prevInstance;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (prevInstance != null) 
        {
            //GameObject.Destroy(this.gameObject);

            this.rb.velocity = prevInstance.rb.velocity;
            this.rb.angularVelocity= prevInstance.rb.angularVelocity;
            GameObject.Destroy(prevInstance.gameObject);
            prevInstance = this;
        }
       
        else 
        {
            prevInstance = this;    
        }

       
        //moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveValue = // moveAction.ReadValue<Vector2>(); 
            new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        //rb.AddForce(moveValue.x, 0, moveValue.y);
        // 

       // 
       Vector3 cameraForward = Camera.main.transform.forward;
       cameraForward.y = 0f;
        //
        if (cameraForward == Vector3.zero) 
        {
            cameraForward = Camera.main.transform.up;
        }
        else
        {
            cameraForward.Normalize();
        }
       cameraForward.Normalize(); 

       Vector3 cameraRight = Camera.main.transform.right;
       

       rb.AddForce(Time.timeScale *
           (moveValue.x * cameraRight + moveValue.y * cameraForward)
       );     
       
    }
}