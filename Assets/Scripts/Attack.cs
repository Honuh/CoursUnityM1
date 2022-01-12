using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public float puissance = 5;
    private void OnTriggerEnter(Collider collision)
    {
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.currentLife -= puissance;

            }

        }
    }
}
