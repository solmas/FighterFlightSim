using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    private Animator animator;
    private Camera playerCamera;

    void Start() {
        animator = GetComponentInChildren<Animator>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update () {
        HandleInput();
	}

    private bool punchToggle = false;
    private void HandleInput() {
        if (CrossPlatformInputManager.GetButton("Fire1")) {
            Punch();
        }
    }

    [SerializeField] private float punchCooldownTime = 0.3f;
    [SerializeField] private LayerMask punchable;
    [SerializeField] private float punchForce = 10f;

    private bool punchReady = true;
    private RaycastHit[] punchHitResults = new RaycastHit[16];

    private void Punch() {
        if (!punchReady) return;

        int hitCount = Physics.SphereCastNonAlloc(
            playerCamera.transform.position,
            0.5f,
            playerCamera.transform.forward,
            punchHitResults,
            5f,
            punchable);

        if (hitCount > 0) {
            punchHitResults[0].transform.GetComponentInParent<Animator>().enabled = false;
            for (int i=0; i<hitCount; ++i) {
                Debug.Log("Hit: " + punchHitResults[i].transform.name);
                punchHitResults[i].rigidbody.AddForceAtPosition(playerCamera.transform.forward * punchForce, punchHitResults[i].point, ForceMode.Impulse);
            }
        }

        animator.SetTrigger(punchToggle ? "PunchLeft" : "PunchRight");
        punchToggle = Random.Range(0, 2) == 0;
        punchReady = false;

        Invoke("ResetPunchCooldown", punchCooldownTime);
    }
    
    private void ResetPunchCooldown() {
        punchReady = true;
    }
}
