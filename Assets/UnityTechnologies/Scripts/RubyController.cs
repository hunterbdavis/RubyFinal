using System;
using System.Collections;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // ========= MOVEMENT =================
    public float speed = 4;
    float speedMultiplier = 1f;

    // ======== HEALTH ==========
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public Transform respawnPosition;
    public ParticleSystem hitParticle;
    public ParticleSystem healParticle;

    // ======== PROJECTILE ==========
    public GameObject projectilePrefab;
    public GameObject scoreText;
    public int score;

    // ======== AUDIO ==========
    public AudioClip hitSound;
    public AudioClip shootingSound;
    public AudioClip talkSound;
    public AudioClip winSound;

    // ======== HEALTH ==========
    public int health
    {
        get { return currentHealth; }
    }

    // =========== MOVEMENT ==============
    Rigidbody2D rigidbody2d;
    Vector2 currentInput;

    // ======== HEALTH ==========
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;

    // ==== ANIMATION =====
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // ================= SOUNDS =======================
    AudioSource audioSource;

    void Start()
    {
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
        SpeedCollectible.OnSpeedCollected += StartSpeedBoost;

        // ======== HEALTH ==========
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;

        // ==== ANIMATION =====
        animator = GetComponent<Animator>();

        // ==== AUDIO =====
        audioSource = GetComponent<AudioSource>();
    }

    void StartSpeedBoost(float multiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        Instantiate(healParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(3f);
        speedMultiplier = 1f;
    }

    void Update()
    {
        // ================= HEALTH ====================
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if (currentHealth <= 0)
        {
            Respawn();
            audioSource.PlayOneShot(winSound);
        }

        // ============== MOVEMENT ======================
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        currentInput = move;

        // ============== ANIMATION =======================

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // ============== PROJECTILE ======================

        if (Input.GetKeyDown(KeyCode.C))
            LaunchProjectile();

        // ======== DIALOGUE ==========
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, 1 << LayerMask.NameToLayer("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                    audioSource.PlayOneShot(talkSound);
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        if (currentHealth > 0)
        {
            position = position + currentInput * speed * speedMultiplier * Time.deltaTime;
        }

        rigidbody2d.MovePosition(position);
    }

    // ===================== HEALTH ==================
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound);

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        if (amount > 0)
        {
            Instantiate(healParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
            Respawn();

        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Respawn()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeHealth(maxHealth);
            transform.position = respawnPosition.position;
        }
    }

    // =============== PROJECTICLE ========================
    void LaunchProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        audioSource.PlayOneShot(shootingSound);
    }

    public void ChangeScore(int scoreAmount)
    {
        score = score + scoreAmount;
        Debug.Log("your current score is " + score);
    }

    // =============== SOUND ==========================

    //Allow to play a sound on the player sound source. used by Collectible
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}