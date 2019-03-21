using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba_Corpse : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer renderer;

    [SerializeField]
    Sprite sprite;

    Controller2D controller;

    Vector3 direction;

    [SerializeField]
    BoxCollider2D collider;

    private void Awake() {

        renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = sprite;

        controller = GetComponent<Controller2D>();

        direction = Vector3.down;

        collider = GetComponent<BoxCollider2D>();

        collider.bounds.SetMinMax(sprite.bounds.min, sprite.bounds.max);

        StartCoroutine(WaitBeforeDestroy());
    }

    private void Update() {

        controller.ApplyGravity(ref direction, true);
    }

    IEnumerator WaitBeforeDestroy() {

        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }
}
