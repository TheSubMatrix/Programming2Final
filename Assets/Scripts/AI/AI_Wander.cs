using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AI_Wander : BC_AIState
{
    GameObject gameObject;
    Coroutine m_wanderRoutine;
    public override void EnterState(AI stateHandler)
    {
        Debug.Log("Wander State");
        gameObject = stateHandler.gameObject;
        StartWander(stateHandler);
    }

    public override void ExitState(AI stateHandler)
    {
        StopWander(stateHandler);
    }

    public override void OnTargetHeard(AI stateHandler, ISoundReactable.HeardSoundInfo heardSoundInfo)
    {
        StopWander(stateHandler);
        stateHandler.SwitchState(this, stateHandler.AIInvestigate);
    }

    public override void OnTargetLost(AI stateHandler, GameObject objectLost)
    {

    }

    public override void OnTargetSeen(AI stateHandler, GameObject objectSeen)
    {
        StopWander(stateHandler);
        stateHandler.SwitchState(this, stateHandler.AIChase);
    }

    public override void UpdateState(AI stateHandler)
    {

    }
    IEnumerator WanderRoutine(AI stateHandler)
    {
        stateHandler.Agent.destination = RandomNavSphere(gameObject.transform.position, Random.Range(stateHandler.MinWanderDistance, stateHandler.MaxWanderDistance), stateHandler.RandomSphereLayerMask);
        yield return new WaitUntil(() => stateHandler.Agent.remainingDistance != Mathf.Infinity && stateHandler.Agent.pathStatus == NavMeshPathStatus.PathComplete && stateHandler.Agent.remainingDistance <= stateHandler.Agent.stoppingDistance);
        yield return new WaitForSeconds(Random.Range(stateHandler.MinWanderWaitTime, stateHandler.MaxWanderWaitTime));
        m_wanderRoutine = stateHandler.StartCoroutine(WanderRoutine(stateHandler));
    }
    void StartWander(AI stateHandler)
    {
        m_wanderRoutine = stateHandler.StartCoroutine(WanderRoutine(stateHandler));
    }
    void StopWander(AI stateHandler)
    {
        stateHandler.StopCoroutine(m_wanderRoutine);
    }
}
