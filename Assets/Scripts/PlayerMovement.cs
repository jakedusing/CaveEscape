using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] AudioClip arrowShotSFX;
    [SerializeField] Vector2 deathKick = new Vector2(15f, 15f);
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue value) {
        if (!isAlive) {
            return;
        }
        myAnimator.SetTrigger("isShooting");
        Instantiate(arrow, bow.position, transform.rotation);
        AudioSource.PlayClipAtPoint(arrowShotSFX, Camera.main.transform.position);
    }

    void OnMove(InputValue value) {
        if (!isAlive) {
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) {
        if (!isAlive) {
            return;
        }
        // check is player is touching the ground before allowing jump
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }

        if (value.isPressed) {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
        
    }

    void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, 
        myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) {
        transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }

    }

    void ClimbLadder() {
        // check is player is touching ladder before allowing climbing
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        
    }

    void Die() {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
