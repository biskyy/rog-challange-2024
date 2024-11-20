using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyKatanaCollider : MonoBehaviour
{

  public RedKatana redKatana;
  public Transform parryPoint;
  // Start is called before the first frame update
  void Start()
  {
    redKatana = GetComponentInParent<RedKatana>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter(Collider collider)
  {
    if (collider.tag == "EnemyWeapon")
    {
      redKatana.enemyKatanaTouched = true;
    }
    //Vector3 collisionPoint = collider.ClosestPointOnBounds(transform.position);
    //parryPoint.position = collisionPoint;
  }
  void OnTriggerExit(Collider collider)
  {
    if (collider.tag == "EnemyWeapon")
    {
      redKatana.enemyKatanaTouched = false;
    }
  }


  //private void OnCollisionEnter(Collision collision)
  //{
  //  if (collision.gameObject.tag == "EnemyWeapon" && redKatana.animator.GetBool("parrying"))
  //    redKatana.enemyKatanaTouched = true;

  //  Vector3 collisionPoint = collision.contacts[0].point;
  //  print(collisionPoint);
  //  parryPoint.position = collisionPoint;
  //}

  //private void OnCollisionExit(Collision collision)
  //{
  //  if (collision.gameObject.tag == "EnemyWeapon" && redKatana.animator.GetBool("parrying"))
  //    redKatana.enemyKatanaTouched = false;
  //}
}
