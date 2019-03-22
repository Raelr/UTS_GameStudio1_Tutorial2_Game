using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    
    public void OnTrigger(Controller2D controller) {

        if (controller.transform.tag == "Player") {

            LevelManager.instance.OnPlayerKilled();

        } else if (controller.transform.tag == "Enemy") {

            controller.gameObject.SetActive(false);
        }
    }
}
