using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretCameraScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    public float mouseSensitivity = 2f;
    public float cameraVerticalRotation = 0f;

    public bool lockedCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        // Lock and Hide the Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

    // Update is called once per frame
    void Update()
    {
        if (lockedCursor == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (lockedCursor != true)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        // Collect Mouse Input

        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the Camera around its local X axis
        if (lockedCursor == true)
        {
            cameraVerticalRotation -= inputY;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
            transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

            // Rotate the Player Object and the Camera around its Y axis

            player.Rotate(Vector3.up * inputX);
        }
    }
}
