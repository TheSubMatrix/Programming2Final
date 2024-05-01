using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AI_Chase : BC_AIState
{
    Coroutine m_stopChaseRoutine;
    public override void EnterState(AI stateHandler)
    {
        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(stateHandler.transform, SoundManager.instance.FindSoundInfoByName("Alert"));
        }
    }

    public override void ExitState(AI stateHandler)
    {
        stateHandler.Animator.SetBool("Should Be Punching", false);
    }

    public override void OnTargetHeard(AI stateHandler, ISoundReactable.HeardSoundInfo heardSoundInfo)
    {

    }

    public override void OnTargetLost(AI stateHandler, GameObject objectlost)
    {
        m_stopChaseRoutine = stateHandler.StartCoroutine(StopChaseTimer(stateHandler, objectlost));
    }

    public override void OnTargetSeen(AI stateHandler, GameObject objectSeen)
    {
        stateHandler.StopCoroutine(m_stopChaseRoutine);
    }

    public override void UpdateState(AI stateHandler)
    {
        stateHandler.Agent.destination = stateHandler.CurrentTarget.transform.position;
        stateHandler.InvestigationPosition = stateHandler.CurrentTarget.transform.position;
        Vector3 directionOfTarget = (stateHandler.CurrentTarget.transform.position - stateHandler.transform.position).normalized;
        if(Vector3.Distance(stateHandler.transform.position, stateHandler.CurrentTarget.transform.position) < stateHandler.AttackingDistance && Vector3.Dot(stateHandler.transform.forward, directionOfTarget) > 0.25f)
        {
            stateHandler.Animator.SetBool("Should Be Punching", true);
        }
        else
        {
            stateHandler.Animator.SetBool("Should Be Punching", false);
        }
    }
    IEnumerator StopChaseTimer(AI stateHandler, GameObject target)
    {
        yield return new WaitForSeconds(stateHandler.TimeToGiveUpOnChase);
        stateHandler.SwitchState(this, stateHandler.AIInvestigate);
        if (stateHandler.CurrentTarget == target)
        {
            stateHandler.CurrentTarget = null;
        }
    }
}
