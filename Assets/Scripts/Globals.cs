using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Globals : MonoBehaviour
{
  public static Globals Instance { get; private set; }

  public float sensitivityMultiplier = 1.0f;
  public float musicVolume = 0.25f;
  public float sfxVolume = 0.25f;
  public bool victory = false;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject); // Ensure there's only one instance
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject); // Persist through scenes
  }
}
