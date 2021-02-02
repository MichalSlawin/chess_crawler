using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();
        if(cameraController == null)
        {
            throw new System.Exception("Camera controller not found.");
        }
    }

    public void ChangeCameraPosition()
    {
        cameraController.ChangeCameraPosition();
    }
}
