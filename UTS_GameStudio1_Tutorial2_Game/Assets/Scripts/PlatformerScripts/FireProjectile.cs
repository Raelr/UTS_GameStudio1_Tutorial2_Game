using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : PlatformUser {

    public Vector3 Direction { get { return direction; } set { direction = value; } }

    Vector3 direction;

    Controller2D controller;

    void Start() {

        controller = GetComponent<Controller2D>();

        controller.onCollision += CheckCurrentCollider;

        controller.onCollision += OnEnemyCollision;
    }

    void Update() {

        BounceToLocation();

    }

    void BounceToLocation() {

        if (controller.Collisions.isRight || controller.Collisions.isLeft) {
            gameObject.SetActive(false);
        }

        controller.ApplyGravity(ref direction);

        if (controller.Collisions.isBelow) {

            controller.Bounce();
        }

        controller.ApplyMovement(direction);
    }

    protected override void CheckForTrigger(RaycastHit2D hit) {
        throw new System.NotImplementedException();
    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0) {

        bool success = false;

        if (hit.transform.tag == "Player") {

            success = true;
        }

        return success;
    }

    public void OnEnemyCollision(RaycastHit2D hit) {

        if (hit.transform.tag == "Enemy" || hit.transform.tag == "VulnerablePoint") {

            Enemy enemy = hit.transform.GetComponent<Enemy>();

            enemy.OnHitByFireball();

            gameObject.SetActive(false);
        }
    }
}
