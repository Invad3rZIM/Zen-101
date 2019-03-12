using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameData : MonoBehaviour {

    public List<AudioClip> Music;
    private SimpleSQL.SimpleSQLManager dbManager;
    public GameData i;
    public int currentLevel;
    private int bestMoves, personalMoves, moveCount;
    private int levelGrouping;
    public string hash, description;
    private int currentCritterID;
    private int currentStars = -1, blockAds = 0, adCount = 0;
    private Dictionary<int, LevelStateInfo> levelStates;
    private int noteCount = 0;
    private UnityEngine.Color[] tileFades;
    private string[] hints = new string[3];
    private int maxHints, hintsUnlocked;
    public int WinQuality; // 3 = gold, 2 = silver, 1 = bronze.
    private AudioSource source;
    private bool playingIntro;
    private bool giveHint;
    public int lastAdd = 0;
    private bool unlockState;

    //there's up to 3 hints per level, and they're unlocked via watching ads.
    //hintsunlocked is dynamic and gets updated as the player watches ads to unlock hints.
    public int TotalHints()
    {
        return maxHints;
    }

    //how many hints has the player unlocked?
    public int HintsUnlocked()
    {
        return hintsUnlocked;
    }
    

    public void AddHint()
    {
        hintsUnlocked++;
        UpdateHintState(currentLevel, hintsUnlocked);
    }
    
    public string GetHint(int i)
    {
        if(i < maxHints)
            return hints[i];

        return null;
    }

    //levelgrouping is used for figuring out the numerical values that go on
    //the level select blocks.
    public int GetLevelGrouping()
    {
        return levelGrouping;
    }

    //sets the random colors that will be overlaid on the tiles so to add variety in graphics.
    //its done here instead of in game as a memoization technique, time complexity of 1 ingame
    //with an initial generation time on level start.
    private void GenTileFades()
    {
        tileFades = new UnityEngine.Color[100];

        for(int a = 0; a < tileFades.Length; a++)
            tileFades[a] = new UnityEngine.Color(Random.Range(.85f, 1f), Random.Range(.75f, 1f), Random.Range(.75f, 1f), 1f); ;

    }
    
    public UnityEngine.Color GetRandomFade()
    {
        return tileFades[Random.Range(0, 99)];
    }


    //Notes Are Used For Music Box tiling.
    public int GetNoteCount()
    {
        return noteCount;
    }

    public void addNote()
    {
            noteCount++;
    }

    public void subNotes()
    {
        noteCount--;
        noteCount--;
    }

    public bool noteReady()
    {
        return noteCount > 0;
    }

    //pause menu.
    public void ToggleLevelSelect()
    {
        GameObject g = GameObject.Find("Canvas").transform.FindChild("LevelSelectPrompt").gameObject;
        g.SetActive(!g.activeSelf);
    }
    public void SetLevelGrouping()
    {
        //1-15, 16-30, 31-45, 46-60, 61-75, 76-90, 91-101
        if (currentLevel < 16)
        {
            levelGrouping = 0;
            return;
        }

        if (currentLevel < 31)
        {
            levelGrouping = 1;
            return;
        }
        if (currentLevel < 46)
        {
            levelGrouping = 2;
            return;
        }
        if (currentLevel < 61)
        {
            levelGrouping = 3;
            return;
        }
        if (currentLevel < 76)
        {
            levelGrouping = 4;
            return;
        }
        if (currentLevel < 91)
        {
            levelGrouping = 5;
            return;
        }

        levelGrouping = 6;
    }
    
    //Establishes Singleton
    void Awake()
    {
        if (GameObject.Find("GameData") == null && GameObject.Find("GameData(Clone)").GetComponent<GameData>().i == null) 
        {
            this.name = "GameData";
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject, 1f);
            this.enabled = false;
        }
    }
    
    public void SetLevel(int a)
    {
        currentLevel = a;
    }


    public int GetLevel()
    {
        return currentLevel;
    }

    public string GetHash()
    {
        return hash;
    }

    
    public void AddMove()
    {
        moveCount++;
    }

    public int GetMoveCount()
    {
        return moveCount;
    }

    //max amt of moves to get gold on a level. used for calculating stars.
    public int GetGold()
    {
        return bestMoves;
    }

    //id of the critter we're touching (if we're touching it)
    public int GetCurrentCritter()
    {
        return currentCritterID;
    }

    //sets the value of the critter we're tracking (to keep track of moves)
    public void NewCritter(int id)
    {
        AddMove();
        currentCritterID = id;
    }
    //gets the level description. this variable is set in LevelParameter() method,
    //and is queried from the level DB.
    public string GetDescription()
    {
        return description;
    }

    //goes forward next level
    public void NextLevel()
    {
        SetLevel(GetLevel() + 1);
        LoadLevel();
    }
    //goes back one level
    public void PrevLevel()
    {
        SetLevel(GetLevel() - 1);
        LoadLevel();
    }

    //loads the level associated with the value of CurrLevel
    public void LoadLevel()
    {
        if (CanLoad())
        {
            GetLevelParameters();
            moveCount = 0;
            currentCritterID = 0;
            noteCount = 0;
            hintsUnlocked = GetHintState(currentLevel);
            SceneManager.LoadScene("Game");
        }
    }

    //used as a singleton to transition between scenes.
    public void LoadStart()
    {
        SceneManager.LoadScene("Start");
    }

    public void LoadOptions()
    {

        SceneManager.LoadScene("Options");
    }
    public void LoadLevelSelector()
    {
        SetLevelGrouping();
        SceneManager.LoadScene("LevelSelector");
    }

    public void ToggleHints()
    {
        GameObject g = GameObject.Find("Canvas").transform.FindChild("Hints").gameObject;
        g.SetActive(!g.activeSelf);
    }

    /*Grouping is used to keep track of the 15 levels displayed at a time in level select.
    Each button is hardcoded to a number (1- 15), then that number is add to (levelGrouping * 15)
    to get the actual value of the button. For instance, level 7 is on the first Grouping index (0),
    and is derived at 7 + (0 * 15), when you hit the next button, nextGrouping() is called,
    and that button that had a 7 on it is now 22, (7 + (1*15)). LevelGrouping is handled via the
    LoadLevel Script attached to LevelSelectorManager gameObject.*/
    public void NextGrouping()
    {
        levelGrouping++;
    }

    public void TryToUnlockHint() //put ad here.
    {
        giveHint = false;
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult3(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                unlockState = true;
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
    }


    void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult2;

        Advertisement.Show("video", options);
    }

    void HandleShowResult2(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            lastAdd = 0;
        }
        else if (result == ShowResult.Skipped)
        {
        }
        else if (result == ShowResult.Failed)
        {
        }
    }

    private void HandleShowResult(ShowResult result)
              {
                switch (result)
                {
                  case ShowResult.Finished:
                giveHint = true;
                Debug.Log(giveHint); 
                    break;
                  case ShowResult.Skipped:
                    break;
                  case ShowResult.Failed:
                    break;
                }
              }
    public void PrevGrouping()
    {
        levelGrouping--;
    }

    public void LoadLevelSelector(int i)
    {
        SceneManager.LoadScene("LevelSelector");
    }

    //used to see if the level the user is tryna access is available.
    public bool IsUnlocked(int level)
    {
        return levelStates[level].IsUnlocked();
    }

    public void PromptNextLevelAd()
    {
        
        GameObject g = GameObject.Find("Canvas").transform.FindChild("UnlockStuff").gameObject;
        g.SetActive(!g.activeSelf);
    }

    public void WatchUnlockAd()
    {
        unlockState = false;
        
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult3 };
            Advertisement.Show("rewardedVideo", options);
        }
    }
    
    private bool CanLoad()
    {
        if (currentLevel < 1 || currentLevel > 101)
            return false;

        return true;
    }

    //Quit is gonna update the database whenever the user steps away from the screen.
    //this allows for Update Executions at a point where the user wont be caring about lag.
    void OnApplicationQuit()
    {
        UpdateCurrentLevel();
        UpdateLevelStateDB();
    }
    void OnApplicationPause()
    {
        UpdateCurrentLevel();
        UpdateLevelStateDB();
    }

    public void UpdateCurrentLevel()
    {
        if (dbManager != null)
        {
            dbManager.Execute("UPDATE ZenPlayerData SET CurrentLevel = ?", currentLevel);
        }
    }

    //loads initial values at the beginning of each app opening.
    void LoadStartingData()
    {
        List<PlayerInfo> data = dbManager.Query<PlayerInfo>("SELECT Z.CurrentLevel, Z.CurrentStars, Z.BlockAds, Z.AdCount FROM ZenPlayerData Z");

        currentLevel = data[0].CurrentLevel;
        currentStars = data[0].CurrentStars;
        blockAds = data[0].BlockAds;
        adCount = data[0].AdCount;

        currentStars = 3;

    }

    void GetLevelParameters()
    {
        List<Level> levels = dbManager.Query<Level>("SELECT L.LevelNumber, L.LevelDescription, L.LevelSerialization, L.LevelHint1, L.LevelHint2, L.HintsTotal, L.LevelHint3, L.LevelBestMoves, L.LevelPersonalMoves " +
                                                        "FROM ZenLevels L WHERE L.LevelNumber == " + currentLevel);
        if (levels.Count != 0)
        {
            description = levels[0].LevelDescription;
            hash = levels[0].LevelSerialization;
            bestMoves = levels[0].LevelBestMoves;
            personalMoves = levels[0].LevelPersonalMoves;
            hints[0] = levels[0].LevelHint1;
            hints[1] = levels[0].LevelHint2;
            hints[2] = levels[0].LevelHint3;
            maxHints = levels[0].HintsTotal;

           // levelFinished = levels[0].LevelFinished; //0 = no, 1 = yes, 2 = yes, optimum moves
        }
    }

    void Start()
    {
        if (dbManager == null)
            if (GetComponent<SimpleSQL.SimpleSQLManager>())
                dbManager = GetComponent<SimpleSQL.SimpleSQLManager>();

        source = GetComponent<AudioSource>();
        source.clip = Music[0];
        playingIntro = true;
        source.Play();

        Advertisement.Initialize("1595089");
        GenTileFades(); //generates random colors for tile aesthetics.
        ReadLevelStates();
    }



    //queries levelstates from database. (01234) are possible staetes.
    void ReadLevelStates()
    {
        if (dbManager == null)
            if (GetComponent<SimpleSQL.SimpleSQLManager>())
                dbManager = GetComponent<SimpleSQL.SimpleSQLManager>();

        levelStates = new Dictionary<int, LevelStateInfo>();
        List<Level> levels = dbManager.Query<Level>("SELECT L.LevelNumber, L.LevelState, L.HintsUnlocked FROM ZenLevels L");

        for (int a = 0; a < levels.Count; a++)
            levelStates[levels[a].LevelNumber] = new LevelStateInfo(levels[a].LevelNumber, levels[a].LevelState, levels[a].HintsUnlocked);
    }

    //Gets the state (01234) for the given level. mostly used by Buttons in levelSelect...
    public int GetLevelState(int level)
    {
        if(levelStates.ContainsKey(level))
            return levelStates[level].GetState();
        return 0;
    }

    //how many hints has the player unlocked?
    public int GetHintState(int level)
    {
        if (levelStates.ContainsKey(level))
            return levelStates[level].GetHints();
        return 0;
    }

    //level states. 0 = not unlocked, 1 = unlocked unbeaten, 2+ = bronze, silver, gold
    public void UpdateState(int level, int state)
    {
        levelStates[level].UpdateState(state);
    }

    public void UpdateHintState(int level, int hint)
    {
        levelStates[level].UpdateHints(hint);
    }

    //called via appQuit, updates database
    public void UpdateLevelStateDB()
    {
        if (dbManager == null)
            return;

        for (int a = 1; a <= levelStates.Count; a++)
        {
            if (levelStates[a].StateChanged())
            {
                dbManager.Execute("UPDATE ZenLevels SET LevelState = ?, HintsUnlocked = ? WHERE LevelNumber = ? ", levelStates[a].GetState(), levelStates[a].GetHints(), a);
                levelStates[a].UnFlag();
            }
        }
    }

    void Update()
    {
        if(dbManager == null)
            if (GetComponent<SimpleSQL.SimpleSQLManager>())
                dbManager = GetComponent<SimpleSQL.SimpleSQLManager>();

        if(dbManager != null && tileFades == null)
        {
            GenTileFades(); //generates random colors for tile aesthetics.
            ReadLevelStates();
        }

        if(dbManager != null && currentStars == -1)
        {
            LoadStartingData();
        }

        if (playingIntro && !source.isPlaying)
        {
            playingIntro = false;
            source.clip = Music[1];
            source.loop = true;
            source.Play();
        }

        if(giveHint)
        {
            AddHint();
            giveHint = false;
        }

        if (lastAdd > 8)
            ShowRewardedVideo();
        
        if(unlockState == true)
        {
            unlockState = false;

            this.UpdateState(this.currentLevel + 1, 1);
            this.NextLevel();
        }
    }

    public void ToggleMute()
    {
        source.mute = !source.mute;
    }

    //Level State Objects are used to keep track of the data we modify as the player beats levels.
    //OnApplicationQuit We Cycle through our Dictionary of LevelStateInfos and Update our database
    //with anything that's been flagged. 

    //0: incomplete, inaccessable
    //1: incomplete, accessable
    //2,3,4: bronze, silver, gold.
    public class LevelStateInfo
    {
        private int level, initialState, finalState, hintsUnlocked, finalHintsUnlocked;
        private bool changed;

        public LevelStateInfo(int level, int initialState, int hintsUnlocked)
        {
            this.level = level;
            this.initialState = this.finalState = initialState;
            this.hintsUnlocked = this.finalHintsUnlocked = hintsUnlocked;
            changed = false;
        }

        public void UnFlag()
        {
            initialState = finalState;
            changed = false;
        }

        public void UpdateHints(int hint)
        {
            if(hint > hintsUnlocked)
            {
                this.finalHintsUnlocked = this.hintsUnlocked = hint;
                changed = true;
            }
            
        }

        public void UpdateState(int state)
        {
            if(state >= this.finalState)
                this.finalState = state;

            changed = (this.finalState != this.initialState);
        }

        public int GetState()
        {
            return finalState;
        }

        public int GetHints()
        {
            return finalHintsUnlocked;
        }

        public bool IsUnlocked()
        {
            return finalState != 0;
        }

        public bool StateChanged()
        {
            return changed;
        }
    }

}
