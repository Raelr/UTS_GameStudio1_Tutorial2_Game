using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleStar : PowerUp {

    Vector3 direction;

    // Start is called before the first frame update
    void Awake() {

        controller = GetComponent<Controller2D>();
        InitialiseAnimation();
        direction = Vector3.right;
        ability = Abilities.Invincible;

        controller.onCollision += CheckCurrentCollider;
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
        controller.Jump(ref direction);

        controller.ApplyMovement(direction);
    }

    protected override void CheckForTrigger(RaycastHit2D hit) {
        throw new System.NotImplementedException();
    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0) {
        throw new System.NotImplementedException();
    }

    public override void OnPickup() {

        Debug.Log("Picked up test");

        Destroy(gameObject);
    }
}

