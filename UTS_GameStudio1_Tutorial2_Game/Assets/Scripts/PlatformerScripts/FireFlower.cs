using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireFlower : PowerUp {

    [SerializeField]
    FireProjectile projectile;

    // Start is called before the first frame update
    void Awake() {

        InitialiseAnimation();

        ability = Abilities.Fire;

        controller = GetComponent<Controller2D>();

        controller.onCollision += CheckCurrentCollider;

    }

    protected override void CheckForTrigger(RaycastHit2D hit) {
        
    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0) {

        return false;
    }

    public override void OnPickup() {

        Debug.Log("Picked up fire flower!");

        LevelManager.instance.SetPlayerProjectile(projectile);

        Destroy(gameObject);
    }
}