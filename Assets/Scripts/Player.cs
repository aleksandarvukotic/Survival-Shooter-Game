using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int health;

    private Rigidbody2D rb;
    private Vector2 moveAmount;
    private Animator anim;

    public Image[] hearts;
    public Sprite fullHearts;
    public Sprite emptyHearts;

    public Animator hurtAnim;
    public SceneTransition sceneTransitions;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sceneTransitions = FindObjectOfType<SceneTransition>();
        //if (sceneTransitions == null)
        //{
        //    Debug.LogError("SceneTransition component not found in the scene.");
        //}
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAmount = moveInput.normalized * speed;

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        UpdateHealthUI(health);
        hurtAnim.SetTrigger("hurt");
        if (health <= 0)
        {
            Destroy(gameObject);
            sceneTransitions.LoadScene("Lose");
        }
    }

    void UpdateHealthUI(int currentHealth)
    {
        for (int i=0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHearts;
            }
            else
            {
                hearts[i].sprite = emptyHearts;
            }
        }
    }

    public void Heal(int healAmount)
    {
        if(health + healAmount > 6)
        {
            health = 6;
        }
        else
        {
            health += healAmount;
        }
        UpdateHealthUI(health);
    }
}
