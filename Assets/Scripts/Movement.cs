using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
  [Header("Player Properties")]
  public Vector3 playerScale = new Vector3(1, 1, 1);
  public float playerDrag = 14f;
  public float crouchDrag = 20f;
  public float airDrag = 7f;
  public float dragSmoothMultiplier = 8000f;

  public Transform body;
  private Rigidbody rb;

  [Header("Camera")]
  public Transform orientation;

  public Transform yOrientation;

  [Header("Movement")]
  public Vector3 moveDirection;
  public float speed;
  public float moveSpeed = 8000f;

  public float fallMultiplier = 3000f;
  public float airMultiplier = 0.52f;
  public float crouchAirMultiplier = 0.2f;

  [Header("Jump")]
  public float jumpForce = 1000f;
  public float crouchJumpMultiplier = 0.65f;

  [Header("Crouch")]
  public Vector3 crouchScale = new Vector3(1, 0.7f, 1);
  public float crouchTopSpeed = 13000f;
  public float crouchSpeed = 5000f;

  [Header("Others")]
  public Transform feet;
  public Vector3 groundCheckBox = new Vector3(0.1f, 0.1f, 0.1f);
  public LayerMask groundLayer;

  public bool grounded;
  public bool jumping;
  public bool crouching;

  private float horizontalInput, verticalInput;
  public Vector3 slopeMoveDirection;
  private Vector3 groundNormal;

  // s tart is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;

    speed = moveSpeed;
  }

  // update is called once per frame
  void Update()
  {
    HandleInput();
    HandleSpeedAndDrag();
    HandleSlopes();

    grounded = Physics.CheckBox(feet.position, groundCheckBox, Quaternion.Euler(Vector3.down), groundLayer);
    AdvancedGizmosVisualizer.DisplayBox(feet.position, groundCheckBox, Quaternion.Euler(Vector3.down)); // draw gizmos for ground check
  }

  void LateUpdate()
  {
  }

  void FixedUpdate()
  {
    HandleMovement();
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

  void HandleMovement()
  {
    if (rb.velocity.y < 0 || GetSlopeMovementDirection() < 0)
      rb.AddForce(Vector3.down * fallMultiplier * Time.deltaTime);

    if (grounded)
    {
      rb.AddForce(slopeMoveDirection * speed * Time.deltaTime);
    }
    else if (crouching && !grounded)
    {
      rb.AddForce(slopeMoveDirection * speed * crouchAirMultiplier * Time.deltaTime);
    }
    else
    {
      rb.AddForce(slopeMoveDirection * speed * airMultiplier * Time.deltaTime);
    }
  }

  void HandleSpeedAndDrag()
  {
    if (!grounded)
    {
      speed = Mathf.MoveTowards(speed, moveSpeed, Time.deltaTime * dragSmoothMultiplier);
      rb.drag = airDrag;
    }
    else if (crouching)
    {
      if (!IsOnSlope())
        speed = Mathf.MoveTowards(speed, crouchSpeed, Time.deltaTime * dragSmoothMultiplier);
      else
      {
        if (slopeMoveDirection != Vector3.zero && GetSlopeMovementDirection() < 0) // make sure we are sliding down and not up
          speed = Mathf.MoveTowards(speed, speed + 1000f, Time.deltaTime * dragSmoothMultiplier);
        else
        {
          speed = Mathf.MoveTowards(speed, crouchSpeed, Time.deltaTime * dragSmoothMultiplier);
        }
      }
      if (grounded) // if player crouches mid-air, reset the drag once he touches the ground
        rb.drag = playerDrag;
    }
    else
    {
      speed = Mathf.MoveTowards(speed, moveSpeed, Time.deltaTime * dragSmoothMultiplier);
      rb.drag = playerDrag;
    }
  }

  void Jump()
  {
    if (crouching)
      rb.AddForce(rb.transform.up * jumpForce * crouchJumpMultiplier * Time.fixedDeltaTime, ForceMode.Impulse);
    else
      rb.AddForce(rb.transform.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
  }

  void StartCrouch()
  {
    // this drag implementation has a logic flaw: you can gain momentum in any direction but you should only gain forward momentum
    //if (rb.drag >= playerDrag)
    //    rb.drag = playerDrag / 1.5f; // TODO: apply this only if grounded

    if (speed <= moveSpeed && GetSlopeMovementDirection() <= 0) // prevent spamming (not good detection, must be reworked)
      speed = crouchTopSpeed;

    // another implementation would be this, however for some reason whatever slide force i give
    // it, it will always slide for the same amount of time
    //if (rb.velocity.magnitude > 0.5f)
    //    rb.AddForce(orientation.transform.forward * slideForce, ForceMode.Force);

    body.localScale = crouchScale;
    transform.position = new Vector3(transform.position.x, transform.position.y - (playerScale.y - crouchScale.y), transform.position.z);

  }

  void StopCrouch()
  {
    body.localScale = playerScale;
    transform.position = new Vector3(transform.position.x, transform.position.y + (playerScale.y - crouchScale.y), transform.position.z);
  }

  private RaycastHit slopeHit;

  void HandleSlopes()
  {
    Physics.Raycast(feet.position, Vector3.down, out slopeHit, 0.3f);
    groundNormal = slopeHit.normal;
    print(groundNormal);
  }

  bool IsOnSlope()
  {
    if (groundNormal != Vector3.up)
      return true;
    else
      return false;
  }

  float GetSlopeMovementDirection()
  {
    // calculate the movement direction on the slope
    Vector3 projectedMovement = Vector3.ProjectOnPlane(moveDirection, groundNormal);

    // return the dot product between the movement direction and the global up vector
    return Vector3.Dot(projectedMovement, Vector3.up);
  }

}
