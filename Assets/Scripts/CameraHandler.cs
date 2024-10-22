using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("Camera")]
    public Camera cam;
    public Transform cameraPosition;
    public Transform orientation;
    public float sensitivity = 100f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = GetComponentInChildren<Camera>();
    }

    private float yRotation; // horizontal
    private float xRotation; // vertical

    // update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;

        float mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0f);
        orientation.eulerAngles = new Vector3(0f, yRotation, 0f);
    }
}
