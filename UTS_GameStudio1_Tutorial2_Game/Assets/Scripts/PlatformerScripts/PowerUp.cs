using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;

    protected bool isInitialising;

    public enum Abilities {Mashroom, Fire, Invincible}
    protected Abilities ability;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected void InitialiseAnimation() {

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation() {

        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MushroomRisingAnimation")) {

        //    isInitialising = true;

        //    animator.SetBool("Instantiated", true);

        //    float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

        //    yield return new WaitForSeconds(animationTime);

        //    isInitialising = false;

        //    animator.SetBool("Instantiated", false);

        //} else {

            yield return null;
        //}
    }

    public void OnPickup() {

        Debug.Log("Picked Up!");
        Destroy(this.gameObject);

    }

    public Abilities GetAbility(){
        return ability;
    }
}
