using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float maxSpeed = 1;
    public float angularSpeed = 1;
    public float jumpForce = 50;
    public float life = 100;

    public Transform cam;
    private bool isGrounded;

    private bool jump;
    private float x;
    private float y;
    private Vector3 vert;
    private Vector3 horiz;
    [Header("Ground Detection")]
    [SerializeField] float groundCheckRadius = 0.05f;

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
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");
        vert = y * transform.forward;
        horiz =  x * transform.right;
        
        float rotY = Input.GetAxis("Mouse Y") * -angularSpeed;
        float rotX = Input.GetAxis("Mouse X") * angularSpeed;

        //Mort
        if (life <= 0)
        {
            rotY = 0;
            rotX = 0;

        }

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
        
        Vector3 horizontalVelocity = (vert + horiz) * speed;

        if (rb != null)
        {
            //Mort
            if (life <= 0)
            {
                Animator anim = GetComponent<Animator>();
                anim.SetBool("death", true);
                rb.constraints = RigidbodyConstraints.FreezePositionZ;
                rb.constraints = RigidbodyConstraints.FreezePositionX;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                horizontalVelocity = Vector3.zero;

            }

            //Actualisation de la variable de collision avec le sol
            isGrounded = false;

            //création de la collision avec le sol
            RaycastHit info;
            bool trouve = Physics.SphereCast(transform.position + transform.up * 0.1f, groundCheckRadius, -transform.up, out info, 2);

            //Check de la distance
            if (trouve && info.distance < 0.15)
            {
                isGrounded = true;
            }

            //Jump
            if (isGrounded)
            {
                if(jump) rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                if (rb.velocity.y < 3)
                {
                    rb.AddForce(transform.up * 25);
                }
            }
            
            //Mouvements grounded
            if (isGrounded && !jump)
            {
                rb.AddForce(Vector3.down * Time.deltaTime * 5000);
                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            }
            //Mouvements mid air
            else
            {
                if (y > 0 && rb.velocity.y > maxSpeed) y = 0;
                if (y < 0 && rb.velocity.y < -maxSpeed) y = 0;
                if (x > 0 && rb.velocity.x > maxSpeed) y = 0;
                if (x < 0 && rb.velocity.x < -maxSpeed) y = 0;

                rb.AddForce(new Vector3(horizontalVelocity.x*0.5f, 0, horizontalVelocity.z*0.5f));
            }
            

            //tomber plus vite
            if (rb.velocity.y < 0.5)
            {
                rb.AddForce(-transform.up * 20);
            }


            Debug.Log(x);


        }
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cam.position, shotPos);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(muzzle.transform.position, shotPos);*/
    }   
}
