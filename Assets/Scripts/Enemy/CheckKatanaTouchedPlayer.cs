using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckKatanaTouchedPlayer : MonoBehaviour
{
  public SilverKatana silverKatana;
  public Player player;
  public bool playerDidAnythingToStopAttack = false;
  public bool playerParried = false;

  // Start is called before the first frame update
  void Start()
  {
    silverKatana = GetComponentInParent<SilverKatana>();
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

      if (player.blocking)
        playerDidAnythingToStopAttack = true;
      if (player.redKatana.parried)
        playerParried = true;
      print(collider);
    }
  }

  void OnTriggerStay(Collider collider)
  {
    if (collider.tag == "player")
    {
      if (player.blocking)
        playerDidAnythingToStopAttack = true;
      if (player.redKatana.parried)
        playerParried = true;
    }
  }


  void OnTriggerExit(Collider collider)
  {
    if (collider.tag == "player")
    {

      if (player.blocking)
        playerDidAnythingToStopAttack = true;
      if (player.redKatana.parried)
        playerParried = true;

      // do nothing
      if (playerParried) { }
      else if (playerDidAnythingToStopAttack)
        player.TakeDamage(silverKatana.damage / 2f);
      else
        player.TakeDamage(silverKatana.damage);

      playerDidAnythingToStopAttack = false;
      playerParried = false;
      silverKatana.playerTouched = false;
    }

  }
}
