using System.Net.NetworkInformation;
using System.ComponentModel.Design.Serialization;
using System;
using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Statics : MonoBehaviour
{
   public static bool startLoad = true;
   public static bool isFirstTime = true;
   public static bool startNew = false;
   public static int level = 1;
   public static int prevLevel = 1;
   public static float sens = 5;
   public static int typeofJoystick = 2;
   public static bool isJump = false;
   public static bool wasEndOpen = false;
   public static bool died = false;
   public static bool diedForReturn = false;

   public GameObject pause;
   public GameObject playerMovement1;
   public GameObject playerMovement2;
   public GameObject playerMovement3;
   public GameObject settingsPanel;
   public GameObject navigationPanel;
   public GameObject videoPanel;
   public GameObject endCanvas;
   public GameObject diedCanvas;

   public Slider volumeSlider;
   public Slider sensSlider;

   public Toggle smallToggle;
   public Toggle mediumToggle;
   public Toggle bigToggle;

   public RewaedAds rAd;

   void Start(){
      if (isFirstTime) {
         level = PlayerPrefs.GetInt("Level", 1);
         AudioListener.volume = PlayerPrefs.GetFloat("Music", 1);
         sens = PlayerPrefs.GetFloat("Sens", 5);
         typeofJoystick = PlayerPrefs.GetInt("Joystick", 2);  
           
         isFirstTime = false;
      }

      if (level > 1){
         prevLevel = level;
      } 

      if (rAd) {
         rAd.LoadAd();
      }

      volumeSlider.value = AudioListener.volume;
      sensSlider.value = sens /10;

      if (typeofJoystick == 1) {
         smallToggle.isOn = true;
      }
      else if (typeofJoystick == 2) {
         mediumToggle.isOn = true;
      }
      else {
         bigToggle.isOn = true;
      }
   }

   public void startNewGame() {
      navigationPanel.SetActive(false);
      PlayerPrefs.SetInt("Level", 1);
      startNew = true;
      Statics.level = 1;
   }

   public void closeDiedCanvs() {
      diedCanvas.SetActive(false)
;   }

   public void pauseGame() {
      Time.timeScale = 0;
      pause.SetActive(true);
      playerMovement1.SetActive(false);
      playerMovement2.SetActive(false);
      playerMovement3.SetActive(false);
   }

   public void countineGame() {
      Time.timeScale = 1;
      pause.SetActive(false);
      if (Statics.typeofJoystick == 1) {
         playerMovement1.SetActive(true);
      }
      else if (Statics.typeofJoystick == 2) {
         playerMovement2.SetActive(true);
      }
      else{
         playerMovement3.SetActive(true);
      }
   }

   public void isJumped() {
      if (Statics.level == 6) {
         isJump = true;
      }
   }

   public void isNotJumped() {
      if (Statics.level == 6) {
         isJump = false;
      }
   } 

   public void goToMainMenu() {
      Time.timeScale = 1;
      SceneManager.LoadScene("Start");
   }

   public void exitGame() {
      Application.Quit();
   }

   public void closeEndPanel() {
      Time.timeScale = 1;
      endCanvas.SetActive(false);
   }

   public static void saveProgress() {
      PlayerPrefs.SetInt("Level", Statics.level);
   }

   public void openSettings(){
      settingsPanel.SetActive(true);
      navigationPanel.SetActive(false);
   }

   public void closeSettings(){
      settingsPanel.SetActive(false);
      navigationPanel.SetActive(true);
   }

   public void changeVolume(){
      AudioListener.volume = volumeSlider.value;
      PlayerPrefs.SetFloat("Music", volumeSlider.value);
   }

   public void changeSens(){
      sens = sensSlider.value * 10;
      PlayerPrefs.SetFloat("Sens", sens);
   }

   public void onSmallToggle(){
      if (smallToggle.isOn == true) {
         mediumToggle.isOn = false;
         bigToggle.isOn = false;
         Statics.typeofJoystick = 1;
         PlayerPrefs.SetInt("Joystick", 1);
      }
   }

   public void onMediumToggle(){
      if (mediumToggle.isOn == true){
         smallToggle.isOn = false;
         bigToggle.isOn = false;  
         Statics.typeofJoystick = 2;
         PlayerPrefs.SetInt("Joystick", 2);
      }
   }

   public void onBigToggle(){
      if (bigToggle.isOn == true) {
         smallToggle.isOn = false;
         mediumToggle.isOn = false;
         Statics.typeofJoystick = 3;
         PlayerPrefs.SetInt("Joystick", 3);
      }
   }

   public void dieForAdd() {
      died = true;
   }

   public void diedForReturnAdd(){
      diedForReturn = true;
   }
}



