using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxEncoder : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // Debug.Log(Encode("G F# F G Eb E G F# F Eb C C2 Bb G F C Eb E Eb C C2 G B Bb A Ab G C2 G F# F C Eb E C D Eb E G A Ab A G A G Eb E F# F Ab G F# F Eb C C2 Bb G F# F Eb C C2 Bb Ab G F G Eb D C"));

    }
	
    string Encode(string s)
    {
        string [] list = s.Split(' ');
        string encoded = "";
        for (int a = 0; a < list.Length; a++)
        {
            if (list[a].Equals("A"))
                encoded += "0";
            if (list[a].Equals("Ab"))
                encoded += "1";
            if (list[a].Equals("B"))
                encoded += "2";
            if (list[a].Equals("Bb"))
                encoded += "3";
            if (list[a].Equals("C#"))
                encoded += "4";
            if (list[a].Equals("C"))
                encoded += "5";
            if (list[a].Equals("C2"))
                encoded += "6";
            if (list[a].Equals("D"))
                encoded += "7";
            if (list[a].Equals("E"))
                encoded += "8";
            if (list[a].Equals("Eb"))
                encoded += "9";
            if (list[a].Equals("F#"))
                encoded += "A";
            if (list[a].Equals("F"))
                encoded += "B";
            if (list[a].Equals("G"))
                encoded += "C";
        }

        return encoded;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
