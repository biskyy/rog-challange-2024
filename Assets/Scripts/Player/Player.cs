using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
  public float maxHealth = 100f;
  public float health = 100f;

  [Header("Combat")]
  public bool attacking = false;
  public bool parrying = false;
  public bool blocking = false;

  [Header("Movement")]
  public Movement movement;
  public bool grounded;
  public bool jumping;
  public bool crouching;

  // Start is called before the first frame update
  void Start()
  {
    movement = GetComponent<Movement>();
  }

  // Update is called once per frame
  void Update()
  {
    HandleInput();
  }

  void HandleInput()
  {
    // handle jumping
    jumping = Input.GetKey(KeyCode.Space);

    if (Input.GetKeyDown(KeyCode.Space) && grounded)
      movement.Jump();

    // handle crouching
    crouching = Input.GetKey(KeyCode.LeftShift);

    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      movement.StartCrouch();
    }
    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
      movement.StopCrouch();
    }

    if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.R))
      transform.position = new Vector3(0, 1, 0);
  }

  public void TakeDamage(float damage)
  {
    health -= damage;
  }

  public void AddHealth(float amount)
  {
    health += amount;
  }

  public void RestoreHealth()
  {
    health = maxHealth;
  }

  public void GameOver()
  {
    print("game over");
  }

}
