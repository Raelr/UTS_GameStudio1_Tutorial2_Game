using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlatformUser {

    public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
    public Status CurrentStatus { get { return status; }}

    public FireProjectile Projectile { get { return projectile; } set { projectile = value; } }

    public bool CanMove { get { return canMove; } set { canMove = value; } }

    [Header("Player Controller")]
    [SerializeField]
    Controller2D controller;

    Vector3 input;

    [SerializeField]
    Animator animator;

    bool isAlive;

<<<<<<< HEAD
    //status store the stage of powerup, 1 is normal, 2 is mashroomed, 4 is fire mode, 8 is invincible, 
    //16 is untouchable, which happen during status changing time, normally last for few seconds.
    //Flags means status value could be used as bit values, so that multiple values can be true at once.
    //For example, mario can have SizeUp/OnFire/OnInvincible all at one time.
    [Flags] public enum Status {Normal = 1, SizeUp = 2, OnFire = 4, OnInvincible = 8, OnUnTouchable = 16}
    private float timer;
    private Status status = Status.Normal;
=======
    bool canMove;

    //status store the stage of powerup, 1 is normal, 2 is mashroomed, 3 is fire mode, 4 is invincible.
    private int status = 1;
>>>>>>> 68b567973ef4f0b60a0d12b0669e03d167461661

    SpriteRenderer renderer;

    Vector3 facingDirection;

    [SerializeField]
    Vector3 projectileSpawnOffset;

    [SerializeField, ReadOnly]
    FireProjectile projectile;

    // Delegate for anything which needs to know whether the player is moving
    public delegate void PlayerMovedHandler();

    public event PlayerMovedHandler playerMoved;

    //SFX
    [SerializeField]
    AudioClip jumpSound, gameOverSound, stageClearSound, starSound, extraLifeSound, breakBlockSound, bumpSound,
        fireballSound, flagpoleSound, kickSound, pipeSound, mushroomSound, stompSound,
        dieSound;



    void Start() {

        isAlive = true;

        canMove = true;

        controller = GetComponent<Controller2D>();

        animator = GetComponent<Animator>();

        renderer = GetComponentInChildren<SpriteRenderer>();

        facingDirection = Vector2.right;

        controller.onCollision += CheckEnemyHit;

        controller.onCollision += CheckForPowerUp;

        controller.onCollision += CheckForTrigger;

        controller.onCollision += CheckCurrentCollider;

        controller.directionConditions += TriggerPlatformMove;

        controller.collisionIgnoreConditions += IgnoreCollisions;
    }

    void Update() {

        Tick();
        if (isAlive) {

            if (canMove) {

                MoveByInput();

                if (Input.GetKeyDown("f")) {

                    SpawnProjectile();
                }
            }
        }
    }

    /// <summary>
    /// for timer
    /// </summary>
    void Tick(){
        if (timer > 0){
            timer -= Time.deltaTime;
            if (timer <= 0){
                //This is bit operation. To set the status 0 on Status.OnUnTouchable bit.
                //Which means mario is no longer Untouchable.
                status &= ~Status.OnUnTouchable;
            }
        }
    }

    /// <summary>
    /// Maps player inputs into movement and calls appropriate commands from the controller.
    /// </summary>

    void MoveByInput() {

        // Get the current axis values and add them to a vector.
        FindAxes();

        UpdateFacingDirection(input);

        bool crouching;

        bool isJumping = false;

        if (Input.GetKey("s") || Input.GetKey("down")) {
            crouching = true;
        } else {
            crouching = false;
        }

        controller.Crouch(crouching);

        if (IsStill() && !SpacePressed()) {

            // If we arent moving then just apply gravity normally.
            controller.ApplyGravity(ref input, true);

            animator.SetBool("isWalking", false);

        } else {

            controller.ApplyGravity(ref input);

            if (SpacePressed()) {

                if (controller.Collisions.isBelow) {
                    controller.Jump(ref input);
                    SoundManager.instance.PlaySingle(jumpSound);
                }

            }

            if (controller.Collisions.isBelow && input.x != 0) {

                animator.SetBool("isWalking", true);

            }

            controller.ApplyMovement(input);
        }

        if (!controller.Collisions.isBelow) {

            isJumping = true;

        }

        animator.SetBool("isJumping", isJumping);



        // If anything is listening for player movement then invoke the delegate.
        if (playerMoved != null) {

            playerMoved.Invoke();
        }
    }

    void UpdateFacingDirection(Vector3 input) {

        float xValue = Mathf.RoundToInt(input.x);

        Vector3 direction = new Vector3(xValue, 0, 0);

        if (direction != Vector3.zero) {

            if (Utilities.VectorEquals(facingDirection, Vector3.right)) {

                if (renderer.transform.rotation.y != 0) {
                    renderer.transform.rotation = Quaternion.Euler(renderer.transform.rotation.x, 0, renderer.transform.rotation.z);
                    //Debug.Log(renderer.transform.rotation.y);
                }

            } else if (Utilities.VectorEquals(facingDirection, -Vector3.right)) {

                if (renderer.transform.rotation.y != 180) {
                    renderer.transform.rotation = Quaternion.Euler(renderer.transform.rotation.x, 180, renderer.transform.rotation.z);
                    //Debug.Log(renderer.transform.rotation.y);
                }
            }
            facingDirection = direction;
        }

    }

    /// <summary>
    /// Modifies the input vector to have the horizontal and vertical axes values.
    /// </summary>

    void FindAxes() {

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.z = 0;
    }

    bool SpacePressed() {
        return Input.GetKeyDown(KeyCode.Space);
    }

    bool IsStill() {

        return input.x == 0 && input.y == 0;
    }

    void SpawnProjectile() {

        if (status == 3 && projectile != null) {

            FireProjectile projectile = Instantiate(this.projectile, transform.position + facingDirection, Quaternion.identity);

            SoundManager.instance.PlaySingle(fireballSound);

            projectile.Direction = facingDirection;
        }
    }

    public void GetPowerUp(PowerUp.Abilities ability) {
        if (status == Status.Normal){
            SizeUp();
        }

        switch (ability) {
            case PowerUp.Abilities.Mashroom:
<<<<<<< HEAD
                break;
            case PowerUp.Abilities.Fire:
                //Bit operation. Means the bit on Status.OnFire is set to 1;
                status |= Status.OnFire;
                break;
            case PowerUp.Abilities.Invincible:
                status |= Status.OnInvincible;
=======
                status = 2;
                SoundManager.instance.PlaySingle(mushroomSound);
                break;
            case PowerUp.Abilities.Fire:
                SoundManager.instance.PlaySingle(mushroomSound);
                status = 3;
                break;
            case PowerUp.Abilities.Invincible:
                SoundManager.instance.PlaySingle(mushroomSound);
                status = 4;
>>>>>>> 68b567973ef4f0b60a0d12b0669e03d167461661
                break;
            default:
                break;
        }
    }

    public void OnDeath() {

        SoundManager.instance.PlaySingle(dieSound);
        SoundManager.instance.StopSound();

        isAlive = false;
<<<<<<< HEAD
        animator.SetBool("Alive", false);
        Debug.Log("I'm dead");
    }

    public void SizeUp(){
        float feety = transform.position.y - GetComponent<Collider2D>().bounds.size.y * 0.5f;
        transform.localScale = Vector3.one * 1.6f;
        transform.position = new Vector3(transform.position.x, GetComponent<Collider2D>().bounds.size.y * 0.5f + feety, transform.position.z);
        status = Status.SizeUp;
        ChangeStatus();
    }

    public void Shrink(){
        float feety = transform.position.y - GetComponent<Collider2D>().bounds.size.y * 0.5f;
        transform.localScale = Vector3.one;
        transform.position = new Vector3(transform.position.x, GetComponent<Collider2D>().bounds.size.y * 0.5f + feety, transform.position.z);
        status = Status.Normal;
        ChangeStatus();
    }

    public void ChangeStatus(){
        status |= Status.OnUnTouchable;
        timer = 2f;
    }

    public void OnHurt(){
        if (status != Status.Normal){
            Shrink();
            Debug.Log("I'm hurt");
        }else{
            OnDeath();
        }
=======

        if (LevelManager.instance.Lives <= 0) {

            StartCoroutine(LevelManager.instance.PlayAnimation(animator, "MarioDeath", "Alive", false, 8, LevelManager.instance.ReturnToMenu));
            SoundManager.instance.PlaySingle(gameOverSound);

        } else {

            StartCoroutine(LevelManager.instance.PlayAnimation(animator, "MarioDeath", "Alive", false, 10, LevelManager.instance.RestartLevel));
            SoundManager.instance.PlaySingle(dieSound);
        }
    }

    public void CheckEnemyHit(RaycastHit2D hit) {

        if (hit.transform.tag == "VulnerablePoint") {
            SoundManager.instance.PlaySingle(stompSound);

            Enemy enemy = hit.transform.parent.GetComponent<Enemy>();

            enemy.OnPlayerStomp();

            controller.Bounce();

        } else if (hit.transform.tag == "Enemy") {

            LevelManager.instance.OnPlayerKilled();
        }
    }

    void CheckForPowerUp(RaycastHit2D hit) {

        if (hit.transform.tag.Equals("PowerUp")) {


            PowerUp powerUp = hit.transform.GetComponent<PowerUp>();

            powerUp.OnPickup();

            GetPowerUp(powerUp.GetAbility());
        }
    }

    void TriggerPlatformMove() {

        if (controller.Collisions.isAbove) {
            currentPlatform.OnPlayerHit();
        }
    }

    protected override void CheckForTrigger(RaycastHit2D hit) {

        if (hit.transform.tag == "Trigger" && currentPlatformCollider != hit.collider) {

            Trigger trigger = hit.transform.GetComponent<Trigger>();

            trigger.OnPlayerEnter();

            currentPlatformCollider = hit.collider;

        } else if (hit.transform.tag == "FallPoint" && currentPlatformCollider != hit.collider) {

            FallTrigger trigger = hit.transform.GetComponent<FallTrigger>();

            trigger.OnTrigger(controller);

            currentPlatformCollider = hit.collider;

        } else if (hit.transform.tag == "FlagPole" && currentPlatformCollider != hit.collider) {

            LevelManager.instance.PlayEndAnimation();
            SoundManager.instance.PlayLoop(stageClearSound);
        } else if (hit.transform.tag == "End" && currentPlatformCollider != hit.collider) {

            LevelManager.instance.ReturnToMenu();
        }
    }

    protected override bool IgnoreCollisions(RaycastHit2D hit, float direction = 0) {

        bool success = false;

        if (currentPlatformCollider != null) {

            success = hit.transform.tag == "PowerUp" || currentPlatform.AllowedToJumpThrough(direction) || controller.IsCrouching && currentPlatform.CanFallThrough() || hit.transform.tag == "Trigger" || hit.transform.tag == "Enemy" || hit.transform.tag == "VulnerablePoint" || hit.transform.tag == "FlagPole"
                || hit.transform.tag == "End";
        }

        return success;
    }

    public void ChangeSprite(Sprite sprite) {


        animator.SetBool("isJumping", false);

        animator.SetBool("isWalking", false);

        renderer.sprite = sprite;
>>>>>>> 68b567973ef4f0b60a0d12b0669e03d167461661
    }
}
