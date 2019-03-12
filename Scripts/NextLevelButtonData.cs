using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButtonData : MonoBehaviour {
    public int endPost;
    private GameData data;

	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();

        if (data.currentLevel == endPost)
            gameObject.SetActive(false);
    }
}
