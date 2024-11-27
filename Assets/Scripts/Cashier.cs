using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cashier : MonoBehaviour
{
  public Transform semnDirectie;
  public TextMeshPro dialog;
  public float radiusCheck = 2f;
  public Player player;
  public LayerMask playerLayer;
  public Transform orientation;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    bool playerNearby = Physics.CheckSphere(transform.position, radiusCheck, playerLayer);
    if (playerNearby)
    {
      semnDirectie.gameObject.SetActive(true);
      dialog.gameObject.SetActive(true);
      orientation.LookAt(player.transform);
    }
    else
    {
      dialog.gameObject.SetActive(false);
      orientation.localRotation = Quaternion.identity;
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(1, 1, 1, 0.5f);
    Gizmos.DrawSphere(transform.position, radiusCheck);
  }
}
