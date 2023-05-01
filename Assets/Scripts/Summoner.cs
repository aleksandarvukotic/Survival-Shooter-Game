using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Enemy
{
    //min-max of random position on X and Y
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    Vector2 targetPosition;
    Animator anim;

    //Summon variables
    float timer;
    [SerializeField] float interval;
    public Enemy enemyToSummon;

    //Attack variables
    [SerializeField] float attackSpeed;
    [SerializeField] float stopDistance;
    [SerializeField] float attackTime;

    public override void Start()
    {
        base.Start();
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        targetPosition = new Vector2(randomX, randomY);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Setting animations for run-idle, and running Summon method if condition is met
        if (player != null)
        {
            if(Vector2.Distance(transform.position, targetPosition) > 0.5f)
            {
                anim.SetBool("isRunning", true);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("isRunning", false);

                timer += Time.deltaTime;
                if (timer >= interval)
                {
                    Summon();
                    timer = 0f;
                }
            }

            //Summoners attack if player gets close
            if (Vector2.Distance(transform.position, player.position) < stopDistance)
            {
                if (Time.time >= attackTime)
                {
                    StartCoroutine(Attack());
                    attackTime = Time.time + timeBetweenAttacks;
                }
            }
        }
    }

    public void Summon()
    {
        if(player != null)
        {
            Instantiate(enemyToSummon, transform.position, transform.rotation);
        }
    }

    IEnumerator Attack()
    {
        player.GetComponent<Player>().TakeDamage(damage);

        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = player.position;

        float percent = 0;
        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
            yield return null;
        }
    }

}