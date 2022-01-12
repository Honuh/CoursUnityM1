using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class farmerAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator anim;
    public float attackRange = 1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);

        if (agent.velocity != Vector3.zero)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }


        anim.SetBool("attack", false);

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            anim.SetBool("attack", true);
            //Attack(goPlayer, 10);
        }


        

        Debug.Log(agent.velocity);
        
    }
}
