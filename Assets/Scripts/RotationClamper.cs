using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationClamper : MonoBehaviour
{
    public float minClamp;
    public float maxClamp;

    public bool x;
    public bool y;
    public bool z;

    // Update is called once per frame
    void Update()
    {
        Vector3 clampedEuler = transform.localEulerAngles;

        // Convert angles greater than 180 to negative values for proper clamping
        clampedEuler.x = NormalizeAngle(clampedEuler.x);
        clampedEuler.y = NormalizeAngle(clampedEuler.y);
        clampedEuler.z = NormalizeAngle(clampedEuler.z);

        // Clamp the axes as specified
        if (x)
            clampedEuler.x = Mathf.Clamp(clampedEuler.x, minClamp, maxClamp);
        if (y)
            clampedEuler.y = Mathf.Clamp(clampedEuler.y, minClamp, maxClamp);
        if (z)
            clampedEuler.z = Mathf.Clamp(clampedEuler.z, minClamp, maxClamp);

        // Apply the clamped rotation
        transform.localEulerAngles = clampedEuler;
    }

    // Normalize angle to range [-180, 180]
    float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;
        return angle;
    }
}
