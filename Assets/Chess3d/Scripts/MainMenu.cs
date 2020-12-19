using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private static Slider cameraSlider;

    void Start()
    {
        GetFileData();
    }

    public void InitOptions()
    {
        InitCameraSlider();
    }

    private void GetFileData()
    {
        GameData gameData = FileHandler.LoadFile();
        if(gameData != null)
        {
            ChangeCameraSpeed(gameData.CameraSpeed);
        }
    }

    private void InitCameraSlider()
    {
        GameObject slider = GameObject.Find("CameraSlider");
        if(slider != null)
        {
            cameraSlider = slider.GetComponent<Slider>();
            if(cameraSlider != null)
            {
                cameraSlider.normalizedValue = (CameraController.MovementSpeed / CameraController.MOVEMENT_SPEED_MAX);
            }
        }
    }

    public void PlayGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeCameraSpeed(float value)
    {
        CameraController.ScrollSpeed = 1 + (value * CameraController.SCROLL_SPEED_MAX);
        CameraController.MovementSpeed = 1 + (value * CameraController.MOVEMENT_SPEED_MAX);

        GameData gameData = new GameData();
        gameData.CameraSpeed = value;
        FileHandler.SaveFile(gameData);
    }
}
