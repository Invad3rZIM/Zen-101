using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadHintScript : MonoBehaviour {
    GameData data;
    
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();

        if (data.TotalHints() > 0) //if the level actually has hints
            transform.FindChild("LightBulb").gameObject.SetActive(true);
    }
}
