using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
  public static SFXManager Instance { get; private set; }

  private List<AudioSource> audioSources = new List<AudioSource>();
  private float globalVolume = 0.25f;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject); // Ensure only one instance exists
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject); // Persist through scenes
  }

  private void Update()
  {
    SetGlobalVolume(Globals.Instance.sfxVolume);
  }

  /// <summary>
  /// Adds an AudioSource to the manager.
  /// </summary>
  public void RegisterAudioSource(AudioSource audioSource)
  {
    if (!audioSources.Contains(audioSource))
    {
      audioSources.Add(audioSource);
      audioSource.volume = globalVolume;
    }
  }

  /// <summary>
  /// Removes an AudioSource from the manager.
  /// </summary>
  public void DeregisterAudioSource(AudioSource audioSource)
  {
    if (audioSources.Contains(audioSource))
    {
      audioSources.Remove(audioSource);
    }
  }

  /// <summary>
  /// Sets the global volume for all managed AudioSources.
  /// </summary>
  public void SetGlobalVolume(float volume)
  {
    globalVolume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1

    foreach (var audioSource in audioSources)
    {
      if (audioSource != null)
      {
        audioSource.volume = globalVolume;
      }
    }
  }

  /// <summary>
  /// Gets the current global volume.
  /// </summary>
  public float GetGlobalVolume()
  {
    return globalVolume;
  }
}
