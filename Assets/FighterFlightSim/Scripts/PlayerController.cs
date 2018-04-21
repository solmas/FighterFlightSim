using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    private Animator animator;

    void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    void Update () {
        HandleInput();
	}

    private bool punchToggle = false;
    private void HandleInput() {
        if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
            animator.SetTrigger(punchToggle ? "PunchLeft" : "PunchRight");
            punchToggle = !punchToggle;
        }
    }
}
