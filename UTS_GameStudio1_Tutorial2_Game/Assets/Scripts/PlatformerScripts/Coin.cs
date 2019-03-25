using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start() {
        
    }
    
    void Update() {
        
    }

    private void OnTrigger() {
        Destroy(this);
        LevelManager.instance.pickUpCoin();
    }
}
