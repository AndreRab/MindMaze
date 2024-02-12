using System.Runtime.Serialization;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickManager : MonoBehaviour
{
    public GameObject PlayerMovement1;
    public GameObject PlayerMovement2;
    public GameObject PlayerMovement3;
    public GameObject Tutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (Statics.startNew == true) {
            Tutorial.SetActive(true);
            Statics.startNew = false;
        }
        else
        {
            UpdateJoystick();
        }
    }


    public void closeTutorial() {
        UpdateJoystick();
        Tutorial.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateJoystick()
    {
        if(Statics.typeofJoystick == 1) {
            PlayerMovement1.SetActive(true);
            PlayerMovement2.SetActive(false);
            PlayerMovement3.SetActive(false);
        }
        else if(Statics.typeofJoystick == 2) {
            PlayerMovement2.SetActive(true);
            PlayerMovement3.SetActive(false);
            PlayerMovement3.SetActive(false);
        }
        else if(Statics.typeofJoystick == 3) {
            PlayerMovement3.SetActive(true);
            PlayerMovement2.SetActive(false);
            PlayerMovement1.SetActive(false);
        }
    }
}
