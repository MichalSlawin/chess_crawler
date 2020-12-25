using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private float cameraSpeed = 0.2f;
    private float musicVolume = 0.5f;
    private int unlockedLevelNum = 0;
    private List<int> levelsWithStar = new List<int>();

    public float CameraSpeed { get => cameraSpeed; set => cameraSpeed = value; }

    public int UnlockedLevelNum
    {
        get => unlockedLevelNum;
        set
        {
            if(value > unlockedLevelNum)
                unlockedLevelNum = value;
        }
    }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    
    public List<int> GetLevelsWithStar()
    {
        return levelsWithStar;
    }

    public void AddLevelWithStar(int levelNum)
    {
        levelsWithStar.Add(levelNum);
    }

    public void InitLevelsWithStar()
    {
        levelsWithStar = new List<int>();
    }
}
