using System;
using System.Collections;
using System.Collections.Generic;
using Ei.Agents.Sims;
using UnityEngine;
using NodeCanvas.BehaviourTrees;

[RequireComponent(typeof(InteractiveObjectBT))]
public class BTPlanPerformer : PlanPerformer {
    public bool forceTransform;
    public bool disablePushing = true;
    public string animationState;

    private float radius;
    private InteractiveObjectBT iObject;

    private void Start() {
        this.iObject = GetComponent<InteractiveObjectBT>();
    }


    public override void Perform(GameObject agent, SimAction action, Action successCallback, Action failedCallback) {
        if (iObject == null) {
            Debug.LogError("Agent can only interact with objects with behaviour trees");
            return;
        }

        if (disablePushing) {
            radius = agent.GetComponent<UnityEngine.AI.NavMeshAgent>().radius;
            agent.GetComponent<UnityEngine.AI.NavMeshAgent>().radius = 0;
        }

        // start the coroutine
        StartCoroutine(iObject.InteractWithObject(agent, null, action.Name, forceTransform, animationState, (success) => {
            if (disablePushing) {
                agent.GetComponent<UnityEngine.AI.NavMeshAgent>().radius = this.radius;
            }

            // notify
            if (success) {
                successCallback();
            } else {
                failedCallback();
            }
        }));
    }

    
}
