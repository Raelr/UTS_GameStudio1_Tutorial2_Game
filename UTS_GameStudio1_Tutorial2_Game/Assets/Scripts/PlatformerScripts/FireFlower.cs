using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlower : PowerUp {
    // Start is called before the first frame update
    void Awake() {
        InitialiseAnimation();
        ability = Abilities.Fire;
    }

    private void Update() {

    }
}