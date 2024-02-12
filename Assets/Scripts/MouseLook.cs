using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    //private float mouseSenssitivity = 5;
    public Transform playerbody;
    public Transform Camera;

    float xRotation;

    // Update is called once per frame
    void Update()
    {
        if((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if(Input.touchCount == 1)   
                    return;
                else if (Input.touchCount == 2)
                {
                    cameraRotation(1);
                    return;
                } 
            }
            cameraRotation(0);
        }
    }


    private void cameraRotation(int number_touch) {

        //float mouseX = Input.GetAxis("Mouse X") * mouseSenssitivity;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSenssitivity;

        float mouseX = Input.GetTouch(number_touch).deltaPosition.x * Statics.sens;
        float mouseY = Input.GetTouch(number_touch).deltaPosition.y * Statics.sens;

        playerbody.Rotate(UnityEngine.Vector3.up * mouseX * Time.deltaTime);

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        Camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
