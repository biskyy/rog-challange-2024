using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaVFXSFXHandler : MonoBehaviour
{
  public RedKatana katana;
  // Start is called before the first frame update
  void Start()
  {
    katana = GetComponentInChildren<RedKatana>();
  }

  // Update is called once per frame
  void Update()
  {

  }
  public void EnableTrailVFX()
  {
    katana.trailVFX.Play();
  }

  public void DisableTrailVFX()
  {
    katana.trailVFX.Clear();
    katana.trailVFX.Stop();
  }

  public void EnableBladeCollider()
  {
    katana.bladeCollider.enabled = true;
  }

  public void DisableBladeCollider()
  {
    katana.bladeCollider.enabled = false;
  }
}
