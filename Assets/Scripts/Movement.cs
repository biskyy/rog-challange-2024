using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour {
    [Header("Player Properties")]
    public Vector3 playerScale = new Vector3(1, 1, 1);
    public float playerDrag = 14f;
    public float crouchDrag = 20f;
    public float airDrag = 7f;
    public float dragSmoothMultiplier = 8000f;
    private Rigidbody rb;

    [Header("Camera")]
    public Transform orientation;

    [Header("Movement")]
    public float speed;
    public float moveSpeed = 8000f;

    public float fallMultiplier = 3000f;
    public float airMultiplier = 0.52f;
    public float crouchAirMultiplier = 0.2f;

    [Header("Jump")]
    public float jumpForce = 1000f;
    public float crouchJumpMultiplier = 0.65f;

    [Header("Crouch")]
    public float crouchTopSpeed = 13000f;
    public float crouchSpeed = 5000f;

    [Header("Others")]
    public Transform feet;
    public Vector3 groundCheckBox = new Vector3(0.1f, 0.1f, 0.1f);
    public LayerMask groundLayer;

    public bool grounded;
    public bool jumping;
    public bool crouching;

    public Vector3 moveDirection;
    private float horizontalInput, verticalInput;
    private Vector3 slopeMoveDirection;
    private Vector3 groundNormal;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;

        speed = moveSpeed;
    }

    // Update is called once per frame
    void Update() {
        HandleInput();
        HandleSpeedAndDrag();
        HandleSlopes();

        grounded = Physics.CheckBox(feet.position, groundCheckBox, Quaternion.Euler(Vector3.down), groundLayer);
        AdvancedGizmosVisualizer.DisplayBox(feet.position, groundCheckBox, Quaternion.Euler(Vector3.down)); // draw gizmos for ground check
    }

    void FixedUpdate() {
        HandleMovement();
    }

    void HandleInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);

        // Handle jumping
        jumping = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            Jump();

        // Handle crouching
        crouching = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            StopCrouch();
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.R))
            rb.position = new Vector3(0, 1, 0);
    }

    void HandleMovement() {
        if (rb.velocity.y < 0)
            rb.AddForce(Vector3.down * fallMultiplier * Time.deltaTime);

        if (grounded && !isOnSlope()) {
            rb.AddForce(moveDirection * speed * Time.deltaTime);
        } else if (grounded && isOnSlope()) {
            rb.AddForce(slopeMoveDirection * speed * Time.deltaTime);
        } else if (crouching && !grounded) {
            rb.AddForce(moveDirection * speed * crouchAirMultiplier * Time.deltaTime);
        } else {
            rb.AddForce(moveDirection * speed * airMultiplier * Time.deltaTime);
        }
    }

    void HandleSpeedAndDrag() {
        if (!grounded) {
            speed = Mathf.MoveTowards(speed, moveSpeed, Time.deltaTime * dragSmoothMultiplier);
            rb.drag = airDrag;
        } else if (crouching) {
            if (!isOnSlope())
                speed = Mathf.MoveTowards(speed, crouchSpeed, Time.deltaTime * dragSmoothMultiplier);
            else
                speed = Mathf.MoveTowards(speed, crouchTopSpeed, Time.deltaTime * dragSmoothMultiplier);
            if (grounded) // if player crouches mid-air, reset the drag once he touches the ground
                rb.drag = playerDrag;
        } else {
            speed = Mathf.MoveTowards(speed, moveSpeed, Time.deltaTime * dragSmoothMultiplier);
            rb.drag = playerDrag;
        }
    }

    void Jump() {
        if (crouching)
            rb.AddForce(rb.transform.up * jumpForce * crouchJumpMultiplier * Time.fixedDeltaTime, ForceMode.Impulse);
        else
            rb.AddForce(rb.transform.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    void StartCrouch() {
        // this drag implementation has a logic flaw: you can gain momentum in any direction but you should only gain forward momentum
        //if (rb.drag >= playerDrag)
        //    rb.drag = playerDrag / 1.5f; // TODO: apply this only if grounded

        if (speed <= moveSpeed) // prevent spamming (not good detection, must be reworked)
            speed = crouchTopSpeed;

        // another implementation would be this, however for some reason whatever slide force i give
        // it, it will always slide for the same amount of time
        //if (rb.velocity.magnitude > 0.5f)
        //    rb.AddForce(orientation.transform.forward * slideForce, ForceMode.Force);

        transform.localScale = new Vector3(playerScale.x, playerScale.y / 2, playerScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private RaycastHit slopeHit;

    void HandleSlopes() {
        Physics.Raycast(feet.position, Vector3.down, out slopeHit, 0.12f);
        groundNormal = slopeHit.normal;
        print(groundNormal);
    }

    bool isOnSlope() {
        if (groundNormal != Vector3.up)
            return true;
        else
            return false;
    }

    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.red;
    //    Gizmos.Draw(feet.position, groundHitDistance);
    //}
}
