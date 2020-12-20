using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private float cameraSpeed = 0.2f;
    private float musicVolume = 0.5f;
    private int unlockedLevelNum = 0;

    public float CameraSpeed { get => cameraSpeed; set => cameraSpeed = value; }
    public int UnlockedLevelNum { get => unlockedLevelNum; set => unlockedLevelNum = value; }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
}
