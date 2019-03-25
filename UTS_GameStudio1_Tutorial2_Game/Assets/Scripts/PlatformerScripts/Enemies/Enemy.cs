using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PlatformUser {

    public bool IsAlive { get { return isAlive; } }

    public bool CanMove { get { return canMove; } set { canMove = value; } }

    [SerializeField]
    protected Controller2D controller;

    [SerializeField]
    protected Collider2D collider;

    [SerializeField]
    protected Animator animator;

    protected Vector3 direction;

    [SerializeField]
    SpriteRenderer renderer;

    protected bool isAlive;

    protected bool canMove;

    protected void Move() {

        if (Utilities.VectorEquals(direction, Vector3.right) && controller.Collisions.isRight) {

            direction = -Vector3.right;

        } else if (Utilities.VectorEquals(direction, -Vector3.right) && controller.Collisions.isLeft) {

            direction = Vector3.right;
        }

        controller.ApplyGravity(ref direction);

        controller.ApplyMovement(direction);
    }


    protected void UpdateFacingDirection(Vector3 input) {

        float xValue = Mathf.RoundToInt(input.x);

        Vector3 direction = new Vector3(xValue, 0, 0);

        if (direction != Vector3.zero) {

            if (Utilities.VectorEquals(direction, Vector3.right)) {

                if (renderer.transform.rotation.y == 0) {
                    renderer.transform.rotation = Quaternion.Euler(new Vector3(renderer.transform.rotation.eulerAngles.x, 180, renderer.transform.rotation.eulerAngles.z));
                }

            } else if (Utilities.VectorEquals(direction, -Vector3.right)) {

                if (renderer.transform.rotation.y == 180) {

                    renderer.transform.rotation = Quaternion.Euler(new Vector3(renderer.transform.rotation.eulerAngles.x, 0, renderer.transform.rotation.eulerAngles.z));
                }
            }
        }
    }

    public abstract void OnPlayerStomp();

    public abstract void OnHitByFireball();

    protected void Init() {

        controller = GetComponent<Controller2D>();

        collider = GetComponent<BoxCollider2D>();

        renderer = GetComponentInChildren<SpriteRenderer>();

        CanMove = true;

        controller.collisionIgnoreConditions += IgnoreCollisions;

        controller.onCollision += CheckForTrigger;

        controller.onCollision += CheckCurrentCollider;
    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0) {

        return hit.transform.tag == "Trigger" || (hit.distance == 0 && hit.transform.tag == "Trigger" || hit.transform.tag == "Enemy");
    }

    protected override void CheckForTrigger(RaycastHit2D hit) {

        if (hit.transform.tag == "FallPoint") {

            FallTrigger fallTrigger = hit.transform.GetComponent<FallTrigger>();

            fallTrigger.OnTrigger(controller);
        }
    }

    protected IEnumerator WaitForAnimation() {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyDeath")) {

            isAlive = false;

            animator.SetBool("Alive", false);

            animator.SetBool("FireballHit", true);

            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(animationTime * 2);

            gameObject.SetActive(false);

        } else {

            yield return null;
        }
    }
}
