using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKatana : MonoBehaviour
{

  public Animator animator;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
      animator.SetTrigger("draw");
    if (Input.GetMouseButtonDown(1))
      animator.SetBool("parrying", true);
    else if (Input.GetMouseButtonUp(1))
      animator.SetBool("parrying", false);
    if (Input.GetMouseButtonDown(0))
    {
      animator.SetTrigger("attacked");
      animator.SetInteger("comboIndex", 1);
      // animator.ResetTrigger("attacked");
    }
  }
}
