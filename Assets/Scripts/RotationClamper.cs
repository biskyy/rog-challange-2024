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



  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (x)
      transform.localRotation = Quaternion.Euler(Mathf.Clamp(transform.rotation.x, minClamp, maxClamp), transform.rotation.y, transform.rotation.z);
    else if (y)
      transform.localRotation = Quaternion.Euler(transform.rotation.x, Mathf.Clamp(transform.rotation.y, minClamp, maxClamp), transform.rotation.z);
    else if (z)
      transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Mathf.Clamp(transform.rotation.z, minClamp, maxClamp));
  }
}
