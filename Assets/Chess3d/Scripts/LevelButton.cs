using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    private bool starEarned = true;

    public bool StarEarned { get => starEarned; set => starEarned = value; }
}
