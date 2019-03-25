using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //SFX
    [SerializeField]
    AudioClip coinSound;

    private void OnTrigger() {
        SoundManager.instance.PlaySingle(coinSound);
        LevelManager.instance.pickUpCoin();
        Destroy(this);      
        
    }
}
