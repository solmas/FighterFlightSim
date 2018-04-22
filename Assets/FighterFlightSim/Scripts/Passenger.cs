using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour {

    protected Animator passengerAnimator;
    protected AudioSource audioSource;

    [SerializeField] protected AudioClip hitSfx;
    [SerializeField] protected AudioClip grumbleSfx;

    protected float gruntRadius = 10f;

    protected float surlyLevel = 0f;
    protected float surlyGrowRate = 0.01f;

    protected readonly float SURLY_LEVEL_1 = 1f;
    protected readonly float SURLY_LEVEL_2 = 2f;
    protected readonly float SURLY_LEVEL_3 = 3f;

    protected enum State {
        CALM,
        SURLY_1,
        SURLY_2,
        SURLY_3
    }

    protected State currentState = State.CALM;

    void Start () {
        passengerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
	}

    void Update() {
        UpdateSurlyness();    
    }

    protected void UpdateSurlyness() {
        if (surlyLevel < SURLY_LEVEL_1) { // surly leve 0-1
            HandleCalmState();
        }
        else if (surlyLevel < SURLY_LEVEL_2) { // surly level 1-2
            HandleSurlyLvl1();
        }
        else if (surlyLevel < SURLY_LEVEL_3) { // surly level 2-3
            HandleSurlyLvl2();
        }
        else { // surly level 3+
            HandleSurlyLvl3();
        }

        surlyLevel += surlyGrowRate * Time.deltaTime;
    }

    protected void HandleCalmState() {
        if (currentState == State.CALM) return;

        Debug.Log(name + " has returned to calm state");

        surlyGrowRate = 0.01f;

        currentState = State.CALM;
    }

    [SerializeField] protected LayerMask affectedByGrunt;
    protected Collider[] gruntColliders = new Collider[16];

    protected void HandleSurlyLvl1() {
        if (currentState == State.SURLY_1) return;

        Debug.Log(name + " has reached SURLY LEVEL 1");

        /*
        int count = Physics.OverlapSphereNonAlloc(transform.position, gruntRadius, gruntColliders, affectedByGrunt);
        if (count > 0) {
            audioSource.pitch = Random.Range(.90f, 1.05f);
            audioSource.clip = grumbleSfx;
            audioSource.Play();

            for (int i=0; i<count; ++i) {
                gruntColliders[i].GetComponentInParent<Passenger>().AddSurly(0.2f);
            }
        }
        //*/

        currentState = State.SURLY_1;
    }

    protected void HandleSurlyLvl2() {
        if (currentState == State.SURLY_2) return;

        Debug.Log(name + " has reached SURLY LEVEL 2");

        currentState = State.SURLY_2;
    }

    protected void HandleSurlyLvl3() {
        if (currentState == State.SURLY_3) return;

        Debug.Log(name + " has reached SURLY LEVEL 3");

        currentState = State.SURLY_3;
    }

    public void AddSurly(float amount) {
        surlyLevel += amount;
    }

    public void TakeHit() {
        passengerAnimator.enabled = false;

        surlyLevel -= 1f;

        if (surlyLevel < 0f) surlyLevel = 0f;

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

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gruntRadius);
    }
}
