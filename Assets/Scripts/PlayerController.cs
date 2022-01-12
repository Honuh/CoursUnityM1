using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Main Variables")]
    [SerializeField] float speed = 1;
    [SerializeField] float maxSpeed = 1;
    [SerializeField] float angularSpeed = 1;
    [SerializeField] float jumpForce = 50;
    [SerializeField] float dashForce = 10f;
    [SerializeField] private float maxLife = 100;
    public float currentLife;
    public HealthBar healthBar;

    //[SerializeField] checkWall checkWall;
    [Header("WallJump")]
    [SerializeField] float wallDistanceDetection;
    [SerializeField] LayerMask walls;
    [SerializeField] float wallJumpForce = 50f;
    [SerializeField] Transform wallJumpParticlesEmitter;
    private ParticleSystem wallParticle;

    [Header("Dash")]
    [SerializeField] Transform dashParticleTrans;
    [SerializeField] Transform dashParticlePosition;
    ParticleSystem dashParticle;
    private bool dash;
    private bool canDash;

    [Header("Camera")]
    public Transform cam;
    private Camera camera;
    [SerializeField] Transform tiltCamera;
    private bool isGrounded;

    private bool jump;
    private float x;
    private float y;
   
    private Vector3 vert;
    private Vector3 horiz;
    [Header("Ground Detection")]
    [SerializeField] float groundCheckRadius = 0.05f;

    [Header("Death")]
    [SerializeField] GameObject dedText;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = transform.GetComponentInChildren<Camera>().transform;
        }

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentLife = maxLife;
        healthBar.SetMaxHealth(maxLife);

        camera = tiltCamera.GetComponent<Camera>();

        dashParticle = dashParticleTrans.GetComponent<ParticleSystem>();
        dashParticle.Stop();
        dedText.SetActive(false);

    }

    // Update is called once per frame
    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
        dash = Input.GetKeyDown(KeyCode.LeftShift);

        vert = y * transform.forward;
        horiz = x * transform.right;

        Look();
        Jump();
        Dash();
        HealthBarUpdate();
        DeathText();
        
    }

    private void FixedUpdate()
    {
        Movements();

    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cam.position, shotPos);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(muzzle.transform.position, shotPos);*/
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(cam.position, cam.position + transform.forward * 10);
        Gizmos.DrawSphere(cam.position+transform.forward, 0.2f);
    }

    void Movements()
    {

        Vector3 horizontalVelocity = (vert + horiz) * speed;
        if (rb != null)
        {
            //Mort
            if (currentLife <= 0f)
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
                canDash = true;
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
             
                if (rb.velocity.magnitude > maxSpeed) horizontalVelocity = Vector3.zero;
                rb.AddForce(new Vector3(horizontalVelocity.x*5 , 0, horizontalVelocity.z*5));
             
            }

            //cameraTilt
            float tiltCam = -x * 2.5f;
            Vector3 tiltedCam = new Vector3(tiltCamera.eulerAngles.x, tiltCamera.eulerAngles.y, tiltCam);

            tiltCamera.eulerAngles = tiltedCam;

            //tomber plus vite
            /*if (rb.velocity.y < 0.5)
            {
                rb.AddForce(-transform.up * 20);
            }*/
        }
    }

    void Jump()
    {
        if (rb != null)
        {
            if (isGrounded && jump)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                /*if (rb.velocity.y < 3)
                {
                    rb.AddForce(transform.up * 25);
                }*/
            }


            //WallJump
            RaycastHit wall;
            if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out wall, wallDistanceDetection, walls))
            {
                //particles
                wallParticle = wallJumpParticlesEmitter.GetComponent<ParticleSystem>();
                wallParticle.Stop();
               
                Vector3 reflectVector = Vector3.Reflect(transform.forward, wall.normal);
                Vector3 wallJumpDirection = new Vector3(wall.normal.normalized.x, 2f, wall.normal.normalized.z);
                wallJumpDirection = wallJumpDirection.normalized;

                if (!isGrounded && jump)
                {
                   
                    wallJumpParticlesEmitter.position = new Vector3(wall.point.x, wall.point.y + 1f, wall.point.z);

                    rb.velocity = Vector3.zero;
                    rb.AddForce(wallJumpDirection * wallJumpForce, ForceMode.Impulse);
                    wallParticle.Play();
                    canDash = true;


                }
                
                Debug.Log(rb.velocity);
            }
        }
    }

    void Look()
    {
        float rotY = Input.GetAxis("Mouse Y") * -angularSpeed;
        float rotX = Input.GetAxis("Mouse X") * angularSpeed;
        //Mort
        if (currentLife <= 0)
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
        if (regardeDevant < 0.2)
        {
            //si on dépasse, on reprend la position précédente
            cam.rotation = lastRotation;
        }

        //Rotation du corps
        Quaternion rotationX = Quaternion.AngleAxis(rotX, transform.up);
        transform.rotation = rotationX * transform.rotation;
    }

    void HealthBarUpdate()
    {
        healthBar.SetHealth(currentLife);
    }

    void Dash()
    {
        if (!isGrounded && dash && canDash)
        {
            StartCoroutine(Dashing());
            rb.velocity = Vector3.zero;
            rb.AddForce(cam.forward * dashForce, ForceMode.Impulse);
            canDash = false;
        }
    }

    void DeathText()
    {
        if (currentLife <= 0f)
        {
            dedText.SetActive(true);
        }
    }

    IEnumerator Dashing()
    {
        
        camera.fieldOfView = 100f;
        rb.useGravity = false;

        dashParticleTrans.position = dashParticlePosition.position;
        dashParticleTrans.rotation = dashParticlePosition.rotation;
        dashParticle.Play();
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.2f);

        dashParticle.Stop();
        camera.fieldOfView = 90f;
        rb.useGravity = true;

    }
}
