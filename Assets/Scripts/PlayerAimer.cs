using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour {
    private const bool DEBUG = false;

    [Header("References")]
    public GameObject cam;
    public GameObject lookatTarget;
    public GameObject laserSource;
    public LineRenderer lineRenderer;
    private Vector3 reticleTarget;

    [Header("Aiming")]
    public float maxAimDistance;
    public LayerMask raycastLayermask;
    public Vector3 baseTailOffset;
    public float maxRearTailDistance, maxSideTailDistance;
    public float minRearTailDistance, minSideTailDistance;
    public float forwardSimilarityMaxThreshold, forwardSimilarityMinThreshold;


    // Start is called before the first frame update
    void Start() {
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update() {
        CalculateReticlePosition();
        UpdateLaserSource();
        UpdateLineRenderer();
    }

    // -- Find the aim position
    // Raycast from the lookat target, in the direction of the camera.
    //  We start from the lookat target, to prevent any issues with obstructions between the camera and the lookat target
    void CalculateReticlePosition() {
        Vector3 raycastDir = (lookatTarget.transform.position - cam.transform.position).normalized;

        // If we collide with anything, that'll be the aim position 
        // If we DON'T collide with anything, then just set the aim position to be "maxAimDistance" meters away
        RaycastHit hit;
        if (Physics.Raycast(lookatTarget.transform.position, raycastDir, out hit, maxAimDistance, raycastLayermask)) {
            // Something is in our reticle - aim at it
            reticleTarget = hit.point;
        } else {
            // Nothing in our reticle. Just aim "maxAimDistance" units away
            reticleTarget = cam.transform.position + raycastDir * maxAimDistance;
        }
    }

    // Move the laser source into position, and make it face the target
    // If the target is infront of the cart, then the tail should move to the side (to avoid shooting the cat)
    void UpdateLaserSource() {
        // Calculate whether or not the laser is pointed in the same direction as the cart
        // If the laser is pointed forward (and so is the cart), then move the tail to the side, and forwards.
        // If the laser is pointed to the side or backwards, then move the tail to the center, and back
        //
        // forwardSimilarity: 1 if laser is pointed forwards, -1 if pointed backwards
        // sideSimilarity: 1 if laser is pointed right, -1 if pointed left
        float forwardSimilarity = Vector3.Dot(lookatTarget.transform.forward, cam.transform.forward);
        float sideSimilarity = Vector3.Dot(lookatTarget.transform.right, cam.transform.forward);

        // TODO: Fix snapping when laser faces forward
        //if (forwardSimilarity > forwardSimilarityMaxThreshold) {
        //    sideSimilarity = 1f;
        //} else if (forwardSimilarity < forwardSimilarityMinThreshold) {
        //    sideSimilarity = -1f;
        //}

        if (DEBUG) {
            Debug.Log("Forward similarity: " + forwardSimilarity);
            Debug.Log("Side similarity: " + sideSimilarity);
        }


        // Lerp between the calculated offsets, and add that to the base position
        Vector3 tailLocalPos = baseTailOffset;
        tailLocalPos.z += Mathf.Lerp(minRearTailDistance, maxRearTailDistance, forwardSimilarity);
        tailLocalPos.x += Mathf.Lerp(minSideTailDistance, maxSideTailDistance, sideSimilarity);
        laserSource.transform.localPosition = tailLocalPos;

        // Make the blaster look at the target
        laserSource.transform.LookAt(reticleTarget, Vector3.up);
    }

    void UpdateLineRenderer() {
        lineRenderer.SetPosition(0, laserSource.transform.position);
        lineRenderer.SetPosition(1, reticleTarget);

        if (DEBUG) {
            Debug.DrawLine(lookatTarget.transform.position, reticleTarget, Color.green);
            Debug.DrawLine(laserSource.transform.position, reticleTarget, Color.red);
        }
    }
}
