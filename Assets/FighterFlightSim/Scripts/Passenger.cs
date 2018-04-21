using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour {

    private Animator passengerAnimator;
    private AudioSource audioSource;

    [SerializeField] private AudioClip hitSfx;

	void Start () {
        passengerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
	}
	
    public void TakeHit() {
        passengerAnimator.enabled = false;

        audioSource.pitch = Random.Range(.90f, 1.05f);
        audioSource.clip = hitSfx;
        audioSource.Play();

        StopAllCoroutines();
        StartCoroutine(ResetHit());
    }

    private IEnumerator ResetHit() {
        yield return new WaitForSeconds(2f);
        passengerAnimator.enabled = true;
    }
}
