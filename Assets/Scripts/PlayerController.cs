using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float angularSpeed = 1;

    public Transform cam;
    private bool isGrounded;
    



    // Start is called before the first frame update
    void Start()
    {
        if(cam == null)
        {
            cam = transform.GetComponentInChildren<Camera>().transform;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        float rotY = Input.GetAxis("Mouse Y") * -angularSpeed;
        float rotX = Input.GetAxis("Mouse X") * angularSpeed;

        //sauvegarde la rotation au tour d'avant
        Quaternion lastRotation = cam.rotation;

        //Rotation de la tête
        Quaternion rotaY = Quaternion.AngleAxis(rotY, cam.right);
        cam.rotation = rotaY * cam.rotation;

        //Check si on dépasse la limite de mouvement
        float regardeDevant = Vector3.Dot(cam.forward, transform.forward);
        if (regardeDevant < 0)
        {
            //si on dépasse, on reprend la position précédente
            cam.rotation = lastRotation;
        }

        //Rotation du corps
        Quaternion rotationX = Quaternion.AngleAxis(rotX, transform.up);
        transform.rotation = rotationX * transform.rotation;

    }

    private void FixedUpdate()
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 vert = Input.GetAxis("Vertical")*transform.forward;
        Vector3 horiz = Input.GetAxis("Horizontal")*transform.right;
        Vector3 deplacement = (vert + horiz) * speed;
        //Vector2 deplacement = new Vector2(vert, horiz);



        if (rb != null)
        {

            //Movement
            rb.velocity = new Vector3(deplacement.x, rb.velocity.y, deplacement.z);

            //Jump
            isGrounded = false;
            //création de la collision
            RaycastHit info;
            bool trouve = Physics.SphereCast(transform.position + transform.up *0.1f , 0.05f, -transform.up, out info, 2);

            //Check de la distance
            if(trouve && info.distance < 0.15)
            {
                isGrounded = true;
            }

            if(Input.GetButton("Jump"))
            {
                if(isGrounded)
                {
                    rb.AddForce(transform.up * 25, ForceMode.Impulse);
                }

                else
                {
                    if(rb.velocity.y < 3)
                    {
                        rb.AddForce(transform.up * 25);
                    }
                    else
                    {
                        rb.velocity = new Vector3(rb.velocity.x, 3, rb.velocity.z);
                    }
                    
                }
                
            }
        }
}
}
