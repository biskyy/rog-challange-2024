using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth = 5f;
    [SerializeField] private float swayMultiplier = 1f;

    [Header("Orientation Settings")]
    [SerializeField] private bool usePlayerInput = false; // Determines if the sway uses player input
    [SerializeField] private Transform orientationReference; // External orientation for non-player objects

    private Vector3 lastOrientationForward; // Tracks the last forward vector of the reference

    void Start()
    {
        if (orientationReference != null)
        {
            lastOrientationForward = orientationReference.forward;
        }
    }

    void Update()
    {
        float mouseX = 0f;
        float mouseY = 0f;

        if (usePlayerInput)
        {
            // Use player mouse input
            mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
        }
        else if (orientationReference != null)
        {
            // Calculate the change in orientation
            Vector3 currentOrientationForward = orientationReference.forward;

            // Compute the difference (delta) between the last and current orientation
            Vector3 orientationDelta = currentOrientationForward - lastOrientationForward;

            // Map the delta to sway effect
            mouseX = orientationDelta.x * swayMultiplier;
            mouseY = orientationDelta.y * swayMultiplier;

            // Update the last orientation
            lastOrientationForward = currentOrientationForward;
        }

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
