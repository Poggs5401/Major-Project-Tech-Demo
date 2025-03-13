using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Powerup
{
    [Header("Audio Sources")]
    public AudioSource heartPickup;

    [Header("Heart Stats")]
    public FloatValue playerHealth;
    public float healAmount;
    public FloatValue heartContainers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {

            playerHealth.RuntimeValue += healAmount;
            if(playerHealth.initialValue > heartContainers.RuntimeValue * 2f)
            {
                playerHealth.initialValue = heartContainers.RuntimeValue * 2f;
            }
            powerupSignal.Raise();

            if (heartPickup != null)
            {   
                heartPickup.Play();
            }         
              
            Destroy(this.gameObject);
        }
    }
}
