using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Knockback : MonoBehaviour
{

    [Header("Audio Sources")]
    public AudioSource playerTakeDamage;

    [Header("Knockback Values")]
    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Pot>().Destroy();
        }

        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if(other.gameObject.CompareTag("enemy") && other.isTrigger)
                {
                hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                }

                if(other.gameObject.CompareTag("Player"))
                {
                    if(other.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                    {
                        hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                        other.GetComponent<PlayerMovement>().Knock(knockTime, damage);

                        if (playerTakeDamage != null)
                        {   
                            playerTakeDamage.Play();
                        }
                    }
                    
                }


            }
        }
    }
}
