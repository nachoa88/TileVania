using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only start coroutine if the trigger is the player.
        if (other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        // The first thing that you have to do is to determinate how long it has to wait before starting coroutine.
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        // We get the scene and then we play the following in our Build Index in Unity.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // So if the next scene is equal to the total amount of levels, return the index to 0, so that we avoid getting error after passing last level.
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        // Before Loading Next Scene, we have to reset our Scene Persist object, so we call the public method we've created to destroy it.
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        // After checking and arrenging the next scene index, load that next scene.
        SceneManager.LoadScene(nextSceneIndex);
    }





}
