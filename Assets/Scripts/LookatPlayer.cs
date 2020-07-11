using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPlayer : MonoBehaviour {
    [Tooltip("Only rotate along the Y (vertical) axis)")]
    public bool yRotConstrain;
    private void FixedUpdate() {
        if (yRotConstrain) {
            Vector3 dir = Vector3.ProjectOnPlane(transform.position - Player.instance.catTransform.position, Vector3.up).normalized;
            transform.forward = dir;
        } else {
            transform.LookAt(Player.instance.catTransform.position, Vector3.up);
        }
    }
}
