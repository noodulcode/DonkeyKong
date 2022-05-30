using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;

    private void Start() 
    {
        DontDestroyOnLoad(gameObject);
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);
    }

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;

        if (camera != null)
        {
            camera.cullingMask = 0; // as long as camera exists tell camera noit to render
        }

        Invoke(nameof(LoadScene), 1f); // waits one second before loading the scene
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    public void LevelCompleted()
    {
        score += 1000;
        
        int nextLevel = level + 1;
        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else 
        {
            LoadLevel(1);
        }
    }
    public void LevelFailed()
    {
        lives--;

        if (lives <= 0)
        { 
            NewGame();
        }
        else 
        {
            LoadLevel(level);  // load current level
        }
    }

}
