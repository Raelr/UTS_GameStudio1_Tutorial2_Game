using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

    public bool IsAlive { get { return isAlive; } set { isAlive = value; } }

    [Header("Player Controller")]
    [SerializeField]
    Controller2D controller;

    Vector3 input;

    [SerializeField]
    Animator animator;

    bool isAlive;

    // Delegate for anything which needs to know whether the player is moving
    public delegate void PlayerMovedHandler();

    public event PlayerMovedHandler playerMoved;

    //status store the stage of powerup, 1 is normal, 2 is mashroomed, 3 is fire mode, 4 is invincible.
    private int status = 1;

    void Start() {

        isAlive = true;

        controller = GetComponent<Controller2D>();

        animator = GetComponent<Animator>();
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

        bool crouching;

        if (Input.GetKey("s") || Input.GetKey("down")) {
            crouching = true;
        } else {
            crouching = false;
        }

        controller.Crouch(crouching);

        if (IsStill() && !SpacePressed()) {
            // If we arent moving then just apply gravity normally.
            controller.ApplyGravity(ref input, true);

        } else {

            controller.ApplyGravity(ref input);

            if (SpacePressed()) {

                controller.Jump(ref input);

            }

            controller.ApplyMovement(input);
        }

        // If anything is listening for player movement then invoke the delegate.
        if (playerMoved != null) {

            playerMoved.Invoke();
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

    public void GetPowerUp(PowerUp.Abilities ability){
        switch(ability){
            case PowerUp.Abilities.Mashroom:
                transform.position += new Vector3(0, GetComponent<Collider2D>().bounds.size.y/2, 0);
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
}
