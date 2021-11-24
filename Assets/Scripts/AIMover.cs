using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMover : MonoBehaviour
{
    [Tooltip("vitesse de déplacement"), Range(1,15)]
    public float linearSpeed = 6;
    [Tooltip("vitesse de rotation"), Range(1, 15)]
    public float angularSpeed = 1;

    private Transform player;


    // Start is called before the first frame update
    void Start()
    {
        GameObject goPlayer = GameObject.FindGameObjectWithTag("Player");
        player = goPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
        
        Rigidbody rb = GetComponent<Rigidbody>();
        
        if (rb != null) 
        {
           /*if(rb.velocity.magnitude < linearSpeed)
            {rb.AddForce(transform.forward * -linearSpeed);}*/

            if(rb.angularVelocity.magnitude < angularSpeed)
            {
                rb.AddTorque(transform.up * -angularSpeed);
            }
            

            Animator anim = GetComponent<Animator>();
            if(anim != null)
            {
                anim.SetFloat("Speed", rb.velocity.magnitude);
            }

        }
    }


}
