using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public Transform cameraTransform;
    public PlayerAttributes playerAttributes;
    public PlayerActions playerAction;
    public AudioSource walkAudioSource;
    public AudioSource _audioSource;
    public LibrarySounds _actionSounds;

    private Vector2 movementInput;
    public Rigidbody rb;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private Vector3 standingColliderCenter;
    private float standingColliderHeight;

    private PlayerState playerState;
    private bool canAttack = true;
    private bool isRolling = false;
    public bool isAttacking = false;
    public bool isDead = false;
    private enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Crouching,
        Rolling,
        Attacking,
        CrouchingAttack,
        Taunting,
        Unequip,
        Rage
    }

    void Awake()
    {
        playerAttributes.Stamina = 100f;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isDead = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        standingColliderCenter = capsuleCollider.center;
        standingColliderHeight = capsuleCollider.height;
        playerState = PlayerState.Idle;
        playerAction = GetComponent<PlayerActions>();
        walkAudioSource = GetComponents<AudioSource>()[1];
        _audioSource = GetComponents<AudioSource>()[0];
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            IncreaseLife(Time.deltaTime * 0.5f);
            HandleMovement();
            UpdateAnimation();
            AudioMovement();
        }
    }

    public void AudioMovement()
    {
        if (movementInput.magnitude > 0 && !isAttacking && !isRolling)
        {
            if (playerState == PlayerState.Running)
            {
                walkAudioSource.clip = _actionSounds.clipSounds[4];
            }
            else
            {
                walkAudioSource.clip = _actionSounds.clipSounds[3];
            }

            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead) return;
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (playerState != PlayerState.Crouching && playerState != PlayerState.Attacking && playerState != PlayerState.Rage && !isAttacking)
        {
            if (context.started && (movementInput.x != 0 || movementInput.y != 0) && playerAttributes.Stamina > 0)
            {
                playerState = PlayerState.Running;
            }
            else if (context.canceled)
            {
                playerState = PlayerState.Walking;
            }
        }
    }

    public void OnRolling(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (playerState != PlayerState.Crouching && playerAttributes.Stamina > 5 && !isAttacking)
        {
            if (context.started && playerState == PlayerState.Running)
            {
                canAttack = false;
                StartCoroutine(Roll());
            }
        }
    }

    public void OnCrouched(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.started && !isAttacking)
        {
            ToggleCrouch();
        }
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.performed && canAttack && playerState != PlayerState.Rolling)
        {
            if (playerAttributes.Stamina > 10 && playerState != PlayerState.Crouching && playerState != PlayerState.CrouchingAttack)
            {
                StartCoroutine(SecondaryAttack());
            }
        }
    }

    public void OnAttackStand(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.performed && canAttack && playerState != PlayerState.Rolling)
        {
            if (playerState == PlayerState.Crouching && playerAttributes.Stamina > 5)
            {
                StartCoroutine(CrouchAttack());
            }
            else if (playerState != PlayerState.Crouching && playerState != PlayerState.Rolling && playerAttributes.Stamina > 5)
            {
                StartCoroutine(Attack());
            }
        }
    }

    public void RageMode(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (playerState != PlayerState.Crouching && playerAction.GetEnemyKillCount() >= 10 && !isAttacking)
        {
            if (context.performed && !playerAction.rage)
            {
                _audioSource.PlayOneShot(_actionSounds.clipSounds[0]);
                playerAction.TriggerRage();
                StartCoroutine(Rage());
            }
        }
    }

    public void Taunt(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (playerState == PlayerState.Idle)
        {
            if (context.performed)
            {
                StartCoroutine(Taunt());
            }
        }
    }

    private IEnumerator Roll()
    {
        _audioSource.PlayOneShot(_actionSounds.clipSounds[Random.Range(1, 3)]);
        playerState = PlayerState.Rolling;
        isRolling = true;
        DecreaseStamina(5);
        animator.SetBool("IsRolling", true);
        rb.AddForce(transform.forward * playerAttributes.rollForce, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);
        canAttack = true;
        animator.SetBool("IsRolling", false);
        playerState = movementInput != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;
        isRolling = false;
        canAttack = true;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        playerState = PlayerState.Attacking;
        DecreaseStamina(5);
        canAttack = false;
        animator.SetBool("Attack1", true);

        yield return new WaitForSeconds(0.95f);

        animator.SetBool("Attack1", false);
        playerState = movementInput != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;
        canAttack = true;
        isAttacking = false;
    }

    private IEnumerator SecondaryAttack()
    {
        isAttacking = true;
        playerState = PlayerState.Attacking;
        DecreaseStamina(8);
        canAttack = false;
        animator.SetBool("Attack2", true);

        yield return new WaitForSeconds(1.2f);

        animator.SetBool("Attack2", false);
        playerState = movementInput != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;
        canAttack = true;
        isAttacking = false;
    }

    private IEnumerator CrouchAttack()
    {
        isAttacking = true;
        playerState = PlayerState.CrouchingAttack;
        DecreaseStamina(5);
        canAttack = false;
        animator.SetBool("AttackCrouch", true);

        yield return new WaitForSeconds(0.8f);

        animator.SetBool("AttackCrouch", false);
        playerState = PlayerState.Crouching;
        canAttack = true;
        isAttacking = false;
    }

    private IEnumerator Rage()
    {
        playerState = PlayerState.Rage;
        animator.SetBool("RageMode", true);

        yield return new WaitForSeconds(2.5f);

        animator.SetBool("RageMode", false);
        playerState = movementInput != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;
    }

    private IEnumerator Taunt()
    {
        playerState = PlayerState.Taunting;
        animator.SetBool("Taunt", true);

        yield return new WaitForSeconds(2f);

        animator.SetBool("Taunt", false);
        playerState = movementInput != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;
    }

    private void HandleMovement()
    {
        if (isDead || playerState == PlayerState.Rage || playerState == PlayerState.Taunting || playerState == PlayerState.Rolling || isRolling)
        {
            return;
        }
        else if (playerState == PlayerState.Attacking || playerState == PlayerState.CrouchingAttack)
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
            rb.constraints |= RigidbodyConstraints.FreezePositionX;
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * movementInput.y + right * movementInput.x;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.18f);

            if (playerState == PlayerState.Running)
            {
                if (playerAttributes.Stamina > 0 && !playerAction.inRageMode)
                {
                    DecreaseStamina(0.05f);
                    playerAttributes.currentSpeed += playerAttributes.acceleration * Time.fixedDeltaTime;
                    playerAttributes.currentSpeed = Mathf.Clamp(playerAttributes.currentSpeed, 0f, playerAttributes.maxSpeed);
                }
                else if (playerAction.inRageMode)
                {
                    playerAttributes.currentSpeed = playerAttributes.runSpeed;
                }
                else
                {
                    playerState = PlayerState.Walking;
                    playerAttributes.currentSpeed = playerAttributes.walkSpeed;
                }
            }
            else if (playerState == PlayerState.Crouching)
            {
                playerAttributes.currentSpeed = playerAttributes.crouchSpeed;
                IncreaseStamina(Time.deltaTime * 5f);
            }
            else
            {
                playerAttributes.currentSpeed = playerAttributes.walkSpeed;
                IncreaseStamina(Time.deltaTime * 2f);
            }

            Vector3 velocity = moveDirection * playerAttributes.currentSpeed;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }
        else
        {
            IncreaseStamina(Time.deltaTime * 5f);          

            if (playerState == PlayerState.Walking || playerState == PlayerState.Running)
            {
                playerState = PlayerState.Idle;
            }

            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void UpdateAnimation()
    {
        if (isDead)
        {
            ResetAllAnimations();
            return;
        }

        animator.SetFloat("X", movementInput.x);
        animator.SetFloat("Y", movementInput.y);
        animator.SetBool("IsRunning", playerState == PlayerState.Running);
    }

    public bool ChangeWeapon()
    {
        if (playerState == PlayerState.Idle && movementInput == Vector2.zero)
        {
            StartCoroutine(Unequip());
            return true;
        }
        return false;
    }

    private IEnumerator Unequip()
    {
        playerState = PlayerState.Unequip;
        animator.SetBool("Unequip", true);

        yield return new WaitForSeconds(0.3f);

        playerState = PlayerState.Idle;
        animator.SetBool("Unequip", false);

        yield return new WaitForSeconds(0.5f);
    }

    private void ToggleCrouch()
    {
        if (playerState == PlayerState.Crouching)
        {
            if (!Physics.Raycast(transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2f), Vector3.up, 2f))
            {
                playerState = PlayerState.Idle;
                animator.SetBool("IsCrouched", false);
                capsuleCollider.height = standingColliderHeight;
                capsuleCollider.center = standingColliderCenter;
            }
        }
        else
        {
            playerState = PlayerState.Crouching;
            animator.SetBool("IsCrouched", true);
            capsuleCollider.height = standingColliderHeight / 1.5f;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, capsuleCollider.center.y / 1.5f, capsuleCollider.center.z);
        }
    }

    public void PlayerDead()
    {
        isDead = true;
        _audioSource.PlayOneShot(_actionSounds.clipSounds[5]);
        animator.SetTrigger("Dead");
        rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        rb.constraints |= RigidbodyConstraints.FreezePositionX;
        rb.velocity = Vector3.zero;
        walkAudioSource.Stop();
        StopAllCoroutines();
        ResetAllAnimations();
    }

    private void ResetAllAnimations()
    {
        animator.SetBool("IsRolling", false);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("AttackCrouch", false);
        animator.SetBool("RageMode", false);
        animator.SetBool("Taunt", false);
        animator.SetBool("IsCrouched", false);
        animator.SetBool("Unequip", false);
        animator.SetBool("IsRunning", false); 
        animator.SetFloat("X", 0);
        animator.SetFloat("Y", 0);
    }

    private void IncreaseStamina(float amount)
    {
        playerAttributes.Stamina = Mathf.Min(playerAttributes.Stamina + amount, 100);
    }

    private void DecreaseStamina(float amount)
    {
        playerAttributes.Stamina = Mathf.Max(playerAttributes.Stamina - amount, 0);
    }
    private void IncreaseLife(float amount)
    {
        playerAction.currentHP = Mathf.Min(playerAction.currentHP + amount, 200);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rb.constraints = rb.constraints & ~RigidbodyConstraints.FreezePositionY;

        }
    }

}
