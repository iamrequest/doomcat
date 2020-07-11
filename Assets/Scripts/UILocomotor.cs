using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILocomotor : MonoBehaviour {
    [Tooltip("Only rotate along the Y (vertical) axis)")]
    public bool yRotConstrain;
    public Transform lookatTarget;
    public Transform followObject;
    public Vector3 followObjectOffset;

    private void FixedUpdate() {
        if (lookatTarget == null) {
            lookatTarget = Player.instance.catTransform;
        }

        if (yRotConstrain) {
            Vector3 dir = Vector3.ProjectOnPlane(transform.position - lookatTarget.position, Vector3.up).normalized;
            transform.forward = dir;
        } else {
            transform.LookAt(Player.instance.catTransform.position, Vector3.up);
        }

        transform.position = followObject.position + followObjectOffset;
    }
}
