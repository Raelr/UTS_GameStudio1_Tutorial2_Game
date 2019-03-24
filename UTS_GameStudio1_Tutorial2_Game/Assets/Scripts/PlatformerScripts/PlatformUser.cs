using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformUser : MonoBehaviour
{
    protected Collider2D currentPlatformCollider;

    protected Platform currentPlatform;

    protected void CheckCurrentCollider(RaycastHit2D hit) {

        if (hit.transform.tag == "Platform" && hit.collider != currentPlatformCollider) {

            currentPlatformCollider = hit.collider;
            Platform platform = hit.transform.GetComponent<Platform>();
            currentPlatform = platform == null ? null : platform;
        }
    }

    protected abstract void CheckForTrigger(RaycastHit2D hit);

    protected abstract bool IgnoreCollisions(RaycastHit2D hit, float direction = 0);
}
