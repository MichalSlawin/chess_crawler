using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public bool changeX = false;

    public const float MOVEMENT_SPEED_MAX = 200f;
    public const float SCROLL_SPEED_MAX = 1000f;

    private static float movementSpeed = 20f;
    private static float scrollSpeed = 100f;
    private static float mouseSensitivityX = 4.0f;
    private static float mouseSensitivityY = 4.0f;

    private Vector3 cameraStartPosition;
    private GameObject whitePieceObj; // player controllable
    private Vector3 initialOffset; // beetween camera and white piece
    private Vector3 initialOffsetOriginal;
    private bool lockedMode = true;
    private float smoothness = 1f;
    private bool cameraChanged = false;

    public static float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public static float ScrollSpeed { get => scrollSpeed; set => scrollSpeed = value; }
    public static float MouseSensitivityX { get => mouseSensitivityX; set => mouseSensitivityX = value; }
    public static float MouseSensitivityY { get => mouseSensitivityY; set => mouseSensitivityY = value; }

    void Start()
    {
        whitePieceObj = GameObject.FindGameObjectWithTag("PlayerControllable");
        initialOffset = transform.position - whitePieceObj.transform.position;
        initialOffsetOriginal = initialOffset;
        
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        cameraStartPosition = transform.position;
    }

    void Update()
    {
        //HandleRotation();
        //HandleMovement();
        //HandleMouseScroll();
    }

    void FixedUpdate()
    {
        if(whitePieceObj != null && lockedMode)
        {
            Vector3 targetPos = whitePieceObj.transform.position + initialOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothness * Time.fixedDeltaTime);
        }
    }

    public void ChangeCameraPosition()
    {
        if(changeX)
        {
            if (whitePieceObj.transform.position.x < transform.position.x)
            {
                transform.position = new Vector3(transform.position.x - 2 * Mathf.Abs(initialOffsetOriginal.x), transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + 2 * Mathf.Abs(initialOffsetOriginal.x), transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (whitePieceObj.transform.position.z < transform.position.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2 * Mathf.Abs(initialOffsetOriginal.z));
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2 * Mathf.Abs(initialOffsetOriginal.z));
            }
        }
        
        transform.Rotate(0, 180, 0, Space.World);

        initialOffset = transform.position - whitePieceObj.transform.position;
        cameraChanged = !cameraChanged;
    }

    private void HandleMouseScroll()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            transform.Translate(0f, 0f, Input.mouseScrollDelta.y * ScrollSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        // rotation        
        if (Input.GetMouseButton(1))
        {
            float y = Input.GetAxis("Mouse X") * MouseSensitivityX;
            float x = Input.GetAxis("Mouse Y") * MouseSensitivityY;
            transform.eulerAngles = transform.eulerAngles - new Vector3(x, y * -1, 0);
        }
    }

    private void HandleMovement()
    {
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");

        // Camera position reset
        if (Input.GetKeyDown(KeyCode.U))
        {
            gameObject.transform.localPosition = cameraStartPosition;
        }
        // Moving camera in x and z axis
        if(xAxisValue != 0 || zAxisValue != 0)
        {
            transform.Translate(xAxisValue * MovementSpeed * Time.deltaTime, 0.0f, zAxisValue * MovementSpeed * Time.deltaTime);
        }
        // Moving camera up
        if(Input.GetKey(KeyCode.E))
        {
            transform.position += transform.up * MovementSpeed * Time.deltaTime;
        }
        // Moving camera down
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += -transform.up * MovementSpeed * Time.deltaTime;
        }

        // Locking camera
        if (Input.GetKeyDown(KeyCode.Space))
        {
            initialOffset = transform.position - whitePieceObj.transform.position;
            lockedMode = !lockedMode;
        }
    }
}
