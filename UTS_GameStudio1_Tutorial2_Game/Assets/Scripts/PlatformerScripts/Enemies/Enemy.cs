﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    public bool IsAlive { get { return isAlive; } }

    public bool CanMove { get { return canMove; } set { canMove = value; } }

    [SerializeField]
    Controller2D controller;

    [SerializeField]
    protected Collider2D collider;

    [SerializeField]
    protected Animator animator;

    protected Vector3 direction;

    protected bool isAlive;

    protected bool canMove;

    protected void Move() {

        if (direction == Vector3.right && controller.Collisions.isRight) {

            direction = -Vector3.right;

        } else if (direction == -Vector3.right && controller.Collisions.isLeft) {

            direction = Vector3.right;
        }

        controller.ApplyGravity(ref direction);

        controller.ApplyMovement(direction);
    }

    public abstract void OnPlayerStomp();

    protected void Init() {

        controller = GetComponent<Controller2D>();

        collider = GetComponent<BoxCollider2D>();

        CanMove = true;

        controller.collisionIgnoreConditions += IgnoreCollisions;

        controller.onCollision += CheckForFallTrigger;
    }

    protected bool IgnoreCollisions(RaycastHit2D hit, float direction = 0) {

        return hit.transform.tag == "Trigger" || (hit.distance == 0 && hit.transform.tag == "Trigger" || hit.transform.tag == "Enemy");
    }

    protected void CheckForFallTrigger(RaycastHit2D hit) {

        if (hit.transform.tag == "FallPoint") {

            FallTrigger fallTrigger = hit.transform.GetComponent<FallTrigger>();

            fallTrigger.OnTrigger(controller);
        }
    }
}
