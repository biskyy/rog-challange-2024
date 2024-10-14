using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player Properties")]
    public float playerScale = 1f;
    public float playerDrag = 12f;
    public float dragSmoothMultiplier = 10f;


    [Header("Camera")]
    public Transform orientation;

    [Header("Movement")]
    public float speed = 100f;

    private Rigidbody rb;

    private bool crouching;

    float horizontalInput, verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleDrag();
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        crouching = Input.GetKey(KeyCode.LeftShift);

        // Handle crouching
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.drag = playerDrag / 2;
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopCrouch();
        }
    }

    void HandleMovement()
    {
        rb.AddForce(orientation.transform.forward * verticalInput * speed * Time.deltaTime);
        rb.AddForce(orientation.transform.right * horizontalInput * speed * Time.deltaTime);
    }

    void HandleDrag()
    {
        if (crouching)
        {
            rb.drag = Mathf.MoveTowards(rb.drag, 18f, Time.deltaTime * dragSmoothMultiplier);
        }
        else
        {
            rb.drag = playerDrag;
        }
    }

    void StartCrouch()
    {
        transform.localScale = new Vector3(1f, playerScale / 2, 1f);
    }

    void StopCrouch()
    {
        transform.localScale = new Vector3(1f, playerScale, 1f);
    }
}
