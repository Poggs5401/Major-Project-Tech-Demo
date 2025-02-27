using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class Paladin : Enemy
{
    private Rigidbody2D rigidBody;

    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if(currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        
            changeAnim(temp - transform.position);
            rigidBody.MovePosition(temp);
            ChangeState(EnemyState.walk);
            anim.SetBool("wakeUp", true);
            } else if(Vector3.Distance(target.position, transform.position) > chaseRadius){
                anim.SetBool("wakeUp", false);
            }
        }
    }

    private void SetAnimFloat(Vector2 setVector){
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    private void changeAnim(Vector2 direction){
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0){
                SetAnimFloat(Vector2.right);
            } else if(direction.x < 0){
                SetAnimFloat(Vector2.left);

            }
        } else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)){
            if(direction.y > 0){
                SetAnimFloat(Vector2.up);
            } else if(direction.y < 0){
                SetAnimFloat(Vector2.down);
            }
        }
        }

    private void ChangeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }
}
