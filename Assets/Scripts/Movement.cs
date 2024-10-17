using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player Properties")]
    public float playerScale = 1f;
    public float playerDrag = 12f;
    public float crouchDrag = 24f;
    public float dragSmoothMultiplier = 10f;


    [Header("Camera")]
    public Transform orientation;

    [Header("Movement")]
    public float speed = 100f;

    [Header("Crouch")]
    public float slideForce = 1000f;

    [Header("Others")]
    public TextMeshProUGUI velocityText;

    private Rigidbody rb;

    private bool crouching;

    float horizontalInput, verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        velocityText.text = ((int)rb.velocity.magnitude).ToString();
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
        rb.AddForce(orientation.transform.forward * verticalInput * speed * Time.deltaTime);
        rb.AddForce(orientation.transform.right * horizontalInput * speed * Time.deltaTime);
    }

    void HandleDrag()
    {
        if (crouching)
        {
            rb.drag = Mathf.MoveTowards(rb.drag, crouchDrag, Time.deltaTime * dragSmoothMultiplier);
        }
        else
        {
            rb.drag = Mathf.MoveTowards(rb.drag, playerDrag, Time.deltaTime * dragSmoothMultiplier);
        }
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

        transform.localScale = new Vector3(1f, playerScale / 2, 1f);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);  
    }

    void StopCrouch()
    {
        transform.localScale = new Vector3(1f, playerScale, 1f);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }
}
