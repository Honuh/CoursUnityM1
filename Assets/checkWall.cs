using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkWall : MonoBehaviour
{
    public bool wallColliding;
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag =="ground")
        {
            wallColliding = true;
            
            
        }
    }
}
