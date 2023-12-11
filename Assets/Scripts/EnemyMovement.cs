using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D enemyRb;
    
    void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        enemyRb.velocity = new Vector2(moveSpeed, 0);
    }

    // We are using a trigger to make the enemy flip when it touches the wall.
    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRb.velocity.x)), 1f);
    }
}
