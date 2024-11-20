using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckKatanaTouchedPlayer : MonoBehaviour
{
  public SilverKatana silverKatana;
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
      silverKatana.player = collider.GetComponentInParent<Player>();
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.tag == "player")
    {
      silverKatana.playerTouched = false;
    }
  }
}
