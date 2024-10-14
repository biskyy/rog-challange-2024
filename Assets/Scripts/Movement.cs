using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Camera")]
    public Transform orientation;

    [Header("Movement")]
    public float speed = 100f;

    private Rigidbody rb;

    float x, y;

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
    }

    void HandleInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }

    void HandleMovement()
    {
        rb.AddForce(orientation.transform.forward * y * speed * Time.deltaTime);
        rb.AddForce(orientation.transform.right * x * speed * Time.deltaTime);
    }
}
