using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health = 20;
    public Enemy[] enemies;
    public float spawnOffsets;
    public int damage = 2;

    private int halfHealth;
    private Animator anim;
    private Slider healthBar;

    public SceneTransition sceneTransitions;

    //random projectile variables
    public GameObject projectilePrefab;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileLifeTime;
    [SerializeField] float fireTimer;
    [SerializeField] float fireInterval;
    [SerializeField] int numProjectiles;

    private void Start()
    {
        halfHealth = health / 2;
        anim = GetComponent<Animator>();
        healthBar = FindObjectOfType<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        sceneTransitions = FindObjectOfType<SceneTransition>();
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthBar.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
            healthBar.gameObject.SetActive(false);
            sceneTransitions.LoadScene("Win");
        }

        if(health <= halfHealth)
        {
            //chase behaviour
        }

        Enemy randomEnemy = enemies[Random.Range(0, enemies.Length)];
        Instantiate(randomEnemy, transform.position + new Vector3(spawnOffsets, spawnOffsets, 0), transform.rotation);
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        if(fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(damage);
        }
    }

    void Fire()
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);

            Destroy(projectile, projectileLifeTime);
        }
    }
}
