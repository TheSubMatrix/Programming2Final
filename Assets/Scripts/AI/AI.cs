using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))]
public class AI : MonoBehaviour
{
    BC_AIState m_currentState;
    //[Header("Wander")]
    [field: SerializeField] public float MinWanderDistance { get; private set; } = 5;
    [field: SerializeField] public float MaxWanderDistance { get; private set; } = 15;
    [field: SerializeField] public float MinWanderWaitTime { get; private set; } = 3;
    [field: SerializeField] public float MaxWanderWaitTime { get; private set; } = 5;
    //[Header("Investigate")]
    [field: SerializeField] public float MinInvestigateDistance { get; private set; } = 2;
    [field: SerializeField] public float MaxInvestigateDistance { get; private set; } = 5;
    [field: SerializeField] public float MinInvestigateWaitTime { get; private set; } = 1;
    [field: SerializeField] public float MaxInvestigateWaitTime { get; private set; } = 2;
    [field: SerializeField] public float TimeToWanderAfterTargetHeard { get; private set; } = 10;
    [field: SerializeField] public float TimeToGiveUpOnChase { get; private set; } = 5;
    [field: SerializeField] public LayerMask RandomSphereLayerMask { get; private set; }
    [field: SerializeField] public float AttackingDistance { get; private set; } = 5;
    [field: SerializeField] public List<Damager> Damagers { get; private set; } = new List<Damager>();
    public NavMeshAgent Agent { get; private set; }
    public AI_Wander AIWander { get; private set; } = new AI_Wander();
    public AI_Investigate AIInvestigate { get; private set; } = new AI_Investigate();
    public AI_Chase AIChase { get; private set; } = new AI_Chase();
    public Vector3 InvestigationPosition { get; set; }
    public GameObject CurrentTarget { get; set; }
    public Animator Animator { get; private set; }

    const float magnitudeVelocityScalar = 0.5f;
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        m_currentState = AIWander;
        m_currentState.EnterState(this);
    }
    private void Update()
    {
        Vector3 walkVelocity = new Vector3(Agent.velocity.x, 0, Agent.velocity.z);
        Animator.SetFloat("Velocity Magnitude", walkVelocity.magnitude * magnitudeVelocityScalar);
        Animator.SetFloat("Forward Velocity", Vector3.Dot(walkVelocity.normalized, transform.forward));
        Animator.SetFloat("Right Velocity",Vector3.Dot(walkVelocity.normalized, transform.right));
        m_currentState.UpdateState(this);
    }
    public void OnTargetSeen(GameObject target)
    {
        CurrentTarget = target;
        m_currentState.OnTargetSeen(this, target);
    }
    public void OnTargetLost(GameObject target)
    {
        m_currentState.OnTargetLost(this, target);
    }
    public void OnTargetHeard(ISoundReactable.HeardSoundInfo heardSoundInfo)
    {
        m_currentState.OnTargetHeard(this, heardSoundInfo);
    }

    public void SwitchState(BC_AIState currentState,BC_AIState desiredState)
    {
        currentState.ExitState(this);
        m_currentState = desiredState;
        desiredState.EnterState(this);
    }
    public void EnableDamagers()
    {
        foreach(Damager damager in Damagers)
        {
            damager.enabled = true;
        }
    }
    public void DisableDamagers()
    {
        foreach (Damager damager in Damagers)
        {
            damager.enabled = false;
        }
    }
}
