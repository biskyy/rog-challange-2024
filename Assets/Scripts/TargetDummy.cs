using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
  public LayerMask playerLayer;
  public Transform player;
  public Transform orientation;
  public float radiusCheck = 10f;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    bool playerNearby = Physics.CheckSphere(transform.position, radiusCheck, playerLayer);
    print(playerNearby);
    orientation.LookAt(player);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(1, 1, 1, 0.5f);
    Gizmos.DrawSphere(transform.position, radiusCheck);
  }
}
