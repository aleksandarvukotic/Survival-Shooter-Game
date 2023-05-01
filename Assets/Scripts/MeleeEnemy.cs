using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    //Basic enemy mechanics
    [SerializeField] float stopDistance;
    float attackTime;
    public float attackSpeed;

    //Variables for blink mechanics
    [SerializeField] float teleportInterval = 6f;
    [SerializeField] float minDistance = 1f;
    [SerializeField] float maxDistance = 3f;
    float teleportTimer;
    SpriteRenderer spriteRenderer;

    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(TeleportAroundPlayer());
    }

    private void Update()
    {
        if(player != null)
        {
            if(Vector2.Distance(transform.position, player.position) > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else
            {
                if(Time.time >= attackTime)
                {
                    StartCoroutine(Attack());
                    attackTime = Time.time + timeBetweenAttacks;
                }
            }
        }

        teleportTimer += Time.deltaTime;
        if (Time.time >= teleportTimer)
        {
            StartCoroutine(TeleportAroundPlayer());
            teleportTimer = Time.time + teleportInterval;
        }
    }

    IEnumerator Attack()
    {
        player.GetComponent<Player>().TakeDamage(damage);

        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = player.position;

        float percent = 0;
        while(percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
            yield return null;
        }
    }

    IEnumerator TeleportAroundPlayer()
    {
        while (true)
        {
            // Fade out the enemy, t is set to 0.5f so that fade in or out time is 0.5sec (1sec is too much)
            float t = 0.5f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                Color color = spriteRenderer.color;
                color.a = 1f - t;
                spriteRenderer.color = color;
                yield return null;
            }

            // Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            float distance = Mathf.Clamp(distanceToPlayer, minDistance, maxDistance);

            // Teleport around the player
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector2 teleportPosition = (Vector2)player.position + randomDirection * distance;
            transform.position = teleportPosition;

            //Fade in the enemy, t is set to 0.5f so that fade in or out time is 0.5sec (1sec is too much)
            t = 0.5f;
            while(t < 1f)
            {
                t += Time.deltaTime;
                Color color = spriteRenderer.color;
                color.a = t;
                spriteRenderer.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(teleportInterval);
        }
    }

    //Overrided method to include Teleportation mechanics
    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
        StartCoroutine(TeleportAroundPlayer());
    }
}