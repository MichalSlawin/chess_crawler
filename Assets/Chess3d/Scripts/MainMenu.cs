using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private static Slider cameraSlider;
    private static Slider musicSlider;

    void Start()
    {
        GetFileData();
    }

    public void InitOptions()
    {
        InitCameraSlider();
        InitMusicSlider();
    }

    private void GetFileData()
    {
        GameData gameData = FileHandler.LoadFile();
        if(gameData != null)
        {
            if(FileHandler.GameData.GetLevelsWithStar() == null)
            {
                FileHandler.GameData.InitLevelsWithStar();
            }
            ChangeCameraSpeed(FileHandler.GameData.CameraSpeed);
            ChangeMusicVolume(FileHandler.GameData.MusicVolume);
        }
    }

    public void InitLevelsAvailability()
    {
        GameObject levels = GameObject.FindGameObjectWithTag("Levels");
        LevelButton[] levelButtons = levels.GetComponentsInChildren<LevelButton>();

        foreach (LevelButton button in levelButtons)
        {
            if(button.levelNumber > FileHandler.GameData.UnlockedLevelNum)
            {
                button.gameObject.SetActive(false);
            }
            else if(!FileHandler.GameData.GetLevelsWithStar().Contains(button.levelNumber))
            {
                button.StarEarned = false;
                Transform star = button.gameObject.transform.Find("Star");
                star.gameObject.SetActive(false);
            }
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

    private void InitMusicSlider()
    {
        GameObject slider = GameObject.Find("MusicSlider");
        if(slider != null)
        {
            musicSlider = slider.GetComponent<Slider>();
            if(musicSlider != null)
            {
                musicSlider.normalizedValue = AudioController.Volume;
            }
        }
    }

    public void PlayGame(string levelName)
    {
        GameObject audio = GameObject.FindGameObjectWithTag("Music");
        Destroy(audio);
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

        FileHandler.GameData.CameraSpeed = value;
        FileHandler.SaveFile();
    }

    public void ChangeMusicVolume(float value)
    {
        AudioController.Volume = value;

        FileHandler.GameData.MusicVolume = value;
        FileHandler.SaveFile();
    }
}
