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
    public AnimationClip deathAnimation;
    public Animator anim;

    [Header("Audio Sources")]
    public AudioSource enemyDeath;

    [Header("Rigid Body 2D")]
    public Rigidbody2D rigidBody;

    [Header("Death Signals")]
    public SignalObject roomSignal;

    private float deathAnimationDuration = 0.5f;

    private void Awake()
    {
        health = maxHealth.initialValue;
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Get the Animator component
        if(anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }        
    }

    private void TakeDamage(float damage){
        health -= damage;
        if(health <= 0)
        {
            DeathEffect();
        }
    }

    private void DeathEffect(){

        if (anim != null)
        {
            anim.SetTrigger("Death");

            if (enemyDeath != null)
            {   
                enemyDeath.Play();
            }

            StartCoroutine(DeactivateAfterAnimation());
        }
    }

    IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        this.gameObject.SetActive(false);
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
