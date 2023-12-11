using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    
    
    void Awake()
    {
        // We search for the integer of the amount of Scene Persist objects that we have.
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        // So if there is already one Game Session object, destroy the new one.
        if (numScenePersist > 1)
        {
            Destroy(gameObject);
        }
        // If there is only one, don't destroy on load.
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // To reset the ScenePersist, just destroy it.
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
