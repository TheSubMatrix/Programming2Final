using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public abstract class BC_AIState 
{
    public abstract void EnterState(AI stateHandler);
    public abstract void UpdateState(AI stateHandler);
    public abstract void ExitState(AI stateHandler);
    public abstract void OnTargetSeen(AI stateHandler, GameObject objectSeen);
    public abstract void OnTargetLost(AI stateHandler, GameObject objectLost);
    public abstract void OnTargetHeard(AI stateHandler, ISoundReactable.HeardSoundInfo heardSoundInfo);
    public Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }
}
