using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mashroom : PowerUp
{
    [SerializeField]
    Controller2D controller;

    Vector3 direction;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<Controller2D>();
        InitialiseAnimation();
        direction = Vector3.right;
        ability = Abilities.Mashroom;
    }

    private void Update() {

        if (!isInitialising) {
            
            Move();
        }
    }

    void Move() {

        if (direction == Vector3.right && controller.Collisions.isBelow && controller.Collisions.isRight) {
            
            direction = -Vector3.right;
        } else if (direction == -Vector3.right && controller.Collisions.isBelow && controller.Collisions.isLeft) {

            direction = Vector3.right;
        }

        controller.ApplyGravity(ref direction);

        controller.ApplyMovement(direction);
    }
}

     

