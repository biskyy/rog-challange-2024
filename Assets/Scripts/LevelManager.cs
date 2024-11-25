using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  public static LevelManager Instance { get; private set; }

  [Header("Scene Names")]
  public string prototypeScene = "Prototype";
  public string homeScene = "Level1";
  public string level1Scene = "Level1";
  public string level2Scene = "Level2";

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

  public void GoToScene(string sceneName)
  {
    if (Application.CanStreamedLevelBeLoaded(sceneName))
    {
      SceneManager.LoadScene(sceneName);
    }
    else
    {
      Debug.LogError($"Scene {sceneName} does not exist or is not added to the build settings.");
    }
  }

  public void ReloadCurrentScene()
  {
    Scene currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.name);
  }

  public void LoadNextScene()
  {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int nextSceneIndex = currentSceneIndex + 1;

    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    {
      SceneManager.LoadScene(nextSceneIndex);
    }
    else
    {
      Debug.LogWarning("No more scenes to load. Consider looping or ending the game.");
    }
  }
}