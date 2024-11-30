using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckKatanaTouchedPlayer : MonoBehaviour
{
  public EnemyAI owner;
  public TargetDummy targetDummy;
  public SilverKatana silverKatana;
  public Player player;
  public BoxCollider bladeCollider;

  // Start is called before the first frame update
  void Start()
  {
    silverKatana = GetComponentInParent<SilverKatana>();
    owner = GetComponentInParent<EnemyAI>();
    targetDummy = GetComponentInParent<TargetDummy>();
    bladeCollider = GetComponent<BoxCollider>();
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
        if (owner)
        {
          owner.GetStunned();
        }
        else if (targetDummy)
        {
          targetDummy.GetStunned();
        }
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

  public void EnableCollider ()
  {
    bladeCollider.enabled = true;
  }
  
  public void DisableCollider()
  {
    bladeCollider.enabled = false;
  }
}
