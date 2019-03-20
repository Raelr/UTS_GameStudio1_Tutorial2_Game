using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour {

    SpriteRenderer renderer;

    public bool CanJumpThrough { get; set; }

    [SerializeField]
    protected Animator animator;

    protected enum PlatformType {

        Solid,
        CanJumpThrough
    }

    [SerializeField]
    protected PlatformType platformType;

    private void Start() {

        renderer = GetComponent<SpriteRenderer>();


    }

    public abstract bool AllowedToJumpThrough(float direction, bool isCheckingDirection = false);

    public abstract bool CanFallThrough();

    public abstract void OnPlayerHit(bool destroy = false);
}
