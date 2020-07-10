using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour {
    [Header("References")]
    public GameObject cam;
    public GameObject lookatTarget;
    public GameObject laserSource;
    public LineRenderer lineRenderer;
    private Vector3 reticleTarget;

    [Header("Aiming")]
    public float maxAimDistance;
    public LayerMask raycastLayermask;
    public float laserSourceTurnSpeed;


    // Start is called before the first frame update
    void Start() {
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update() {
        UpdateLineRenderer();
        laserSource.transform.LookAt(reticleTarget * laserSourceTurnSpeed, Vector3.up);
    }

    void UpdateLineRenderer() {
        lineRenderer.SetPosition(0, laserSource.transform.position);

        // -- Find the aim position
        // Raycast from the lookat target, in the direction of the camera.
        //  We start from the lookat target, to prevent any issues with obstructions between the camera and the lookat target
        // If we collide with anything, that'll be the aim position 
        // If we DON'T collide with anything, then just set the aim position to be "maxAimDistance" meters away
        Vector3 raycastDir = (lookatTarget.transform.position - cam.transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(lookatTarget.transform.position, raycastDir, out hit, maxAimDistance, raycastLayermask)) {
            // Something is in our reticle - aim at it
            reticleTarget = hit.point;
        } else {
            // Nothing in our reticle. Just aim "maxAimDistance" units away
            reticleTarget = cam.transform.position + raycastDir * maxAimDistance;
        }

        lineRenderer.SetPosition(1, reticleTarget);

        Debug.DrawLine(lookatTarget.transform.position, reticleTarget, Color.green);
        Debug.DrawLine(laserSource.transform.position, reticleTarget, Color.red);
    }
}
