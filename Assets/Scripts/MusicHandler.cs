using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
  private AudioSource musicSource;

  // Start is called before the first frame update
  void Start()
  {
    musicSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    musicSource.volume = Globals.Instance.musicVolume;
  }
}
