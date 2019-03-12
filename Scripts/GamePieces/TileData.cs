using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {
    private int life;
    public bool IsEvil, IsGhostly;
    public Sprite[] sprites;
    private SpriteRenderer rend;
    private bool showTile;
    private GameData data;

    private int x, y;
	// Use this for initialization
	void Start () {
        this.x = (int)transform.position.x;
        this.y = (int)transform.position.y;

        if(rend == null)
            rend = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        data = GameObject.Find("GameData").GetComponent<GameData>();
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public void ShowTile()
    {
        rend.enabled = true;
    }

    public void HideTile() //mostly used for ghost tiles cause the bottom row pops out underneath a 
    {                      //dude and looks ugly. So the visual solution is if a zencritter is atop a tile,
                           //we hide it. Handled in GameMaster
        rend.enabled = false;
    }

    public int GetLives()
    {
        return life;
    }

	public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;

        transform.position = new Vector3(this.x, this.y, 0);
    }    

    public void SetLives(int life)
    {

        if (rend == null)
            rend = GetComponent<SpriteRenderer>();

        if (!IsGhostly)
        {

            return;
        }
        this.life = life;

        if (this.life == 0)
            gameObject.SetActive(false);
        else if (IsEvil)
            rend.sprite = sprites[9 + this.life];
        else
            rend.sprite = sprites[this.life];
        
    }

    //to make sure rend gets initiated.
    public void SetInitialLives(int life)
    {
        if(rend == null)
            rend = GetComponent<SpriteRenderer>();

        if (!IsGhostly)
        {

            rend.color = data.GetRandomFade();
            return;
        }
        this.life = life;

        if (life <= 0)
            gameObject.SetActive(false);
        else if (IsEvil)
            rend.sprite = sprites[9 + life];
        else
            rend.sprite = sprites[this.life];
        
        rend.color = data.GetRandomFade(); //aesthetic tint.
    }

    public void LoseLife()
    {
        if (!IsGhostly)
            return;

        SetLives(this.life - 1);
    }
}
