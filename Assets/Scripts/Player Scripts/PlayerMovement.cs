using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public enum PlayerState
{
    walk, 
    attack, 
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D playerRigidBody;
    private Vector3 positionChange;
    private Animator animator;
    public FloatValue currentHealth;
    public SignalObject playerHealthSignal;
    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        positionChange = Vector3.zero;
        positionChange.x = Input.GetAxisRaw("Horizontal");
        positionChange.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        } 
        
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }

    }

private IEnumerator AttackCo()
{
    animator.SetBool("attacking", true);
    currentState = PlayerState.attack;
    yield return null;
    animator.SetBool("attacking", false);
    yield return new WaitForSeconds(0.3f);
    currentState = PlayerState.walk;
}

    void UpdateAnimationAndMove()
    {
        if (positionChange != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", positionChange.x);
            animator.SetFloat("moveY", positionChange.y);
            animator.SetBool("moving", true);
        } else {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        positionChange.Normalize();
        playerRigidBody.MovePosition(transform.position + positionChange * speed * Time.deltaTime);
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if(currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }else{
            this.gameObject.SetActive(false);
        }
        
    }

        private IEnumerator KnockCo(float knockTime)
    {
        if(playerRigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            playerRigidBody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            playerRigidBody.velocity = Vector2.zero;
        }
    }
}
