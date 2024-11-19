using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
  public float maxHealth = 100f;
  public float health = 100f;

  public bool parrying = false;

  public bool grounded;
  public bool jumping;
  public bool crouching;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void HandleInput()
  {
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");

    moveDirection = yOrientation.forward * verticalInput + yOrientation.right * horizontalInput;
    slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);

    // handle jumping
    jumping = Input.GetKey(KeyCode.Space);

    if (Input.GetKeyDown(KeyCode.Space) && grounded)
      Jump();

    // handle crouching
    crouching = Input.GetKey(KeyCode.LeftShift);

    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      StartCrouch();
    }
    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
      StopCrouch();
    }

    if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.R))
      transform.position = new Vector3(0, 1, 0);
  }

}
