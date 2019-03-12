using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    private GameData data;

    // Use this for initialization
    void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            if(data != null)
                data.LoadStart();
    }
}
