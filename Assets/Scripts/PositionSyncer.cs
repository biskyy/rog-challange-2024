using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSyncer : MonoBehaviour
{
    public Transform cam;
    public Transform cameraPosition;
    public Transform orientation;
    public Transform yOrientation;
    public Transform orientationPosition;
    public Transform feet;
    public Transform feetPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cam)
            cam.position = cameraPosition.position;
        if (orientation)
            orientation.position = orientationPosition.position;
        if (yOrientation)
            yOrientation.position = orientationPosition.position;
        if (feet)
            feet.position = feetPosition.position;
    }
}
