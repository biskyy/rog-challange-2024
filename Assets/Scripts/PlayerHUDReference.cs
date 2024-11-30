using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDReference : MonoBehaviour
{
  public static PlayerHUDReference Instance { get; private set; }

  public Slider healthSlider;

  public TextMeshProUGUI healthAmount;

  public void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject); // Ensure there's only one instance
      return;
    }
    Instance = this;
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}

