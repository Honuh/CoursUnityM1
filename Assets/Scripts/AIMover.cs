using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMover : MonoBehaviour
{
    [Tooltip("vitesse de déplacement"), Range(1, 15)]
    public float linearSpeed = 6;
    [Tooltip("vitesse de rotation"), Range(1, 15)]
    public float angularSpeed = 1;

    private Transform player;
    private Vector3 dirPlayer;
    private float angleAuJoueur;


    Animator anim;



    public float life = 100;


    // Start is called before the first frame update
    void Start()
    {
        GameObject goPlayer = GameObject.FindGameObjectWithTag("Player");
        player = goPlayer.transform;

    }

    // Update is called once per frame
    void Update()
    {
        dirPlayer = player.position - transform.position;
        dirPlayer = dirPlayer.normalized;
        angleAuJoueur = Vector3.SignedAngle(dirPlayer, transform.forward, transform.up);



    }

    void FixedUpdate()
    {


        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            /*if(rb.velocity.magnitude < linearSpeed)
             {rb.AddForce(transform.forward * -linearSpeed);}*/

            //detection du joueur, calcul de l'angle

            if (angleAuJoueur > 40)
            {
                rb.AddTorque(transform.up * -angularSpeed);

            }
            else if (angleAuJoueur < -40)
            {
                rb.AddTorque(transform.up * angularSpeed);
            }
            else
            {
                rb.velocity = new Vector3(transform.forward.x * linearSpeed, rb.velocity.y, transform.forward.z * linearSpeed);
            }


            /*rb.AddTorque(transform.up * -angularSpeed);

            if (rb.angularVelocity.magnitude < angularSpeed)
            {
                rb.AddTorque(transform.up * -angularSpeed);
            }*/
            Animator anim = GetComponent<Animator>();
            anim.SetFloat("Speed", rb.velocity.magnitude);

            if (life <= 0)
            {

                linearSpeed = 0;
                rb.velocity = Vector3.zero;
                angularSpeed = 0;
                anim.SetBool("mort", true);
                Destroy(gameObject, 2);





            }
        }

        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + dirPlayer);

    }
}
