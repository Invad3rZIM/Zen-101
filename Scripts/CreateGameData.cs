using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameData : MonoBehaviour {
    public GameObject gameData;
	// Use this for initialization
	void Start () {
        if (GameObject.Find("GameData") == false)
            Instantiate(gameData);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
