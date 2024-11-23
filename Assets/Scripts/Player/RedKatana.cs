using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedKatana : MonoBehaviour
{
  public Player player;
  public Animator animator;

  public bool enemyKatanaTouched;

  public float intendedParryTimeWindow = 0.5f;
  public float modifiedParryTimeWindow = 0.5f;
  public float currentParryTimeWindow = 0f;

  public float originalParryResetTimer = 2f;
  public float parryResetTimer = 0f;

  public GameObject bladeCollider;

  public bool parried;



  public TextMeshProUGUI modifiedPTW;
  public TextMeshProUGUI currentPTW;

  public ParticleSystem parryVFX;

  // Start is called before the first frame update
  void Start()
  {
    player = GetComponentInParent<Player>();
    //Time.timeScale = 0.4f;
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
    }

    HandleResetParryTimer();
    DecreaseParryTimeWindow();

    modifiedPTW.text = modifiedParryTimeWindow.ToString();
    currentPTW.text = currentParryTimeWindow.ToString();

  }

  void StartParry()
  {
    parryResetTimer = originalParryResetTimer;

    //bladeCollider.SetActive(true);
    player.blocking = true;
    animator.SetBool("parrying", true);

    currentParryTimeWindow = modifiedParryTimeWindow;
    modifiedParryTimeWindow /= 2f;
  }

  void StopParry()
  {
    //bladeCollider.SetActive(false);
    player.blocking = false;
    animator.SetBool("parrying", false);
    enemyKatanaTouched = false;
  }

  void HandleResetParryTimer()
  {
    if (parryResetTimer > 0)
    {
      parryResetTimer -= Time.fixedDeltaTime;
    }
    if (parryResetTimer <= 0 && modifiedParryTimeWindow != intendedParryTimeWindow)
    {
      modifiedParryTimeWindow = intendedParryTimeWindow;
    }
  }

  void DecreaseParryTimeWindow()
  {
    if (currentParryTimeWindow > 0)
    {
      player.parrying = true;
      currentParryTimeWindow -= Time.fixedDeltaTime;
      currentParryTimeWindow = Mathf.Clamp(currentParryTimeWindow, 0, intendedParryTimeWindow);

      if (enemyKatanaTouched)
      {
        print("parried");
        modifiedParryTimeWindow = intendedParryTimeWindow;
        StopCoroutine("ResetParried");
        parried = true;
        StartCoroutine(ResetParried());
        currentParryTimeWindow = 0f;
        parryResetTimer = 0f;

        parryVFX.Clear();
        parryVFX.Stop();
        parryVFX.Play();
      }
    }
    else
    {
      player.parrying = false;
      //StartCoroutine(ResetParried());
      // parried = false;
    }
  }

  IEnumerator ResetParried()
  {
    yield return new WaitForSeconds(1f);
    parried = false;
  }

  IEnumerator StopParryVFX()
  {
    yield return null;
  }
}