using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxPlayer : MonoBehaviour {
    public List<AudioClip> notes;
    public GameData data;
    public AudioSource source;
    private string sequence = "ABCDEFG";
    private bool finished = true;
    private int count = 0;
    private string code = "CABC98CAB9563CB598956C2301C6CAB5985798C010C0C98AB1CAB9563CAB95631CBC975";

	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
        source = GetComponent<AudioSource>();
    }
	
    int GetIndex(char c)
    {
        switch(c)
        {
            case '1': return 1; break;
            case '2': return 2; break;
            case '3': return 3; break;
            case '4': return 4; break; 
            case '5': return 5; break;
            case '6': return 6; break;
            case '7': return 7; break;
            case '8': return 8; break;
            case '9': return 9; break;
            case 'A': return 10; break;
            case 'B': return 11; break;
            case 'C': return 12; break;
        }

        return 0;
    }
	// Update is called once per frame
	void Update () {
        if(data.noteReady() && finished)
        {
            char c = code[count];
            source.clip = notes[GetIndex(c)];
            source.Play();
            finished = false;
            data.subNotes();

            count++;
            if (count >= code.Length)
                count = 0;

        }

        if (!source.isPlaying)
            finished = true;

		
	}
}
