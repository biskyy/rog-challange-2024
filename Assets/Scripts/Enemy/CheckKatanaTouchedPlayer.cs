using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckKatanaTouchedPlayer : MonoBehaviour
{
  public EnemyAI owner;
  public SilverKatana silverKatana;
  public Player player;

  // Start is called before the first frame update
  void Start()
  {
    silverKatana = GetComponentInParent<SilverKatana>();
    owner = GetComponentInParent<EnemyAI>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter(Collider collider)
  {
    if (collider.tag == "player")
    {
      silverKatana.playerTouched = true;
      player = collider.GetComponentInParent<Player>();
      silverKatana.player = player;
    }
  }

  void OnTriggerStay(Collider collider)
  {
    if (collider.tag == "player")
    {
      if (!player.canTakeDamage)
      {
        owner.enemyKatanaAnimator.speed = 0f;
        owner.GetStunned();
      }
    }
  }


  void OnTriggerExit(Collider collider)
  {
    if (collider.tag == "player")
    {
      player.TakeDamage(silverKatana.damage);
      silverKatana.playerTouched = false;
    }

  }
}
