﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool IsAlive { get { return isAlive; } set { isAlive = value; } }

    [Header("Player Controller")]
    [SerializeField]
    Controller2D controller;

    Vector3 input;

    [SerializeField]
    Animator animator;

    bool isAlive;

    //status store the stage of powerup, 1 is normal, 2 is mashroomed, 3 is fire mode, 4 is invincible.
    private int status = 1;

    SpriteRenderer renderer;

    Vector3 facingDirection;

    // Delegate for anything which needs to know whether the player is moving
    public delegate void PlayerMovedHandler();

    public event PlayerMovedHandler playerMoved;

    Collider2D currentPlatformCollider;

    Platform currentPlatform;

    void Start() {

        isAlive = true;

        controller = GetComponent<Controller2D>();

        animator = GetComponent<Animator>();

        renderer = GetComponentInChildren<SpriteRenderer>();

        facingDirection = Vector2.right;

        controller.onCollision += CheckEnemyHit;

        controller.onCollision += CheckForPowerUp;

        controller.onCollision += CheckForTrigger;

        controller.onCollision += CheckCurrentCollider;

        controller.directionConditions += TriggerPlatformMove;

        controller.collisionIgnoreConditions += CanJumpThrough;
    }

    void Update() {

        if (isAlive) {
            MoveByInput();
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
        switch (ability) {
            case PowerUp.Abilities.Mashroom:
                transform.position += new Vector3(0, GetComponent<Collider2D>().bounds.size.y / 2, 0);
                transform.localScale *= 2;
                status = 2;
                break;
            case PowerUp.Abilities.Fire:
                status = 3;
                break;
            case PowerUp.Abilities.Invincible:
                status = 4;
                break;
            default:
                break;
        }
    }

    public void OnDeath() {

        isAlive = false;
        animator.SetBool("Alive", false);
    }

    public void CheckEnemyHit(RaycastHit2D hit) {

        if (hit.transform.tag == "VulnerablePoint") {

            Enemy enemy = hit.transform.parent.GetComponent<Enemy>();

            enemy.OnPlayerStomp();

            controller.Bounce();

        } else if (hit.transform.tag == "Enemy") {

            Debug.Log("Enemy");

            LevelManager.instance.OnPlayerKilled();
        }
    }

    void CheckForPowerUp(RaycastHit2D hit) {

        if (hit.transform.tag.Equals("PowerUp")) {

            PowerUp powerUp = hit.transform.GetComponent<PowerUp>();

            powerUp.OnPickup();

            GetPowerUp(powerUp.GetAbility());
        }
    }

    void CheckForTrigger(RaycastHit2D hit) {

        if (hit.transform.tag == "Trigger" && currentPlatformCollider != hit.collider) {

            Trigger trigger = hit.transform.GetComponent<Trigger>();

            trigger.OnPlayerEnter();

            currentPlatformCollider = hit.collider;

        } else if (hit.transform.tag == "FallPoint" && currentPlatformCollider != hit.collider) {

            FallTrigger trigger = hit.transform.GetComponent<FallTrigger>();

            trigger.OnTrigger(controller);

            currentPlatformCollider = hit.collider;
        }
    }

    void CheckCurrentCollider(RaycastHit2D hit) {

        if (hit.transform.tag == "Platform" && hit.collider != currentPlatformCollider) {

            currentPlatformCollider = hit.collider;
            Platform platform = hit.transform.GetComponent<Platform>();
            currentPlatform = platform == null ? null : platform;
        }
    }

    void TriggerPlatformMove() {

        if (controller.Collisions.isAbove) {
            currentPlatform.OnPlayerHit();
        }
    }

    bool CanJumpThrough(RaycastHit2D hit, float direction) {

        if (currentPlatformCollider != null) {
            return hit.transform.tag == "PowerUp" || currentPlatform.AllowedToJumpThrough(direction) || controller.IsCrouching && currentPlatform.CanFallThrough() || hit.transform.tag == "Trigger" || hit.transform.tag == "Enemy" || hit.transform.tag == "VulnerablePoint";
        } else {
            return false;
        }

    }
}