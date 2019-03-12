using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMaster : MonoBehaviour {

    // Use this for initialization
    private GameData data;
    private Text description;
    // Use this for initialization
    void Start()
    {
        data = GameObject.Find("GameData").GetComponent<GameData>();
        description = GetComponent<Text>();
        string t = "" + data.GetLevel();
        string s = "";
        for (int a = 0; a < t.Length; a++)
            if (t[a] == '0')
                s += 'O';
            else
                s += t[a];
        description.text ="" + s;
    }
}
