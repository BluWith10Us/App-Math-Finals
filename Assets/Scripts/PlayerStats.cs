using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int lives = 3;
    public bool isInvincible = false;

    // FIREBALL ABILITY
    public bool canShootFireball = false;
    public float fireballCooldown = 0.5f;
    float fireTimer;

    float invTimer;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // INVINCIBILITY TIMER
        if (isInvincible)
        {
            invTimer -= Time.deltaTime;
            if (invTimer <= 0)
            {
                isInvincible = false;
            }
        }

        // FIREBALL INPUT
        if (canShootFireball)
        {
            fireTimer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.F) && fireTimer <= 0)
            {
                FireballManager.Instance.SpawnFireball();
                fireTimer = fireballCooldown;
            }
        }
    }

    public void AddLife(int amount)
    {
        lives += amount;
        Debug.Log("Lives: " + lives);
    }

    public void EnableInvincibility(float time)
    {
        isInvincible = true;
        invTimer = time;
        Debug.Log("INVINCIBLE!");
    }
}
