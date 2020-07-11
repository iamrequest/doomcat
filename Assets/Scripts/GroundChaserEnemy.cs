using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChaserEnemy : MonoBehaviour {
    [Tooltip("The target of the multi-aim constraint")]
    public Transform multiaimLookatTarget;

    private void FixedUpdate() {
        // Update the Animation Rig multi-aim target, so that it looks at the cat in the cart
        multiaimLookatTarget.position = Player.instance.catTransform.position;
    }
}
