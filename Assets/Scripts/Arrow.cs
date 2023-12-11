using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 20f;
    
    Rigidbody2D arrowRb;
    // Get reference for the PlayerMovement script.
    PlayerMovement player;
    float xSpeed;
    
    void Awake()
    {
        arrowRb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        // We get the player's local scale X to verify if it's looking left or right to be able to shoot in that direction.
        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    void Update()
    {
        arrowRb.velocity = new Vector2(xSpeed, 0f);
    }

    // If the arrow hits something, then do stuff.
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
    
    // Destroy arrow if it hits anything.
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
