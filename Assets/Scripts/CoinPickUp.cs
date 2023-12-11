using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int pointsForCoinPickUp = 10;

    // This bool is to prevent the coin to be collected twice before being destroyed.
    bool wasCollected = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            // When the coin collide with player, search for the Public Method "AddToScore" in Game Session script and add the int created for the points.
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickUp);
            // As it's a 2D game we will play the sound in the position of the main camera, we don't need it to be played in any other place.
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
