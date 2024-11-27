using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKatanaTouchedEnemy : MonoBehaviour
{
  private EnemyAI enemy;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.tag == "enemy")
    {
      enemy = collider.GetComponentInParent<EnemyAI>();
      if (enemy != null)
      {
        Vector3 collisionPoint = collider.ClosestPoint(transform.position);
        enemy.bloodVFX.transform.position = collisionPoint;
        enemy.bloodVFX.transform.rotation = Quaternion.LookRotation(transform.position - enemy.transform.position);

        enemy.bloodVFX.Play();
      }
    }
  }

  private void OnTriggerExit(Collider collider)
  {
    if (collider.tag == "enemy")
    {
      print(collider.name);
      if (enemy != null)
      {
        enemy.TakeDamage(20f);
      }
    }
  }
}
