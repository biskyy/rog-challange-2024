using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
  public LayerMask playerLayer;
  public Transform player;
  public Transform orientation;
  public float radiusCheck = 10f;
  public Animator enemyKatanaAnimator;
  public ParticleSystem stunVFX;
  public bool stunned = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    bool playerNearby = Physics.CheckSphere(transform.position, radiusCheck, playerLayer);
    if (playerNearby && !stunned)
    {
      enemyKatanaAnimator.SetBool("enemySpotted", true);
      orientation.LookAt(player);
    }
    else
    {
      enemyKatanaAnimator.SetBool("enemySpotted", false);
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(1, 1, 1, 0.5f);
    Gizmos.DrawSphere(transform.position, radiusCheck);
  }

  public void GetStunned()
  {
    StartCoroutine(Stun());
  }

  private IEnumerator Stun()
  {
    yield return new WaitForSeconds(0.2f);
    enemyKatanaAnimator.speed = 1f;
    stunned = true;
    stunVFX.Play();
    yield return new WaitForSeconds(3f);
    stunned = false;
    stunVFX.Stop();
    stunVFX.Clear();
  }
}
