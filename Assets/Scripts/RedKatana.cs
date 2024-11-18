using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedKatana : MonoBehaviour
{

  public Animator animator;

  public bool enemyKatanaTouched;

  public float intendedParryTimeWindow = 0.5f;
  public float modifiedParryTimeWindow = 0.5f;
  public float currentParryTimeWindow = 0f;

  public GameObject bladeCollider;

  public bool parried;

  public TextMeshProUGUI modifiedPTW;
  public TextMeshProUGUI currentPTW;

  public ParticleSystem parryVFX;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
      animator.SetTrigger("draw");
    if (Input.GetMouseButtonDown(1) && (animator.GetCurrentAnimatorStateInfo(0).IsName("Drawed") || animator.GetNextAnimatorStateInfo(0).IsName("Drawed")))
      StartParry();
    else if (Input.GetMouseButtonUp(1) && (animator.GetCurrentAnimatorStateInfo(0).IsName("Parry") || animator.GetNextAnimatorStateInfo(0).IsName("Parry")))
      StopParry();
    if (Input.GetMouseButtonDown(0))
    {
      animator.SetTrigger("attacked");
      animator.SetInteger("comboIndex", 1);
      // animator.ResetTrigger("attacked");
    }

    DecreaseParryTimeWindow();

    modifiedPTW.text = modifiedParryTimeWindow.ToString();
    currentPTW.text = currentParryTimeWindow.ToString();

  }

  void StartParry()
  {
    //bladeCollider.SetActive(true);
    animator.SetBool("parrying", true);
    if (currentParryTimeWindow > 0)
    {
      modifiedParryTimeWindow /= 2f;
    }

    currentParryTimeWindow = modifiedParryTimeWindow;
  }

  void StopParry()
  {
    //bladeCollider.SetActive(false);
    animator.SetBool("parrying", false);
    enemyKatanaTouched = false;
  }

  void DecreaseParryTimeWindow()
  {
    if (currentParryTimeWindow > 0)
    {
      currentParryTimeWindow -= Time.fixedDeltaTime;
      currentParryTimeWindow = Mathf.Clamp(currentParryTimeWindow, 0, intendedParryTimeWindow);
      // print(Time.fixedDeltaTime);
      //print("inside parry");
      if (enemyKatanaTouched)
      {
        print("parried");
        modifiedParryTimeWindow = intendedParryTimeWindow;
        parryVFX.Play();
      }
    }
  }

}
