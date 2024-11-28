using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  public GameObject pauseObject;
  public GameObject gameOverObject;

  public bool gameOver;
  public bool gameIsPaused;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
    {
      print("paused");

      if (gameIsPaused)
      {
        ResumeGame();
      }
      else
      {
        PauseGame();
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
    pauseObject.SetActive(true);
     Time.timeScale = 0f;
    gameIsPaused = true;
  }

  public void ExitToMainMenu()
  {
    ResumeGame();
    LevelManager.Instance.GoToScene("MainMenu");
    gameIsPaused = true;
  }

  public void GameOver()
  {
     Time.timeScale = 0f;
    gameOver = true;
    gameOverObject.SetActive(true);
  }
}
