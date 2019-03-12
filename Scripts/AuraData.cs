using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraData : MonoBehaviour {
    private int aura;
    public List<Sprite> sprites;
    private SpriteRenderer rend;
    public void SetAura(int s)
    {
        aura = s;

        if (rend == null)
            rend = GetComponent<SpriteRenderer>();
        
        rend.sprite = sprites[aura];
    }
	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}


public enum Aura
{
    Melon,
    Rose
}
