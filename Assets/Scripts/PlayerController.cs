using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float maxSpeed = 1;
    public float angularSpeed = 1;
    public Transform cam;


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
        if(Vector3.Dot(cam.forward, transform.forward) > 0)
        {
            float rot = 0;
            rot = -Input.GetAxis("Mouse Y") * angularSpeed / 5;
            Quaternion q = Quaternion.AngleAxis(rot, cam.right);
            cam.rotation = q * cam.rotation;
        }
        
    }

    private void FixedUpdate()
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 vert = Input.GetAxis("Vertical") * transform.forward;
        Vector3 horiz = Input.GetAxis("Horizontal") * transform.right;

        if (rb != null)
        {
            //Movement
            rb.velocity = (vert + horiz) * speed;

            //Rotation
            rb.angularVelocity = (transform.up * angularSpeed * Input.GetAxis("Mouse X"));
            //cam.Rotate(-Input.GetAxis("Mouse Y") * angularSpeed, 0, 0);
        }

    }
}
