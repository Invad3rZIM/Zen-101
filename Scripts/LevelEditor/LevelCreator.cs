using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour {
    public GameObject [] objects;
    public int level;
    public string description;
    public int offsetX = -16;
    public int offsetY = -16;
    private char[,] board;
    private string[,] zencritters;
    private char[,] zenhomes;
    private int curX, curY;
    private char currC;
    private int lives = 0;
    private int aura = 0;
    private Dictionary<string, GameObject> instantiated;
    private Dictionary<string, GameObject> critters;
    private Dictionary<string, GameObject> homes;

    private GUIText outputText;
    public int critType;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (outputText == null)
            outputText = GameObject.Find("Debug").GetComponent<GUIText>();
        outputText.text = "char: " + currC + "\nlife: " + lives + "\ncritType: " + critType + "\naura: " + aura;

        if (Input.GetMouseButton(0))
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            curX = (int)((x-(float)offsetX) / 32f) -2;
            curY = (int)((y- (float)offsetY) / 32f) -2;

            if (curX > 13 || curX< 0)
                return;
            if (curY  > 13 || curY < 0)
                return;
            
            if (currC - 'a' >= 0 && currC - 'c' <= 0)
                AddTile();

            if (Input.GetMouseButtonDown(0) && currC == 'd')
                AddCritter();
            if (Input.GetMouseButtonDown(0) && currC == 'e')
                AddHome();

            if (currC == '?')
                DeleteTile(curX , curY );

            if (currC == '@')
            {

                if (critters == null)
                    critters = new Dictionary<string, GameObject>();

                if (homes == null)
                    homes = new Dictionary<string, GameObject>();

                DeleteCritter((curX ), curY);
                DeleteHome((curX ) , curY);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.A)) currC = 'a';
        if (Input.GetKeyDown(KeyCode.B)) currC = 'b';
        if (Input.GetKeyDown(KeyCode.C)) currC = 'c';
        if (Input.GetKeyDown(KeyCode.D)) currC = 'd';

        if (Input.GetKeyDown(KeyCode.E)) currC = 'e';


        if (Input.GetKeyDown(KeyCode.Delete)) currC = '@';


        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            aura = (int)aura / 10;
            critType = (int)critType / 10;
            currC = '?';
        }

        if (Input.GetKeyDown(KeyCode.Return)) SaveGrid();
        if (Input.GetKeyDown(KeyCode.Alpha1)) lives = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) lives = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) lives = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) lives = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) lives = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) lives = 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) lives = 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) lives = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) lives = 9;
        if (Input.GetKeyDown(KeyCode.Alpha0)) critType = critType * 10;
        if (Input.GetKeyDown(KeyCode.Alpha1)) critType = critType * 10 + 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) critType = critType * 10 + 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) critType = critType * 10 + 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) critType = critType * 10+ 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) critType = critType * 10+  5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) critType = critType * 10+ 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) critType = critType * 10 + 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) critType = critType * 10  +8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) critType = critType * 10  +9;

        if (Input.GetKeyDown(KeyCode.Keypad0)) aura = aura * 10;
        if (Input.GetKeyDown(KeyCode.Keypad1)) aura = aura * 10 + 1;
        if (Input.GetKeyDown(KeyCode.Keypad2)) aura = aura * 10 + 2;
        if (Input.GetKeyDown(KeyCode.Keypad3)) aura = aura * 10 + 3;
        if (Input.GetKeyDown(KeyCode.Keypad4)) aura = aura * 10 + 4;
        if (Input.GetKeyDown(KeyCode.Keypad5)) aura = aura * 10 + 5;
        if (Input.GetKeyDown(KeyCode.Keypad6)) aura = aura * 10 + 6;
        if (Input.GetKeyDown(KeyCode.Keypad7)) aura = aura * 10 + 7;
        if (Input.GetKeyDown(KeyCode.Keypad8)) aura = aura * 10 + 8;
        if (Input.GetKeyDown(KeyCode.Keypad9)) aura = aura * 10 + 9;
    }

    void AddCritter()
    {

        if (critters == null)
            critters = new Dictionary<string, GameObject>();
        if (zencritters == null)
            zencritters = new string[14, 14];

        string s = "" + curX + "." + curY;
        
        GameObject g = Instantiate(objects[10 + critType]);
        g.transform.position = new Vector3(curX, curY, 0);

        CharacterData c = g.GetComponent<CharacterData>();
        
        c.SetInitialPosition(new Vector2(curX , curY));
        c.SetAura(aura);
        zencritters[curY, curX] = (string)("" + ((char)((int)'a' + critType)) + "" + (char)('a' + (aura)));
        
        critters[s] = g;
    }

    void DeleteHome(int x, int y)
    {
        if (zenhomes == null)
            zenhomes = new char[14, 14];
        if (x > 0)
            x--;
        if(x >= 0 && y >= 0 && x < 14 && x < 14)
        zenhomes[y, x] = '\0';
        if (homes.ContainsKey("" + x + "." + y))
            if (homes["" + x + "." + y] != null)
            {
                GameObject g = homes["" + x + "." + y];
                Destroy(g);

                homes["" + x + "." + y] = null;
            }
    }
    void AddHome()
    {

        if (homes == null)
            homes = new Dictionary<string, GameObject>();

        if (zenhomes == null)
            zenhomes = new char[14, 14];
        string s = "" + (curX ) + "." + curY;
       
        DeleteHome(s);
        
      
        GameObject g = Instantiate(objects[20 + critType]);
        g.transform.position = new Vector3(curX, curY, 0);
        zenhomes[curY, curX] = (char)((int)'a' + critType);

        homes[s] = g;
    }
    /*_____________________________________________aaaaaaaa______aaaaaaaa______________________________________________________________aaaaaaaa______aaaaaaaa_____________________________________________$c39a93$cd99aa33$
UnityEngine.Debug:Log(Object)
LevelCreator:SaveGrid() (at Assets/Scripts/LevelEditor/LevelCreator.cs:344)
LevelCreator:Update() (at Assets/Scripts/LevelEditor/LevelCreator.cs:92)



    */

    void AddTile()
    {
        if (instantiated == null)
            instantiated = new Dictionary<string, GameObject>();
        if (board == null)
            board = new char[14, 14];
        
        
        if (currC == 'a') //base tile
        {
            GameObject g = Instantiate(objects[0]);
            DeleteTile(curX , curY );
            g.transform.position = new Vector3(curX, curY, 0);
           
            string s = "" + curX + "." + curY;
            board[curY, curX] = 'a';
            instantiated[s] = g;

        }
        if (currC == 'c' || currC == 'b') //c=evil, b = ghost
        {
            int ind = 1;
            if (currC == 'b')
                ind++;
            GameObject g = Instantiate(objects[ind]);
            DeleteTile(curX, curY);
            TileData data = g.GetComponent<TileData>();

            data.SetPosition(curX, curY);
            data.SetLives(lives);

            string s = "" + curX + "." + curY;

            if (ind == 2) //saves it to board for serialization
                board[curY, curX] = (char)((int)'a' + lives);
            if (ind == 1)

                board[curY, curX] = (char)((int)'j' + lives);

            instantiated[s] = g;
        }
    }

    
    int GetInt(char c)
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
    public void SaveGrid()
    {
        if (zencritters == null)
            zencritters = new string[14, 14];
        if (zenhomes == null)
            zenhomes = new char[14, 14];
        if (board == null)
            board = new char[14, 14];
        string s = "";

        for (int a = 0; a < board.GetLength(0); a++)
        {
            for (int b = 0; b < board.GetLength(1); b++)
            {
                if (board[a, b] != '\0')
                {
                    s += board[a, b];
                }
                else
                    s += '_';
            }

        }
        

        Block g = new Block('0', 0, 0, 0, 0);
        s += "$";
        for (int a = board.GetLength(0) - 1; a >= 0; a--)
        {
            for (int b = 0; b < board.GetLength(1); b++)
                if (zenhomes[a, b] != '\0')
                    s += zenhomes[a,b] + "" + g.GetChar(b) + "" + g.GetChar(a);
        }
        s += "$";
        for (int a = board.GetLength(0) - 1; a >= 0; a--)
        {
            for (int b = 0; b < board.GetLength(1); b++)
            {
                if (zencritters[a, b] != null && zencritters[a,b].Length > 0)
                {
                    s += zencritters[a, b] + "" + g.GetChar(b) + "" + g.GetChar(a);
                }
            }
        }
        s += "$";

        Debug.Log(s);
    }

    void DeleteCritter(string s)
    {
        if (critters.ContainsKey(s))
            if (critters[s] != null)
            {
                GameObject g = critters[s];
                Destroy(g);

                critters[s] = null;
            }
    }

    void DeleteHome(string s)
    {
        if (homes.ContainsKey(s))
            if (homes[s] != null)
            {
                GameObject g = homes[s];
                Destroy(g);

                homes[s] = null;
            }
    }

    void DeleteCritter(int x, int y)
    {
        if(zencritters == null)
            zencritters = new string[14, 14];
        if (x > 0)
            x--;
        if(x >= 0 && y < 14 && x < 14 && y >= 0)
        zencritters[y, x] = "";
        if (critters.ContainsKey(""+x+"."+y))
            if (critters["" + x + "." + y] != null)
            {
                GameObject g = critters["" + x + "." + y];
                Destroy(g);

                critters["" + x + "." + y] = null;
            }
    }

    void DeleteTile(float x, float y)
    {

        string s = "" + x + "." + y;

        board[(int)y,(int) x] = '\0';
        if (instantiated.ContainsKey(s))
        {
            if (instantiated[s] != null)
            {
                GameObject g = instantiated[s];
                Destroy(g);

                instantiated[s] = null;
            }
        }
    }

    public class Block
    {
        

        private char a;
        public int x,y, w;
        public int h;


        public Block(char a, int x, int y, int w, int h)
        {
            this.a = a;
            
            this.y = y;
            this.x = x;
            this.w = w;
            this.h = h;
        }
        public int GetLayers()
        {
            return h;
        }

        public void addLayer(int l)
        {
            h++;
        }
        public char GetChar(int z)
        {
            if (z == 1)
                return '1';
            if (z == 2)
                return '2';
            if (z == 3)
                return '3';
            if (z == 4)
                return '4';
            if (z == 5)
                return '5';
            if (z == 6)
                return '6';
            if (z == 7)
                return '7';
            if (z == 8)
                return '8';
            if (z == 9)
                return '9';
            if (z == 10)
                return 'a';
            if (z == 11)
                return 'b';
            if (z == 12)
                return 'c';
            if (z == 13)
                return 'd';
            if (z == 14)
                return 'e';
            if (z == 15)
                return 'f';

            return '0';
        }
        public override string ToString()
        {
            return "" + a + "" + GetChar(x) + "" + GetChar(y);
        }

        public int GetWidth()
        {
            return w;
        }

        public string Serialize()
        {
            Debug.Log(x + "  " + y + "  " +w + "  " + h);
            return "" + a + "" + GetChar(x) + "" + GetChar(y) + "" + GetChar(w) + "" + GetChar(h);
        }
        
    }
}
