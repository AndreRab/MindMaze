﻿using System.Threading;
using UnityEngine.UI;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour {

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject ceilingPrefab;

	public GameObject characterController;

	public GameObject floorParent;
	public GameObject wallsParent;
	public GameObject endCanvas;

	public InterstitialAds ad;

	// allows us to see the maze generation from the scene view
	public bool generateRoof = true;  

	// number of times we want to "dig" in our maze
	private int tilesToRemove = 300 + 50 * Statics.level;

	//(10 + 20 * Statics.level);

	private int mazeSize = 20 + 10 * Statics.level;

	// spawns at the end of the maze generation
	public GameObject pickup;

    public GameObject slowEnemy;
	public GameObject fastEnemy;

	public Text txt1;
	public Text txt3;
	public Text txt2;

	// this will determine whether we've placed the character controller
	private bool characterPlaced = false;

	// 2D array representing the map
	private bool[,] mapData;

	// we use these to dig through our maze and to spawn the pickup at the end
	private int mazeX = 3, mazeY = 1;

	private float timer = 0;

	// Use this for initialization
	void Start () {

		if (Statics.diedForReturn) {	
			Statics.level = Statics.prevLevel;
			Statics.diedForReturn = false;
		}
		else {
			Statics.level = PlayerPrefs.GetInt("Level", 1);
		}

		Time.timeScale = 1;

        UnityEngine.Debug.Log(Statics.level);
        UnityEngine.Debug.Log(mazeSize);

		GenerateMaze(Statics.level);
		txt1.text = "Level " + Statics.level;
		txt2.text = "Level " + Statics.level;
		txt3.text = "Level " + Statics.level;
		
	}

	void Update() {
		timer = timer + Time.deltaTime;
		if (Statics.level == 6 && timer < 3 && Statics.isJump == true && Statics.wasEndOpen == false) {
			endCanvas.SetActive(true);
			Time.timeScale = 0;
			Statics.wasEndOpen = true;
		}

		else if (timer > 1 && Statics.died) {
			ad.ShowAd();
			Statics.died = false;
		}

	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	void CreateChildPrefab(GameObject prefab, GameObject parent, int x, int y, int z) {
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.transform.parent = parent.transform;
	}

	void GenerateMaze(int level) {

		// initialize map 2D array
		mapData = GenerateMazeData();

		// create actual maze blocks from maze boolean data
		for (int z = 0; z < mazeSize; z++) {
			for (int x = 0; x < mazeSize; x++) {
				if (mapData[z, x]) {
					CreateChildPrefab(wallPrefab, wallsParent, x, 1, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 2, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 3, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 4, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 5, z);
				} else if (!characterPlaced) {
					
					// place the character controller on the first empty wall we generate
					characterController.transform.SetPositionAndRotation(
						new Vector3(x, 2, z), Quaternion.identity
					);

					// flag as placed so we never consider placing again
					characterPlaced = true;
				}

				// create floor and ceiling && Random.value < 0.05
				if (x > 1 && z > 1 && Random.value < (0.05 + level / 100)  && level > 1)
					{}
				else{
                    CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				}

				if (generateRoof) {
					CreateChildPrefab(ceilingPrefab, wallsParent, x, 5, z);
				}
			}
		}

		// spawn the pickup at the end
		var myPickup = Instantiate(pickup, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
		myPickup.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    	
        //var myEnemy = Instantiate(enemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
		//myEnemy.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        
		if(level == 3) {
			var myEnemySlow = Instantiate(slowEnemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
			myEnemySlow.SetActive(true);
		}
		else if (level == 4)
		{
			var myEnemyFast = Instantiate(fastEnemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
            myEnemyFast.SetActive(true);
		}
		else if (level == 5)
		{
			var myEnemySlow = Instantiate(slowEnemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
			myEnemySlow.SetActive(true);

			var myEnemyFast = Instantiate(fastEnemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
            myEnemyFast.SetActive(true);
		}

		else if (level > 5)
		{
			var myEnemySlow = Instantiate(slowEnemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
			myEnemySlow.SetActive(true);

			var myEnemyFast = Instantiate(fastEnemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
            myEnemyFast.SetActive(true);

			for (int i = 1; i <= level - 5; i += 1) {
				var myEnemys = Instantiate(fastEnemy, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
				myEnemys.SetActive(true);
			}
		}

	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	bool[,] GenerateMazeData() {
		bool[,] data = new bool[mazeSize, mazeSize];

		// initialize all walls to true
		for (int y = 0; y < mazeSize; y++) {
			for (int x = 0; x < mazeSize; x++) {
				data[y, x] = true;
			}
		}

		// counter to ensure we consume a minimum number of tiles
		int tilesConsumed = 0;

		// iterate our random crawler, clearing out walls and straying from edges
		while (tilesConsumed < tilesToRemove) {
			
			// directions we will be moving along each axis; one must always be 0
			// to avoid diagonal lines
			int xDirection = 0, yDirection = 0;

			if (Random.value < 0.5) {
				xDirection = Random.value < 0.5 ? 1 : -1;
			} else {
				yDirection = Random.value < 0.5 ? 1 : -1;
			}

			// random number of spaces to move in this line
			int numSpacesMove = (int)(Random.Range(1, mazeSize - 1));

			// move the number of spaces we just calculated, clearing tiles along the way
			for (int i = 0; i < numSpacesMove; i++) {
				mazeX = Mathf.Clamp(mazeX + xDirection, 1, mazeSize - 2);
				mazeY = Mathf.Clamp(mazeY + yDirection, 1, mazeSize - 2);

				if (data[mazeY, mazeX]) {
					data[mazeY, mazeX] = false;
					tilesConsumed++;
				}
			}
		}

		return data;
	}
}
