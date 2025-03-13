using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{

    [Header("State")]
    public EnemyState currentState;

    [Header("Enemy Stats")]
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public Vector2 homePosition;

    [Header("Death Effects")]
    public GameObject deathEffect;
    public Animator anim;

    [Header("Rigid Body 2D")]
    public Rigidbody2D rigidBody;

    [Header("Death Signals")]
    public SignalObject roomSignal;

    private void Awake()
    {
        health = maxHealth.initialValue;
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();        
    }

    private void TakeDamage(float damage){
        health -= damage;
        if(health <= 0)
        {
            DeathEffect();
            roomSignal.Raise();
        }
    }

    private void DeathEffect(){
        if (anim != null)
        {
            anim.SetFloat("moveX", rigidBody.velocity.x);
            anim.SetFloat("moveY", rigidBody.velocity.y);
            anim.SetTrigger("Death");

            GetComponent<Collider2D>().enabled = false;
            rigidBody.velocity = Vector2.zero;

            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    public void Knock(Rigidbody2D rigidBody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(rigidBody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D rigidBody, float knockTime)
    {
        if(rigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            rigidBody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            rigidBody.velocity = Vector2.zero;
        }
    }
}
