using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandler : MonoBehaviour
{
  public EnemyAI enemyToDie;
  public GameObject thingToToggle;
  private bool initialState;

  // Start is called before the first frame update
  void Start()
  {
    initialState = thingToToggle.activeInHierarchy;
  }

  // Update is called once per frame
  void Update()
  {
    if (enemyToDie == null)
      thingToToggle.SetActive(!initialState);
  }
}
