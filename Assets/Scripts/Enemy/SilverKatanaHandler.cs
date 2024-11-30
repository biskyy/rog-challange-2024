using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverKatanaHandler : MonoBehaviour
{
  private CheckKatanaTouchedPlayer bladeCollider;

  private void Start()
  {
    bladeCollider = GetComponentInChildren<CheckKatanaTouchedPlayer>();
  }

  public void EnableBladeCollider()
  {
    bladeCollider.EnableCollider();
  }

  public void DisableBladeCollider()
  {
    bladeCollider.DisableCollider();
  }
}
