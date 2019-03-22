using System.Collections;
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

    // Delegate for anything which needs to know whether the player is moving
    public delegate void PlayerMovedHandler();

    public event PlayerMovedHandler playerMoved;

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

            animator.SetBool("isWalking", false);

        } else {

            controller.ApplyGravity(ref input);

            if (SpacePressed()) {

                controller.Jump(ref input);

            }

            animator.SetBool("isWalking", true);

            animator.SetFloat("DirectionX", input.x);

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

    public void OnDeath() {

        isAlive = false;
        animator.SetBool("Alive", false);
    }
}
