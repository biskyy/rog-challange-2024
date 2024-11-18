using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyKatanaCollider : MonoBehaviour
{

  public RedKatana redKatana;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "EnemyWeapon")
    {
      if (redKatana.animator.GetBool("parrying"))
        redKatana.enemyKatanaTouched = true;
    }
  }
}
