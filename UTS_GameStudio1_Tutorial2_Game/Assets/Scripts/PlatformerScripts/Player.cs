using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
    public Status CurrentStatus { get { return status; } }

    [Header("Player Controller")]
    [SerializeField]
    Controller2D controller;

    Vector3 input;

    [SerializeField]
    Animator animator;

    bool isAlive;

    //status store the stage of powerup, 1 is normal, 2 is mashroomed, 4 is fire mode, 8 is invincible, 
    //16 is untouchable, which happen during status changing time, normally last for few seconds.
    //Flags means status value could be used as bit values, so that multiple values can be true at once.
    //For example, mario can have SizeUp/OnFire/OnInvincible all at one time.
    [Flags] public enum Status { Normal = 1, SizeUp = 2, OnFire = 4, OnInvincible = 8, OnUnTouchable = 16 }
    private float timer;
    private Status status = Status.Normal;

    SpriteRenderer renderer;

    Vector3 facingDirection;

    // Delegate for anything which needs to know whether the player is moving
    public delegate void PlayerMovedHandler();

    public event PlayerMovedHandler playerMoved;

    void Start() {

        isAlive = true;

        controller = GetComponent<Controller2D>();

        animator = GetComponent<Animator>();

        renderer = GetComponentInChildren<SpriteRenderer>();

        facingDirection = Vector2.right;
    }

    void Update() {

        Tick();
        if (isAlive) {
            MoveByInput();
        }
    }

    /// <summary>
    /// for timer
    /// </summary>
    void Tick() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                //This is bit operation. To set the status 0 on Status.OnUnTouchable bit.
                //Which means mario is no longer Untouchable.
                status &= ~Status.OnUnTouchable;
            }
        }
    }

    /// <summary>
    /// Maps player inputs into movement and calls appropriate commands from the controller.
    /// </summary>

    void MoveByInput() {

        // Get the current axis values and add them to a vector.
        FindAxes();

        UpdateFacingDirection(input);

        bool crouching;

        bool isJumping = false;

        if (Input.GetKey("s") || Input.GetKey("down")) {
            crouching = true;
        } else {
            crouching = false;
        }

        controller.Crouch(crouching);

        if (IsStill() && !SpacePressed()) {

            // If we arent moving then just apply gravity normally.
            controller.ApplyGravity(ref input, true);

            animator.SetBool("isWalking", false);

        } else {

            controller.ApplyGravity(ref input);

            if (SpacePressed()) {

                controller.Jump(ref input);
            }

            if (controller.Collisions.isBelow && input.x != 0) {

                animator.SetBool("isWalking", true);

            }

            controller.ApplyMovement(input);
        }

        if (!controller.Collisions.isBelow) {

            isJumping = true;
        }

        animator.SetBool("isJumping", isJumping);

        // If anything is listening for player movement then invoke the delegate.
        if (playerMoved != null) {

            playerMoved.Invoke();
        }
    }

    void UpdateFacingDirection(Vector3 input) {

        float xValue = Mathf.RoundToInt(input.x);

        facingDirection = new Vector3(xValue, 0, 0);

        if (Utilities.VectorEquals(facingDirection, Vector3.right)) {

            if (renderer.transform.rotation.y != 0) {
                renderer.transform.rotation = Quaternion.Euler(renderer.transform.rotation.x, 0, renderer.transform.rotation.z);
            }

        } else if (Utilities.VectorEquals(facingDirection, -Vector3.right)) {

            if (renderer.transform.rotation.y != 180) {
                renderer.transform.rotation = Quaternion.Euler(renderer.transform.rotation.x, 180, renderer.transform.rotation.z);
            }
        }
    }

    /// <summary>
    /// Modifies the input vector to have the horizontal and vertical axes values.
    /// </summary>

    void FindAxes() {

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.z = 0;
    }

    bool SpacePressed() {
        return Input.GetKeyDown(KeyCode.Space);
    }

    bool IsStill() {
        return input.x == 0 && input.y == 0;
    }


    public void GetPowerUp(PowerUp.Abilities ability) {
        if (status == Status.Normal) {
            SizeUp();
        }

        switch (ability) {
            case PowerUp.Abilities.Mashroom:
                break;
            case PowerUp.Abilities.Fire:
                //Bit operation. Means the bit on Status.OnFire is set to 1;
                status |= Status.OnFire;
                break;
            case PowerUp.Abilities.Invincible:
                status |= Status.OnInvincible;
                break;
            default:
                break;
        }
    }

    public void OnDeath() {

        isAlive = false;
        animator.SetBool("Alive", false);
        Debug.Log("I'm dead");
    }

    public void SizeUp() {
        float feety = transform.position.y - GetComponent<Collider2D>().bounds.size.y * 0.5f;
        transform.localScale = Vector3.one * 1.6f;
        transform.position = new Vector3(transform.position.x, GetComponent<Collider2D>().bounds.size.y * 0.5f + feety, transform.position.z);
        status = Status.SizeUp;
        ChangeStatus();
    }

    public void Shrink() {
        float feety = transform.position.y - GetComponent<Collider2D>().bounds.size.y * 0.5f;
        transform.localScale = Vector3.one;
        transform.position = new Vector3(transform.position.x, GetComponent<Collider2D>().bounds.size.y * 0.5f + feety, transform.position.z);
        status = Status.Normal;
        ChangeStatus();
    }

    public void ChangeStatus() {
        status |= Status.OnUnTouchable;
        timer = 2f;
    }

    public void OnHurt() {
        if (status != Status.Normal) {
            Shrink();
            Debug.Log("I'm hurt");
        } else {
            OnDeath();
        }
    }
}