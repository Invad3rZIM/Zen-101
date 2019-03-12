using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour {

    private GameData data;
    private int goldThresh, silverThresh;
    private int stars = 3;

    public int state;
	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();

        goldThresh = data.GetGold();
        silverThresh = (int)((double)goldThresh * 1.4) + 1;
        state = 4; //4 = gold. you can only go down from here.
        data.WinQuality = 3;
    }
	
	// Update is called once per frame
	void Update () {
        if (stars == 1)
            return;

        if (data.GetMoveCount() > goldThresh)
            SetSilver();
        if (data.GetMoveCount() > silverThresh)
            SetBronze();
	}

    private void SetSilver()
    {
        if (stars == 3)
        {
            transform.Find("Silver").gameObject.SetActive(true);
            transform.Find("Gold").gameObject.SetActive(false);
            stars = 2;
            state = 3;
            data.WinQuality = 2;
        }
    }
    private void SetBronze()
    {
        if (stars == 2)
        {
            transform.Find("Bronze").gameObject.SetActive(true);
            transform.Find("Silver").gameObject.SetActive(false);
            stars = 1;
            state = 2;
            data.WinQuality = 1;
        }
    }
}
