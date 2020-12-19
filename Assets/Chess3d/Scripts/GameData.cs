using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private float cameraSpeed;
    private int unlockedLevelNum;

    public float CameraSpeed { get => cameraSpeed; set => cameraSpeed = value; }
    public int UnlockedLevelNum { get => unlockedLevelNum; set => unlockedLevelNum = value; }
}
