using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 10f;

    public float mouseSensitivityX = 4.0f;
    public float mouseSensitivityY = 4.0f;

    float rotY = 0.0f;

    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        // rotation        
        if (Input.GetMouseButton(1))
        {
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
            rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
            transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
        }
    }

    private void HandleMovement()
    {
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");

        // Camera position reset
        if (Input.GetKey(KeyCode.U))
        {
            gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
        // Moving camera in x and z axis
        if(xAxisValue != 0 || zAxisValue != 0)
        {
            transform.Translate(xAxisValue * movementSpeed * Time.deltaTime, 0.0f, zAxisValue * movementSpeed * Time.deltaTime);
        }
        // Moving camera up
        if(Input.GetKey(KeyCode.E))
        {
            transform.position += transform.up * movementSpeed * Time.deltaTime;
        }
        // Moving camera down
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += -transform.up * movementSpeed * Time.deltaTime;
        }
    }
}
