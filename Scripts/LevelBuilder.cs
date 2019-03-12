using UnityEngine;
using System.Collections.Generic;

//This class generates all Objects, then hands the Dictionaries off to the GameMasterClass to process.
//THis class reads from the database, given a certain level, and generates GameObject hashmaps according
//to the level hash found at that database.
public class LevelBuilder : MonoBehaviour {
    public List<GameObject> Prefabs;
	
    private string description, hash;
    private int level;
    private char[,] boardMap;
    private Dictionary<string, TileData> evilTiles, allTiles;
    private Dictionary<string, ZenHouseData> zenhouses;
    private Dictionary<string, CharacterData> crittersByPos;
    private Dictionary<int, CharacterData> crittersByID;
    private List<int> critterIDs;
    
    //these methods are all called by GameMaster class when he's ready.
    public List<int> GetCritterIDs()
    {
        return critterIDs;
    }
    public Dictionary<string, TileData> GetEvilTiles()
    {
        return evilTiles;
    }
    public Dictionary<string, TileData> GetAllTiles()
    {
        return allTiles;
    }
    public Dictionary<string, CharacterData> GetCrittersByPos()
    {
        return crittersByPos;
    }
    public Dictionary<string, ZenHouseData> GetZenHouses()
    {
        return zenhouses;
    }
    public Dictionary<int, CharacterData> GetCrittersByID()
    {
        return crittersByID;
    }

    void Start () 
	{
        CreateHashMaps(); //initializes our <string, GameObject> hashmaps
        GetLevelParameters(); //gets hash, description
        GenerateLevel(); //processes level hash, creates gameobjects

        GetComponent<GameMaster>().enabled = true;
	}

    void AddZenCritter(char type, int aura, int x, int y, int id)
    {
        int index = 10 + type - 'a';

        GameObject critter = Instantiate(Prefabs[index]);
        critter.GetComponent<CharacterData>().SetInitialPosition(new Vector2(x, y));
        aura -= 10;
        critter.GetComponent<CharacterData>().SetAura(aura);
        CharacterData data = critter.GetComponent<CharacterData>();
        data.SetID(id);

        crittersByPos["" + x + "." + y] = data;
        crittersByID[id] = data;
        critterIDs.Add(id);
    }

    void GenerateLevel()
    {
        if (hash.Length == 0)
            return;
        char current = hash[0];
        
        int index = 0, yAxis = 0, xAxis = 0;
        //reads board tiles
        while(hash[index] != '$')
        {
            if(hash[index] != '_')
                AddBoardMap(hash[index], xAxis, yAxis);
            index++;

            xAxis++;

            if(xAxis == 14)
            {
                yAxis++;
                xAxis = 0;
            }
        }

        index++;
        while (hash[index] != '$')
        {
            AddZenHome(hash[index], GetInt(hash[index + 1]), GetInt(hash[index + 2]));
            index+=3;
        }

        index++;
        current = hash[index];

        //reads critter pieces
        while (hash[index] != '$')
        {
            AddZenCritter(hash[index], GetInt(hash[index + 1]), GetInt(hash[index + 2]), GetInt(hash[index + 3]), index);
            index += 4;
        }
        
    }

    private void AddZenHome(char a, int x, int y)
    {
        int index = 20 + (int)a - 'a';
        GameObject home = Instantiate(Prefabs[index]);
        home.transform.position = new Vector3(x, y, 0);

        zenhouses["" + x + "." + y] = home.GetComponent<ZenHouseData>();
    }

    private void CreateHashMaps()
    {
        evilTiles = new Dictionary<string, TileData>();
        allTiles = new Dictionary<string, TileData>();

        crittersByID= new Dictionary<int, CharacterData>();
        critterIDs = new List<int>();

        crittersByPos = new Dictionary<string, CharacterData>();
        zenhouses = new Dictionary<string, ZenHouseData>();
    }

    private void GetLevelParameters()
    {
        hash = GameObject.Find("GameData").GetComponent<GameData>().GetHash();
        description = GameObject.Find("GameData").GetComponent<GameData>().GetDescription();
    }

    private void AddBoardMap(char a, int x, int y)
    {
        if (boardMap == null)
            boardMap = new char[14, 14];
       
        int tileType = 0;
        int lives = 0;

        if(a != 'a') //derives tileType and lives from a
        {
            if(a >= 'b' && a <= 'j')
            {
                tileType = 1;
                lives = (int)(a - 'a');
            } else if(a >= 'k' && a <= 's')
            {
                tileType = 2;
                lives = (int)(a - 'a') - 9;
            }
        }
        
        GameObject tile = Instantiate(Prefabs[tileType]);
        tile.transform.position = new Vector3(x, y, 0);
        TileData data = tile.GetComponent<TileData>();

        data.SetInitialLives(lives);

        string key = "" + x + "." + y;

        allTiles[key] = tile.GetComponent<TileData>();

        if(data.IsEvil)
            evilTiles[key] = tile.GetComponent<TileData>();
    }

    //helper method for my char conversion values
    private int GetInt(char c)
    {
        switch(c)
        {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            case 'a': return 10;
            case 'b': return 11;
            case 'c': return 12;
            case 'd': return 13;
            case 'e': return 14;
            case 'f': return 15;
            case 'g': return 16;
            case 'h': return 17;
            case 'i': return 18;
            case 'j': return 19;
            case 'k': return 20;
            case 'l': return 21;
            case 'm': return 22;
            case 'n': return 23;
            case 'o': return 24;
            case 'p': return 25;
            case 'q': return 26;
            case 'r': return 27;
            case 's': return 28;
            case 't': return 29;
            case 'u': return 30;
            case 'v': return 31;
            default: return 0;
        }
    }
    //Accesses GameData Simpleton to get current level. Uses level to find its contents in the Zen.sql database
}