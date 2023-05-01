using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Basic elements of enemy characters
    [HideInInspector] public Transform player;
    public int health;
    public int currentHealth;
    public float speed;
    public float timeBetweenAttacks;
    public int damage;

    //Drop chance
    public int pickupChance;
    public GameObject[] pickups;

    public GameObject deathEffect;
    public GameObject onDeathSpawn;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = health;
    }
    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            int randomNumber = Random.Range(0, 101);
            if(randomNumber < pickupChance)
            {
                GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
                Instantiate(randomPickup, transform.position, transform.rotation);
            }

            Instantiate(deathEffect, transform.position, transform.rotation);
            Instantiate(onDeathSpawn, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}