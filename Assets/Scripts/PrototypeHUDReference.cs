using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeHUDReference : MonoBehaviour
{
  public static PrototypeHUDReference Instance { get; private set; }

  public TextMeshProUGUI healthText;
  public TextMeshProUGUI velocityText;
  public TextMeshProUGUI modifiedPTW;
  public TextMeshProUGUI currentPTW;

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
