using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float puissance = 30;
    public ParticleSystem particle;
    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }
    public void OnCollisionEnter(Collision collision)
    {
        
        particle.Play();
        AIMover other = collision.gameObject.GetComponent<AIMover>();
        if(other != null)
        {
            other.life -= puissance;
            
        }

        StartCoroutine(ParticleCoroutine());
        
    }

    IEnumerator ParticleCoroutine()
    {
        

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
