using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSyncer : MonoBehaviour
{
  public Transform cam;
  public Transform cameraPosition;
  public Transform orientation;
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
    cam.position = cameraPosition.position;
    orientation.position = orientationPosition.position;
    feet.position = feetPosition.position;
  }
}
