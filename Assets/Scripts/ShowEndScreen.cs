using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEndScreen : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "player") {
      Globals.Instance.victory = true;
      LevelManager.Instance.GoToScene("EndScreen");
    }
  }
}
