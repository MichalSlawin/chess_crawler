using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class FileHandler
{
    private const string filename = "/gameData.dat";
    private static GameData gameData = new GameData();

    public static GameData GameData { get => gameData; set => gameData = value; }

    public static void SaveFile()
    {
        string destination = Application.persistentDataPath + filename;
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, gameData);
        file.Close();
    }

    public static GameData LoadFile()
    {
        string destination = Application.persistentDataPath + filename;
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        gameData = (GameData)bf.Deserialize(file);
        file.Close();

        return gameData;
    }
}
