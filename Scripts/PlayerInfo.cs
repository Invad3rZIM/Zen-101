using SimpleSQL;

public class PlayerInfo {
    [PrimaryKey]
    public int ID { get; set; }

    public int CurrentLevel { get; set; }

    public int CurrentStars { get; set; }

    public int BlockAds { get; set; }

    public int AdCount { get; set; }
}
