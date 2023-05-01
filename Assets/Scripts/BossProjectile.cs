using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Player playerScript;
    [SerializeField] int damage;
    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerScript.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
