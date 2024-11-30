
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
  [Header("Enemy")]
  public NavMeshAgent agent;
  public Transform orientation;
  public float health;
  public LayerMask whatIsGround, whatIsPlayer;
  public SilverKatana katana;
  public Animator enemyKatanaAnimator;
  public bool stunned;
  public ParticleSystem stunVFX;
  public bool lockOrientationY;

  [Header("Patroling")]
  public Vector3 walkPoint;
  public float walkPointRange;
  public float timeBetweenWalkpoints;
  bool walkPointSet;

  [Header("Attacking")]
  public float timeBetweenAttacks;
  public float sightRange, attackRange;
  bool alreadyAttacked;
  public bool useGroundAttack;
  public bool useTornadoAttack;

  [Header("States")]
  public bool playerInSightRange;
  public bool playerInAttackRange;
  public bool isDead;

  [Header("Other")]
  public float destroyEnemyDelay = 20f;
  public Player player;
  private Transform playerCamPosition;
  public ParticleSystem bloodVFX;
  private AudioSource hurtSFX;

  private void Awake()
  {
    agent = GetComponent<NavMeshAgent>();
    enemyKatanaAnimator = GetComponentInChildren<Animator>();
    playerCamPosition = player.GetComponent<PositionSyncer>().cameraPosition;
  }

  private void Start()
  {
    hurtSFX = GetComponent<AudioSource>();
    SFXManager.Instance.RegisterAudioSource(hurtSFX);
  }

  private void OnDestroy()
  {
    SFXManager.Instance.DeregisterAudioSource(hurtSFX);
  }

  private void Update()
  {
    if (!isDead && !stunned)
    {
      //Check for sight and attack range
      playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
      playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

      if (!playerInSightRange && !playerInAttackRange) Patroling();
      if (playerInSightRange && !playerInAttackRange) ChasePlayer();
      if (playerInAttackRange && playerInSightRange) AttackPlayer();
      transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
  }

  private void Patroling()
  {
    enemyKatanaAnimator.SetBool("enemySpotted", false);
    agent.isStopped = false;
    if (!walkPointSet) Invoke(nameof(SearchWalkPoint), timeBetweenWalkpoints);

    if (walkPointSet)
    {
      agent.SetDestination(walkPoint);
    }

    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //Walkpoint reached
    if (distanceToWalkPoint.magnitude < 1f)
      walkPointSet = false;
  }
  private void SearchWalkPoint()
  {
    //Calculate random point in range
    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    float randomX = Random.Range(-walkPointRange, walkPointRange);

    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
      walkPointSet = true;
  }

  private void ChasePlayer()
  {
    enemyKatanaAnimator.SetBool("enemySpotted", true);
    orientation.LookAt(playerCamPosition);
    agent.isStopped = false;
    agent.SetDestination(player.transform.position);
  }

  private void AttackPlayer()
  {
    agent.isStopped = true;
    if (lockOrientationY) orientation.localRotation = Quaternion.Euler(0, orientation.rotation.y, 0);
    else
      orientation.LookAt(playerCamPosition.position);

    if (!alreadyAttacked)
    {
      int attack = Random.Range(0, 3);
      ///Attack code here
      if (attack == 0 && !(useGroundAttack && useTornadoAttack))
      {
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        enemyKatanaAnimator.SetTrigger("attacked");
      }
      else if (attack == 1 && useGroundAttack)
      {
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks + 2f);
        enemyKatanaAnimator.SetTrigger("groundAttack");
      }
      else if (attack == 2 && useTornadoAttack)
      {
        enemyKatanaAnimator.SetTrigger("tornadoAttack");
        lockOrientationY = true;
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks + 4f);
      }
      ///End of attack code

    }
  }
  private void ResetAttack()
  {
    alreadyAttacked = false;
    lockOrientationY = false;
  }

  public void TakeDamage(float damage)
  {
    health -= damage;

    if (health == 0)
    {
      isDead = true;
      hurtSFX.Play();
      gameObject.AddComponent<Rigidbody>();
      gameObject.GetComponent<Rigidbody>().freezeRotation = false;
      gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
      gameObject.GetComponent<Rigidbody>().mass = 5f;
      gameObject.GetComponent<Rigidbody>().AddForce(orientation.forward * -1 * 100f);
      Destroy(agent);
      Invoke(nameof(DestroyEnemy), destroyEnemyDelay);
      katana.Invoke("DestroyKatana", destroyEnemyDelay);
      katana.TurnIntoRagdoll();
      player.AddHealth(20f);
    }
  }

  public void GetStunned()
  {
    StartCoroutine(pauseAnimation());
    if (!enemyKatanaAnimator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack"))
    {
      agent.isStopped = true;
      StartCoroutine(Stun());
    }
  }

  private IEnumerator pauseAnimation()
  {
    enemyKatanaAnimator.speed = 0f;
    yield return new WaitForSeconds(0.2f);
    enemyKatanaAnimator.speed = 1f;
  }

  private IEnumerator Stun()
  {
    stunned = true;
    stunVFX.Play();
    yield return new WaitForSeconds(3f);
    if (agent)
      agent.isStopped = false;
    stunned = false;
    stunVFX.Stop();
    stunVFX.Clear();
  }

  public void DestroyEnemy()
  {
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, sightRange);
  }
}