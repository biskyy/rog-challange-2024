using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKatana : MonoBehaviour
{

  public Animator animator;

  public float parryTimeFrame;
  public bool enemyKatanaTouched;

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
      StartParry();
    else if (Input.GetMouseButtonUp(1))
      StopParry();
    if (Input.GetMouseButtonDown(0))
    {
      animator.SetTrigger("attacked");
      animator.SetInteger("comboIndex", 1);
      // animator.ResetTrigger("attacked");
    }

    while (parryTimeFrame > 0)
    {
      parryTimeFrame -= Time.fixedDeltaTime;
      // print(Time.fixedDeltaTime);
      // print("inside parry");
      if (enemyKatanaTouched)
      {
        print("parried");
      }
    }

  }

  void StartParry()
  {
    animator.SetBool("parrying", true);
    parryTimeFrame = 50f;
  }

  void StopParry()
  {
    animator.SetBool("parrying", false);
    parryTimeFrame = 50f;
    enemyKatanaTouched = false;
  }

}
