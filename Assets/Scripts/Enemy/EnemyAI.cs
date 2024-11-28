
using System.Collections;
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

  [Header("Patroling")]
  public Vector3 walkPoint;
  public float walkPointRange;
  public float timeBetweenWalkpoints;
  bool walkPointSet;

  [Header("Attacking")]
  public float timeBetweenAttacks;
  public float sightRange, attackRange;
  bool alreadyAttacked;

  [Header("States")]
  public bool playerInSightRange;
  public bool playerInAttackRange;
  public bool isDead;

  [Header("Other")]
  public float destroyEnemyDelay = 20f;
  public Player player;
  private Transform playerCamPosition;
  public ParticleSystem bloodVFX;

  private void Awake()
  {
    agent = GetComponent<NavMeshAgent>();
    enemyKatanaAnimator = GetComponentInChildren<Animator>();
    playerCamPosition = player.GetComponent<PositionSyncer>().cameraPosition;
  }

  private void Update()
  {
    if (!isDead)
    {
      //Check for sight and attack range
      playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
      playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

      if (!playerInSightRange && !playerInAttackRange) Patroling();
      if (playerInSightRange && !playerInAttackRange) ChasePlayer();
      if (playerInAttackRange && playerInSightRange && !stunned) AttackPlayer();
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
    //Make sure enemy doesn't move
    agent.isStopped = true;

    orientation.LookAt(playerCamPosition.position);

    if (!alreadyAttacked)
    {
      ///Attack code here

      enemyKatanaAnimator.SetTrigger("attacked");

      ///End of attack code

      alreadyAttacked = true;
      Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
  }
  private void ResetAttack()
  {
    alreadyAttacked = false;
  }

  public void TakeDamage(float damage)
  {
    health -= damage;

    if (health == 0)
    {
      isDead = true;
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
    agent.isStopped = true;
    StartCoroutine(Stun());
  }

  private IEnumerator Stun()
  {
    yield return new WaitForSeconds(0.2f);
    enemyKatanaAnimator.SetTrigger("stunned");
    enemyKatanaAnimator.speed = 1f;
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