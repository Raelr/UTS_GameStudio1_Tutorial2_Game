using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    [SerializeField]
    Goomba_Corpse corpse;

    [SerializeField]
    Vector3 corpseOffset;

    // Start is called before the first frame update
    void Awake()
    {
        Init();

        direction = -Vector3.right;

        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) {

            if (canMove) {
                Move();
                //UpdateFacingDirection(direction);
            }
        }
    }

    public override void OnPlayerStomp() {

        isAlive = false;

        Vector3 spawnPosition = transform.position + corpseOffset;

        Instantiate(corpse, spawnPosition, Quaternion.identity);

        this.gameObject.SetActive(false);
    }

    public override void OnHitByFireball() {

        StartCoroutine(WaitForAnimation());
    }
}
