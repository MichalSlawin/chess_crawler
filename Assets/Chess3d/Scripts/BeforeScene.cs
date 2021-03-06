﻿using UnityEngine;

class BeforeScene
{

    private static int maxFps = 60;

    public static int MaxFps { get => maxFps; set => maxFps = value; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Application.targetFrameRate = MaxFps;
    }

}
