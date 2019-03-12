using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaster : MonoBehaviour {
    private GameData data;
	// Use this for initialization
	void Start () {
        data = GameObject.Find("GameData").GetComponent<GameData>();
    }
	
    public void TryToUnlockHint()
    {
        data.TryToUnlockHint();
    }

    public void Reset()
    {
        data.LoadLevel();
        data.lastAdd++;
    }

    public void NextLevel()
    {
        if (data.IsUnlocked(data.currentLevel + 1))
            data.NextLevel();
        else
            data.PromptNextLevelAd();
    }

    public void SupportKirkAndSam()
    {
        data.WatchUnlockAd();
    }
    public void ResumeFromAd()
    {
        data.PromptNextLevelAd();
    }

    public void NextLevel(int i)
    {
        data.NextLevel();
        data.lastAdd++;
    }

    public void PrevLevel()
    {
        data.PrevLevel();
    }

    public void Resume()
    {
        data.ToggleLevelSelect();
    }

    public void Hint()
    {
        data.ToggleHints();
    }

    public void LevelSelect()
    {
        data.LoadLevelSelector();
    }
}
