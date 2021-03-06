﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : Platform {

    public override bool AllowedToJumpThrough(float direction) {

        bool allowed = platformType == PlatformType.CanJumpThrough && direction == 1;

        return allowed;
    }

    public override bool CanFallThrough() {
        return true;
    }

    public override void OnPlayerHit(bool destroy = false) {
        throw new System.NotImplementedException();
    }
}
