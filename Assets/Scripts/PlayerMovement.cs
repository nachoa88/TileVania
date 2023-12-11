using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// As we're using Unity's Input System to control our player, we need to add:
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (20f, 20f);
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;

    Vector2 moveInput;
    Rigidbody2D playerRb;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float gravityScaleAtStart;

    bool isAlive = true;
    
    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        // We get the value of the gravity scale from the RB in the inspector.
        gravityScaleAtStart = playerRb.gravityScale;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // If player is not alive, return from this method, don't do anything else.
        if (!isAlive)
        {
            return;
        }
        
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    // The name of this method comes from the Input Manager (Input Actions) created by us, we don't need to call this method. 
    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        // We need to get the value from our move Input.
        moveInput = value.Get<Vector2>();
    }

    // We created also a Jump method and assaign the keys, the name also comes from the Input Manager created in Unity.
    void OnJump(InputValue value) 
    {
        if (!isAlive)
        {
            return;
        }
        // We check if the player's collider is not touching the layer mask named as ground. So, if it's not, then return, do nothing, don't jump.
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed) 
        {
            playerRb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        Instantiate(arrow, bow.position, transform.rotation);
    }

    void Run()
    {
        // We create a new vector2 to modify the player's velocity in X vector and prevent from flying by not being able to add speed to Y vector & keeping the same RB's velocity.
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, playerRb.velocity.y);
        playerRb.velocity = playerVelocity;

        // We use the same bool to flip the character, if it's moving, set isRunning to true, if not moving, don't run. 
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    // This method will provide the 1 or -1 to be able to flip our character in the local scale at X, wich will flip the sprite.
    void FlipSprite()
    {
        /* Mathf.Abs give us the absolute value no matter the sign, so if it's -4.5, it will give us 4.5.
           Mathf.Epsilon gives us the smallest float value different from zero, 0.00000001 */
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
        /* Mathf.Sign will gives us only a 1 when value is positive or zero & -1 when it's negative, so if its 3.5 it will give 1, if its -4 it will give us -1. 
        So we get the RB's velocity in the X Vector and if its going right it will give us 1, and if it's left -1. We don't need Y value, so we leave it at 1. */
            transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x), 1f);
        }
    }

    // We will use a similar method than the one we used for running but moving the Y Vector.
    void ClimbLadder()
    {
        // First we check that the layer mask is climbing.
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            // So if we're not touching the Ladder (Climbing Layer), leave the gravity as it was at the start.
            playerRb.gravityScale = gravityScaleAtStart;
            // Set animation into false.
            playerAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(playerRb.velocity.x, moveInput.y * climbSpeed);
        playerRb.velocity = climbVelocity;
        // So if the player is climbing, we set the gravity to 0 so that if you're not pressing the up button, in will stay in position.
        playerRb.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
        // Set the animation into true.
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        // If player is touching enemies layaer, do stuff.
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            // We trigger Death animation, "Dying".
            playerAnimator.SetTrigger("Dying");
            playerRb.velocity = deathKick;
            // We get access to the script Game Session and our Proccess Player Death method.
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }


}
