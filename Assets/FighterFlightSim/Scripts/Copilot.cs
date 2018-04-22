using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copilot : Passenger {

    protected new void HandleCalmState() {
        if (currentState == State.CALM) return;

        Debug.Log(name + " has returned to calm state");

        surlyGrowRate = 0.01f;

        currentState = State.CALM;
    }

    protected new void HandleSurlyLvl1() {
        if (currentState == State.SURLY_1) return;

        Debug.Log(name + " has reached SURLY LEVEL 1");

        currentState = State.SURLY_1;
    }

    protected new void HandleSurlyLvl2() {
        if (currentState == State.SURLY_2) return;

        Debug.Log(name + " has reached SURLY LEVEL 2");

        currentState = State.SURLY_2;
    }

    protected new void HandleSurlyLvl3() {
        if (currentState == State.SURLY_3) return;

        Debug.Log(name + " has reached SURLY LEVEL 3");

        currentState = State.SURLY_3;
    }
}
