using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            //
            if (Input.GetButton("Fire1"))
            {
                if (rb.velocity.magnitude < 5)
                    rb.AddForce(transform.forward * -30);
            }
            

            Animator anim = GetComponent<Animator>();
            if(anim != null)
            {
                anim.SetFloat("Speed", rb.velocity.magnitude);
            }

        }
    }
}
