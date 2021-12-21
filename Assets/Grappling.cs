using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplinPoint;
    public Transform player;

    public LayerMask grappable;
    public Transform cam, guntip;
    public float maxDistance = 100f;
    private SpringJoint grapJoint;

    public float spring, damper, massSclale;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StartGrappling();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            StopGrappling();
        }
    }

    private void LateUpdate()
    {
        DrawLine();
    }

    void StartGrappling()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, grappable))
        {
            grapplinPoint = hit.point;
            grapJoint = player.gameObject.AddComponent<SpringJoint>();
            grapJoint.autoConfigureConnectedAnchor = false;
            grapJoint.connectedAnchor = grapplinPoint;

            float distancePlayerGrapplin = Vector3.Distance(player.position, grapplinPoint);

            grapJoint.maxDistance = distancePlayerGrapplin * 0.7f;
            grapJoint.minDistance = distancePlayerGrapplin * 0.55f;

            grapJoint.spring = spring;
            grapJoint.damper = damper;
            grapJoint.massScale = massSclale;

            lr.positionCount = 2;

        }
    }

    void StopGrappling()
    {
        lr.positionCount = 0;
        Destroy(grapJoint);
    }

    void DrawLine()
    {
        //If not grappling, don't draw rope
        if (!grapJoint) return;


        lr.SetPosition(0, guntip.position);
        lr.SetPosition(1, grapplinPoint);
    }

}
