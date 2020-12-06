using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static Scene loadedScene;

    public static Scene LoadedScene { get => loadedScene; set => loadedScene = value; }

    private void Start()
    {
        LoadedScene = SceneManager.GetActiveScene();
    }
}
