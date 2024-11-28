using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverKatana : MonoBehaviour
{
  public Player player;
  public GameObject bladeCollider;
  public bool playerTouched = false;

  public float damage = 20f;

  [Header("Other")]
  public float knockback = 750f;
  private Rigidbody rb;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
  }

  public void TurnIntoRagdoll()
  {
    if (transform.parent != null)
    {
      rb.isKinematic = false;
      Destroy(transform.parent.parent.GetComponent<Animator>());
      transform.SetParent(null, true);
    }
    bladeCollider.SetActive(false);
  }

  public void DestroyKatana()
  {
    Destroy(gameObject);
  }

}
