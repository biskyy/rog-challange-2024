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
    public float dragSmoothMultiplier = 8f;
    private Rigidbody rb;

    [Header("Camera")]
    public Transform orientation;

    [Header("Movement")]
    public float speed;
    public float moveSpeed = 8000f;
    public float fallMultiplier = 3000f;

    [Header("Jump")]
    public float jumpForce = 1000f;
    public float airMultiplier = 0.48f;
    public float crouchJumpMultiplier = 1.4f;

    [Header("Crouch")]
    public float crouchTopSpeed = 13000f;
    public float crouchSpeed = 5000f;

    [Header("Others")]
    public Transform feet;
    public float groundHitDistance = 0.11f;
    public LayerMask groundLayer;
    public TextMeshProUGUI velocityText;
    public float velocityUpdateTimeWindow = 1f;

    public bool grounded;
    public bool jumping;
    public bool crouching;

    float horizontalInput, verticalInput;

    private Queue<Vector3> positions = new Queue<Vector3>();
    private Queue<float> timestamps = new Queue<float>();

    // Start is called before the first frame update
    void Start() {
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;

        speed = moveSpeed;
    }

    // Update is called once per frame
    void Update() {
        HandleInput();
        HandleDrag();

        CalculateAverageVelocity();
    }

    void FixedUpdate() {
        HandleMovement();
    }

    void CalculateAverageVelocity() { // made by chatgpt 4o
        // Record the current position and time
        positions.Enqueue(rb.position);
        timestamps.Enqueue(Time.time);

        // Remove old data points that are outside the time window
        while (timestamps.Count > 0 && Time.time - timestamps.Peek() > velocityUpdateTimeWindow) {
            positions.Dequeue();
            timestamps.Dequeue();
        }

        // Calculate the displacement over the time window
        if (positions.Count > 1) {
            Vector3 displacement = rb.position - positions.Peek();
            float timeElapsed = Time.time - timestamps.Peek();

            // Calculate average velocity
            Vector3 averageVelocity = displacement / timeElapsed;

            // Debug log to see the result
            velocityText.text = ((int) averageVelocity.magnitude).ToString();
        }
    }

    void HandleInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        RaycastHit groundHit;
        grounded = Physics.Raycast(feet.position, feet.transform.TransformDirection(Vector3.down), out groundHit, groundHitDistance, groundLayer);
        Debug.DrawRay(feet.position, feet.TransformDirection(Vector3.down) * groundHit.distance, Color.yellow);

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

        if (grounded) {
            rb.AddForce(orientation.transform.forward * verticalInput * speed * Time.deltaTime);
            rb.AddForce(orientation.transform.right * horizontalInput * speed * Time.deltaTime);
        } else {
            rb.AddForce(orientation.transform.forward * verticalInput * speed * airMultiplier * Time.deltaTime);
            rb.AddForce(orientation.transform.right * horizontalInput * speed * airMultiplier * Time.deltaTime);
        }
    }

    void HandleDrag() {
        if (!grounded && !crouching) {
            speed = Mathf.MoveTowards(speed, moveSpeed, Time.deltaTime * dragSmoothMultiplier);
            rb.drag = airDrag;
        }
        else if (crouching) {
            speed = Mathf.MoveTowards(speed, crouchSpeed, Time.deltaTime * dragSmoothMultiplier);
        }
        else {
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
}
