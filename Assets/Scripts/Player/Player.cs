using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
  public float maxHealth = 100f;
  public float health = 100f;

  [Header("Combat")]
  public RedKatana redKatana;
  public bool attacking = false;
  public bool parrying = false;
  public bool blocking = false;

  [Header("Movement")]
  public Movement movement;
  public bool grounded;
  public bool jumping;
  public bool crouching;

  [Header("HUD")]
  public Canvas PrototypeHUD;
  public Canvas PlayerHUD;

  // Start is called before the first frame update
  void Start()
  {
    movement = GetComponent<Movement>();
    redKatana = GetComponentInChildren<RedKatana>();
  }

  // Update is called once per frame
  void Update()
  {
    HandleInput();
    UpdatePlayerHUD();
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

  public void UpdatePlayerHUD()
  {
    if (PrototypeHUD)
    {
      PrototypeHUDReference.Instance.healthText.text = health.ToString();
      PrototypeHUDReference.Instance.velocityText.text = GetComponent<AverageVelocityForRb>().averageVelocity.ToString();
      PrototypeHUDReference.Instance.modifiedPTW.text = redKatana.modifiedParryTimeWindow.ToString();
      PrototypeHUDReference.Instance.currentPTW.text = redKatana.currentParryTimeWindow.ToString();
    }
  }

}
