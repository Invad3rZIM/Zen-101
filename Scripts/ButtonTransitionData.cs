using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTransitionData : MonoBehaviour {
    public int ScreenThreshhold;
    private GameData data;

	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();

        if (data.GetLevelGrouping() == ScreenThreshhold)
            this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
