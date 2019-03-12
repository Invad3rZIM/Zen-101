using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionMaster : MonoBehaviour {
    private GameData data;
    private Text description;
	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
        description = GetComponent<Text>();
        description.text = data.GetDescription() ;
    }
	
}
