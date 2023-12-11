using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;


    void Awake()
    {
        // We search for the integer of the amount of Game Session objects that we have.
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        // So if there is already one Game Session object, destroy the new one.
        if (numGameSession > 1 )
        {
            Destroy(gameObject);
        }
        // If there is only one, don't destroy on load.
        else 
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        // It will turn the integer into a string, so that it gets written in the TMP.
        livesText.text = playerLives.ToString();
        // Do the same for score.
        scoreText.text = score.ToString();
    }

    // We create this public so that we're able to access it from  other script.
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        // If the player has at least one life, reset Game Session.
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        // Add the points.
        score += pointsToAdd;
        // Show it in TMP.
        scoreText.text = score.ToString();
    }

    void TakeLife()
    {
        // First we substract one life.
        playerLives--;
        // Then we look for the number of scene that we are currently running.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Finally we reload the current scene.
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }
    
    void ResetGameSession()
    {
        // Before Loading Scene, we have to reset our Scene Persist object, so we call the public method we've created to destroy it.
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        // Then load main menu, in this case first level.
        SceneManager.LoadScene(0);
        // Destroy this instance of Game Session so that if you hit play again, this hole Game Session restores and starts from zero.
        Destroy(gameObject);
    }
}
