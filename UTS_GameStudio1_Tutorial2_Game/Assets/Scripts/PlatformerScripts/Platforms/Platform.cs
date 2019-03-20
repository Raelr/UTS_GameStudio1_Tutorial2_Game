using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Platform : MonoBehaviour {

    protected SpriteRenderer renderer;

    public bool CanJumpThrough { get; set; }

    [Header("Animator")]
    [SerializeField]
    protected Animator animator;

    [Header("Event for block hit")]
    public UnityEvent onHit;

    private void Awake() {

        if (onHit == null) {
            onHit = new UnityEvent();
        }
    }

    protected enum PlatformType {

        Solid,
        CanJumpThrough
    }

    [SerializeField]
    protected PlatformType platformType;

    public abstract bool AllowedToJumpThrough(float direction, bool isCheckingDirection = false);

    public abstract bool CanFallThrough();

    public abstract void OnPlayerHit(bool destroy = false);
}
