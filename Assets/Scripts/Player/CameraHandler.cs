using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
  [Header("Camera")]
  public Camera cam;
  public Transform orientation;
  public Transform yOrientation;
  public float sensitivityMultiplier = 1.0f;
  public float sensitivity = 100f;

  public PauseMenu pauseMenu;

  // Start is called before the first frame update
  void Start()
  {
    cam = GetComponentInChildren<Camera>();
  }

  private void LockCursor()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  private void UnlockCursor()
  {
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true; ;
  }

  private float yRotation; // horizontal
  private float xRotation; // vertical

  // update is called once per frame
  void Update()
  {
    if (pauseMenu.gameIsPaused)
    {
      UnlockCursor();
    }
    else
    {
      LockCursor();
    }

    if (pauseMenu.gameOver)
    {
      UnlockCursor();
    }
    if (!pauseMenu.gameIsPaused && !pauseMenu.gameOver) HandleInput();
    sensitivityMultiplier = Globals.Instance.sensitivityMultiplier;
  }

  private void HandleInput()
  {
    float mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * sensitivity * sensitivityMultiplier;
    float mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * sensitivity * sensitivityMultiplier;

    yRotation += mouseX;
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0f);
    orientation.eulerAngles = new Vector3(xRotation, yRotation, 0f);
    yOrientation.eulerAngles = new Vector3(0f, yRotation, 0f);
  }
}
