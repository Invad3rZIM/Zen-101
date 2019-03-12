using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {
    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void PlayGame()
    {
        GameObject.Find("GameData").GetComponent<GameData>().LoadLevel();
        source.Play();
    }

    public void ToggleMute()
    {
        GameObject.Find("GameData").GetComponent<GameData>().ToggleMute();
    }

    public void LevelSelect()
    {
        GameObject.Find("GameData").GetComponent<GameData>().LoadLevelSelector();
    }

    public void Options()
    {
        GameObject.Find("GameData").GetComponent<GameData>().LoadOptions();
    }
}

//1140 bloomfield montclair radiology 202 suite
