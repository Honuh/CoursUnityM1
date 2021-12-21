using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{

    public Transform cam,muzzle;
    public Transform bullet;
    public GameObject[] particles = new GameObject[0];
    
    public float delayBulletMax = 0.5f;



    private Vector3 shotPoint,dirTir;
    private float delayBullet;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("fire", false);

        //Direction du Tir
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward * 20, out hit, Mathf.Infinity))
        {
            shotPoint = hit.point;
            dirTir = hit.point - muzzle.position;
            dirTir = dirTir.normalized;
            float shotDist = hit.distance;

        }
        else
        {
            dirTir = cam.forward * 50 - muzzle.position;
            dirTir = dirTir.normalized;
        }


        //TIR
        if (Input.GetButton("Fire1") && delayBullet >= delayBulletMax)
        {
            for(int i=0; i < particles.Length; i++)
            {
                particles[i].GetComponent<ParticleSystem>().Stop(); 
            }

            Transform balle = GameObject.Instantiate<Transform>(bullet).transform;
            balle.position = muzzle.transform.position;
            balle.rotation = muzzle.transform.rotation;

            balle.GetComponent<Rigidbody>().AddForce(dirTir * 20, ForceMode.Impulse);
            delayBullet = 0f;
            anim.SetBool("fire", true);
            anim.Play("shoot");

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].GetComponent<ParticleSystem>().Play();
            }



        }
        delayBullet += Time.deltaTime;

        if (Input.GetButtonUp("Fire1"))
        {
            delayBullet = 1f;
        }
    }
}
