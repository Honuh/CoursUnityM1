using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float angularSpeed = 1;
    public float jumpForce = 1;

    public Transform cam;
    private bool isGrounded;

    public Transform bullet;
    private float delayBullet = 0f;
    public float delayBulletMax = 0.1f;

    public GameObject canon;
    public GameObject canon2;
    private Vector3 dirTir;


    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = transform.GetComponentInChildren<Camera>().transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
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

        dirTir = cam.forward;
        dirTir = dirTir.normalized;
        if (Input.GetButton("Fire1"))
        {
            ParticleSystem firePart = canon.GetComponent<ParticleSystem>();
            ParticleSystem firePart2 = canon2.GetComponent<ParticleSystem>();

            if (delayBullet >= delayBulletMax)
            {
                firePart.Stop();
                firePart2.Stop();
                Transform balle = GameObject.Instantiate<Transform>(bullet).transform;
                balle.position = canon.transform.position;
                balle.rotation = canon.transform.rotation;
                balle.GetComponent<Rigidbody>().AddForce(dirTir *20, ForceMode.Impulse);
                delayBullet = 0f;
                firePart.Play();
                firePart2.Play();
            }
            delayBullet += Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            delayBullet = 1f;
        }


    }

    private void FixedUpdate()
    {

        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 vert = Input.GetAxis("Vertical") * transform.forward;
        Vector3 horiz = Input.GetAxis("Horizontal") * transform.right;
        Vector3 horizontalVelocity = (vert + horiz) * speed;

        if (rb != null)
        {

            //Movement
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);

            //tomber plus vite
            if (rb.velocity.y < 0.5)
            {
                rb.AddForce(-transform.up * 10);
            }

            //Jump
            isGrounded = false;

            //création de la collision
            RaycastHit info;
            bool trouve = Physics.SphereCast(transform.position + transform.up * 0.1f, 0.05f, -transform.up, out info, 2);

            //Check de la distance
            if (trouve && info.distance < 0.15)
            {
                isGrounded = true;
            }

            if (Input.GetButton("Jump"))
            {
                if (isGrounded)
                {
                    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                }

                else
                {
                    if (rb.velocity.y < 3)
                    {
                        rb.AddForce(transform.up * 25);
                    }
                    /*                    else
                                        {
                                            rb.velocity = new Vector3(rb.velocity.x, 3, rb.velocity.z);
                                        }*/

                }

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(canon.transform.position, dirTir);
        Gizmos.DrawLine(cam.position, cam.position+cam.forward*20);
    }
}
