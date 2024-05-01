using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AI_Investigate : BC_AIState
{
    Coroutine m_currentRoutine;
    Coroutine m_wanderRoutine;
    public override void EnterState(AI stateHandler)
    {
        Debug.Log("Investigate State");
        StartWalkToPosition(stateHandler, stateHandler.InvestigationPosition);
    }

    public override void ExitState(AI stateHandler)
    {
        if (m_currentRoutine != null)
        {
            stateHandler.StopCoroutine(m_currentRoutine);
        }
        if (m_wanderRoutine != null)
        {
            stateHandler.StopCoroutine(m_wanderRoutine);
        }
    }

    public override void OnTargetHeard(AI stateHandler, ISoundReactable.HeardSoundInfo heardSoundInfo)
    {
        stateHandler.InvestigationPosition = heardSoundInfo.SoundLocation;
        if (m_currentRoutine != null)
        {
            stateHandler.StopCoroutine(m_currentRoutine);
        }
        if (m_wanderRoutine != null)
        {
            stateHandler.StopCoroutine(m_wanderRoutine);
        }
        StartWalkToPosition(stateHandler, stateHandler.InvestigationPosition);
    }

    public override void OnTargetLost(AI stateHandler, GameObject objectLost)
    {

    }

    public override void OnTargetSeen(AI stateHandler, GameObject objectSeen)
    {
        if(m_currentRoutine != null)
        {
            stateHandler.StopCoroutine(m_currentRoutine);
        }
        if(m_wanderRoutine != null)
        {
            stateHandler.StopCoroutine(m_wanderRoutine);
        }
        stateHandler.SwitchState(this, stateHandler.AIChase);
    }

    public override void UpdateState(AI stateHandler)
    {

    }
    void StartWalkToPosition(AI stateHandler, Vector3 destinationPosition)
    {
        m_currentRoutine = stateHandler.StartCoroutine(WalkToPosition(stateHandler, destinationPosition));
    }
    IEnumerator WalkToPosition(AI stateHandler, Vector3 destinationPosition)
    {
        stateHandler.Agent.destination = destinationPosition;
        yield return new WaitUntil(() => stateHandler.Agent.remainingDistance != Mathf.Infinity && stateHandler.Agent.pathStatus == NavMeshPathStatus.PathComplete && stateHandler.Agent.remainingDistance <= stateHandler.Agent.stoppingDistance);
        m_currentRoutine = stateHandler.StartCoroutine(WanderForPeriod(stateHandler));
    }
    IEnumerator WanderRoutine(AI stateHandler)
    {
        stateHandler.Agent.destination = RandomNavSphere(stateHandler.InvestigationPosition, Random.Range(stateHandler.MinInvestigateDistance, stateHandler.MaxInvestigateDistance), stateHandler.RandomSphereLayerMask);
        yield return new WaitUntil(() => stateHandler.Agent.remainingDistance != Mathf.Infinity && stateHandler.Agent.pathStatus == NavMeshPathStatus.PathComplete && stateHandler.Agent.remainingDistance <= stateHandler.Agent.stoppingDistance);
        yield return new WaitForSeconds(Random.Range(stateHandler.MinInvestigateWaitTime, stateHandler.MaxInvestigateWaitTime));
        m_currentRoutine = stateHandler.StartCoroutine(WanderRoutine(stateHandler));
    }
    IEnumerator WanderForPeriod(AI stateHandler)
    {
        m_wanderRoutine = stateHandler.StartCoroutine(WanderRoutine(stateHandler));
        yield return new WaitForSeconds(stateHandler.TimeToWanderAfterTargetHeard);
        stateHandler.StopCoroutine(m_wanderRoutine);
        stateHandler.SwitchState(this, stateHandler.AIWander);
    }
}
