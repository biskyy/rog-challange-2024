using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
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
    public float speed = 100f;
    public float jumpForce = 1000f;

    [Header("Others")]
    public Transform feet;
    public float groundHitDistance = 0.11f;
    public LayerMask groundLayer;
    public TextMeshProUGUI velocityText;

    public bool grounded;
    public bool jumping;
    public bool crouching;

    float horizontalInput, verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleDrag();
        
        velocityText.text = ((int)rb.velocity.magnitude).ToString();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
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
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopCrouch();
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.R))
            rb.position = new Vector3(0, 1, 0);
    }

    void HandleMovement()
    {

        rb.AddForce(orientation.transform.forward * verticalInput * speed * Time.deltaTime);
        rb.AddForce(orientation.transform.right * horizontalInput * speed * Time.deltaTime);
    }

    void HandleDrag()
    {
        if (!grounded || jumping)
            rb.drag = airDrag;
        if (crouching)
        {
            rb.drag = Mathf.MoveTowards(rb.drag, crouchDrag, Time.deltaTime * dragSmoothMultiplier);
        }
        else
        {
            rb.drag = Mathf.MoveTowards(rb.drag, playerDrag, Time.deltaTime * dragSmoothMultiplier);
        }
    }

    void Jump()
    {
        rb.AddForce(rb.transform.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    void StartCrouch()
    {
        // this drag implementation has a logic flaw: you can gain momentum in any direction but you should only gain forward momentum
        if (rb.drag >= playerDrag)
            rb.drag = playerDrag / 1.5f; // TODO: apply this only if grounded

        // another implementation would be this, however for some reason whatever slide force i give
        // it, it will always slide for the same amount of time
        //if (rb.velocity.magnitude > 0.5f)
        //    rb.AddForce(orientation.transform.forward * slideForce, ForceMode.Force);

        transform.localScale = new Vector3(playerScale.x, playerScale.y / 2, playerScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);  
    }

    void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }
}
