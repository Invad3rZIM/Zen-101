using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    private GameData data;
    // Use this for initialization
    void Start () {

        data = GameObject.Find("GameData").GetComponent<GameData>();
    }
	
    public void StartMenu()
    {
        data.LoadStart();
    }

    //loads the next 15 levels on the levle selector.
    public void Next()
    {
        if (data.GetLevelGrouping() < 6)
        {
            data.NextGrouping();
            data.LoadLevelSelector(1);
        }
    }
    //loads the previous 15 levels on the level selector page.
    public void Prev()
    {
        if (data.GetLevelGrouping() > 0)
        {
            data.PrevGrouping();
            data.LoadLevelSelector(1);
        }
    }

    //THis method checks to see if the level is unlocked before loading it
    //if its unlocked, loads. otherwise, offer to unlock via add.
    public void T(int i)
    {
        i = i + data.GetLevelGrouping() * 15;

        if (data.IsUnlocked(i))
        {
            data.SetLevel(i);
            data.LoadLevel();
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
