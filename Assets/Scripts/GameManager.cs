using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject gameplayUI;
    public GameObject victoryScreen;
    public AudioSource gameMusic;
    public bool isGameActive;

    public void StartGame()
    {
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
        if (gameMusic != null)
        {
            gameMusic.Play();
        }
    }

    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        gameplayUI.gameObject.SetActive(false);

        isGameActive = false;
    }

    public void Victory()
    {
        victoryScreen.gameObject.SetActive(true);
        gameplayUI.gameObject.SetActive(false);

        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
