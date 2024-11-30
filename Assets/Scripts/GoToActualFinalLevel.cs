using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToActualFinalLevel : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "player")
    {
      LevelManager.Instance.GoToScene("Level4");
    }
  }
}
