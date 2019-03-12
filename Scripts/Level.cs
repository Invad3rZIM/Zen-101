using SimpleSQL;

public class Level
{
    //Holds Our Database information about our levels.
    [PrimaryKey]
    public int LevelNumber { get; set; }

    public string LevelDescription { get; set; }

    public string LevelSerialization { get; set; }

    public int LevelState { get; set;  }

    public int LevelBestMoves { get; set; }

    public int LevelPersonalMoves { get; set; }

    public string LevelHint1 { get; set; }
    public string LevelHint2 { get; set; }
    public string LevelHint3 { get; set; }
    public int HintsUnlocked { get; set; }
    public int HintsTotal { get; set; }

}
