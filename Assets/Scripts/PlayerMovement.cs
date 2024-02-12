using System.Runtime.Serialization;
using System.Net.NetworkInformation;
using System.Threading;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 15;
    private Vector3 move;

    public float gravity = -10f;
    public float jumpHeight = 1;
    private Vector3 velocity;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    public FixedJoystick joystick1;
    public FixedJoystick joystick2;
    public FixedJoystick joystick3;

    private AudioSource[] allAudioSources;
    public GameObject diedPanel;
    public GameObject audioD;

    public void StopAllAudio() {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach( AudioSource audioS in allAudioSources) {
            if (audioS.tag != "diedMusic" ) {
                audioS.Stop();
            }
        }
    } 

    void Start() {
        Statics.startLoad = true;
    }

    // Update is called once per frame
    void Update()
    {
        float x;
        float z;
        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");
        if (Statics.typeofJoystick == 1) {
            x = joystick1.Horizontal;
            z = joystick1.Vertical;
        }
        else if (Statics.typeofJoystick == 2) {
            x = joystick2.Horizontal;
            z = joystick2.Vertical;
        }
        else {
            x = joystick3.Horizontal;
            z = joystick3.Vertical;
        }

        move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundLayer);
        
        if (isGrounded && velocity.y < 0)
        {
           velocity.y = -1f;
        }        
        if (isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                if (Statics.level == 6) {
                    Statics.isJump = true;
                }
                Jump();
            }
        }
        else 
        {
           velocity.y += gravity * Time.deltaTime;
        }

        
        controller.Move(velocity * Time.deltaTime);

        if (controller.transform.position.y < -3) {
            StopAllAudio(); 
            Statics.level = 1;
            Statics.saveProgress();
            diedPanel.SetActive(true);
            audioD.SetActive(true);
        }

    }

    public void Jump() {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
        }
    }
}
