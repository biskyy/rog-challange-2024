using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  public GameObject pauseObject;
  public bool gameIsPaused;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      print("paused");

      if (gameIsPaused)
      {
        pauseObject.SetActive(false);
        LevelManager.Instance.CloseSettings();
        Time.timeScale = 1f;
        gameIsPaused = false;
      }
      else
      {
        pauseObject.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
      }
    }
  }

  public void ResumeGame()
  {
    pauseObject.SetActive(false);
    LevelManager.Instance.CloseSettings();
    Time.timeScale = 1f;
    gameIsPaused = false;
  }

  public void PauseGame()
  {
    pauseObject.SetActive(false);
    Time.timeScale = 1f;
    gameIsPaused = false;
  }

  public void ExitToMainMenu()
  {
    ResumeGame();
    LevelManager.Instance.GoToScene("MainMenu");
    gameIsPaused = true;
  }
}
