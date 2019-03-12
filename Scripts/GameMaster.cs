using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    GameData data;

    private GUIText debug;

    private Dictionary<string, ZenHouseData> zenHouses;
    private Dictionary<string, CharacterData> crittersByPos;
    private Dictionary<string, TileData> evilTiles, allTiles;
    private Dictionary<int, CharacterData> crittersByID;
    private List<int> critterIDs;

    private bool timeToCheckHappy = true;
    private int timer = 0;

    public void ItsTimeToCheckHappy()
    {
        timeToCheckHappy = true;
    }
    // Use this for initialization
    void Start() {
        LevelBuilder b = GetComponent<LevelBuilder>();

        //sets values of all our hashmaps we use to track gamestate.
        evilTiles = b.GetEvilTiles();
        allTiles = b.GetAllTiles();
        zenHouses = b.GetZenHouses();
        crittersByPos = b.GetCrittersByPos();

        critterIDs = b.GetCritterIDs();
        crittersByID = b.GetCrittersByID();

        List<string> zenKeys = new List<string>(zenHouses.Keys);

        b.enabled = false;

        data = GameObject.Find("GameData").GetComponent<GameData>();
        HideGhosts();
    }

    public bool ContainsZenHouse(int x, int y)
    {
        string key = "" + x + "." + y;

        return zenHouses.ContainsKey(key);
    }

    public ZenHouseData GetZenHouse(int x, int y)
    {
        return zenHouses["" + x + "." + y];
    }

    //returns a list of cardinally adjacent CharacterDatas that are identical
    //skull colors.
    public List<CharacterData> GetSameSkulls(int x, int y, string skullColor)
    {
        List<CharacterData> friendlyNeighbors = new List<CharacterData>();

        if (ContainsCritter(x - 2, y) && GetCritter(x-2, y).SkullColor.Equals(skullColor))
            friendlyNeighbors.Add(GetCritter(x - 2, y));

        if (ContainsCritter( x, y -2) && GetCritter(x , y-2).SkullColor.Equals(skullColor))
            friendlyNeighbors.Add(GetCritter(x, y-2));

        if (ContainsCritter(x + 2, y) && GetCritter(x + 2, y).SkullColor.Equals(skullColor))
            friendlyNeighbors.Add(GetCritter(x + 2, y));

        if (ContainsCritter(x, y + 2) && GetCritter(x, y + 2).SkullColor.Equals(skullColor))
            friendlyNeighbors.Add(GetCritter(x, y + 2));

        return friendlyNeighbors;
    }

    //if we're on top of an evil tile, zen freak out.
    public bool StandingAtopEvil(int x, int y)
    {
        if (!IsThereEvil())
            return false;

        List<string> keys = new List<string>();

        keys.Add("" + x + "." + y);
        keys.Add("" + (x+1) + "." + y);
        keys.Add("" + x + "." + (y + 1));
        keys.Add("" + (x + 1) + "." + (y + 1));

        for (int a = 0; a < keys.Count; a++)
            if (evilTiles.ContainsKey(keys[a]))
                return true;        

        return false;
    }

    public bool ContainsTile(int x, int y)
    {
        string key = "" + x + "." + y;

        return allTiles.ContainsKey(key);
    }

    //returns yes if any evil tiles exist
    private bool IsThereEvil()
    {
        return (evilTiles.Keys.Count > 0);
    }

    //returns whether or not a critter is found at the hashmap location
    public bool ContainsCritter(int x, int y)
    {
        string key = "" + x + "." + y;

        return crittersByPos.ContainsKey(key);
    }

    //gets the zencritter at a position in the hashmap
    public CharacterData GetCritter(int x, int y)
    {
        string key = "" + x + "." + y;

        return crittersByPos[key];
    }

    //removes a zencritter from the hashmap.
    public void ClearCritterPosition(int x, int y)
    {
        
        string key = "" + x + "." + y;

        if (ContainsCritter(x, y))
            crittersByPos.Remove(key);
    }

    //Called when a critters steps off a tile. decrements life if tile IsGhostly
    public void DecrementGhost(int x, int y)
    {
        data.addNote();
        string key = "" + x + "." + y;

        TileData t = allTiles[key];

        if (t.IsGhostly)
        {
            t.LoseLife();
            t.ShowTile();

            if (t.GetLives() < 1)
                KillTile(x, y);
        }
    }

    //removes tile from its hashmaps and sets it to inactive. happens to ghost/evil
    //tiles when they run out of lives.
    private void KillTile(int x, int y)
    {
        string key = "" + x + "." + y;

        if (evilTiles.ContainsKey(key))
            evilTiles.Remove(key);

        if (allTiles.ContainsKey(key))
        {
            allTiles[key].enabled = false;
            allTiles.Remove(key);
        }
    }


    public void SetCritterPosition(int x, int y, CharacterData c)
    {
        string key = "" + x + "." + y;

        if (allTiles[key].IsGhostly)
            allTiles[key].HideTile();

        string newKey = "" + (x + 1) + "." + y;

        if (this.ContainsTile(x + 1, y))
            if (allTiles["" + (x + 1) + "." + y].IsGhostly)
                        allTiles["" + (x + 1) + "." + y].HideTile();

        crittersByPos[key] = c;
    }

    //Hides ghost tiles when they're underneatha  tile.
    //This Is Purely visual aesthetic.
    private void HideGhosts()
    {
        List<string> occupiedTiles = new List<string>(crittersByPos.Keys);

        for (int a = 0; a < occupiedTiles.Count; a++)
        {
            if(allTiles[crittersByPos[occupiedTiles[a]].GenKey(0,0)].IsGhostly)
                allTiles[crittersByPos[occupiedTiles[a]].GenKey(0, 0)].HideTile();
            
            if(allTiles.ContainsKey(crittersByPos[occupiedTiles[a]].GenKey(1, 0)))
                if (allTiles[crittersByPos[occupiedTiles[a]].GenKey(1, 0)].IsGhostly)
                    allTiles[crittersByPos[occupiedTiles[a]].GenKey(1, 0)].HideTile();
        }
             
    }

    public TileData GetTile(int x, int y)
    {
        string key = "" + x + "." + y;

        if (ContainsTile(x, y))
            return allTiles[key];

        return null;
    }

    // Update is called once per frame
    void Update() {
        timer++;
        if (timeToCheckHappy || timer > 3)
        {
            timer = 0;
            timeToCheckHappy = false;
            
            bool flag = false;
            for (int a = 0; a < critterIDs.Count; a++)
                if (!crittersByID[critterIDs[a]].IsHappy())
                    flag = true;

            if (flag)
                return;
            
            if (IsThereEvil()) //if any evil tiles exist
                return;

            LevelComplete();
        }
	}

    //Triggered Upon Beating A Level
    void LevelComplete()
    {
        GameObject.Find("Canvas").gameObject.transform.FindChild("Victory").gameObject.SetActive(true);
        //GameObject.Find("GameData").GetComponent<GameData>().NextLevel();
    }
}
