using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Switch() {
        animator.SetTrigger("Switch");
    }
}
