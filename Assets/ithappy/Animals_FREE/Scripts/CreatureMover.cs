using System;
using UnityEngine;
using UnityEngine.AI;

namespace ithappy.Animals_FREE
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class CreatureMover : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        private float m_WalkSpeed = 1f;
        [SerializeField]
        private float m_RunSpeed = 4f;
        [SerializeField, Range(0f, 360f)]
        private float m_RotateSpeed = 90f;

        [Header("Animator")]
        [SerializeField]
        private string m_VerticalID = "Vert";
        [SerializeField]
        private string m_StateID = "State";
        [SerializeField]
        private LookWeight m_LookWeight = new(1f, 0.3f, 0.7f, 1f);

        private Transform m_Transform;
        private NavMeshAgent m_Agent;
        private Animator m_Animator;

        private AnimationHandler m_Animation;

        private Vector3 m_Target;
        private bool m_IsRun;
        private bool m_HasDestination;

        public Vector3 Target => m_Target;
        public bool IsRun => m_IsRun;
        public bool HasDestination => m_HasDestination;

        private void OnValidate()
        {
            m_WalkSpeed = Mathf.Max(m_WalkSpeed, 0f);
            m_RunSpeed = Mathf.Max(m_RunSpeed, m_WalkSpeed);
        }

        private void Awake()
        {
            m_Transform = transform;
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();

            m_Animation = new AnimationHandler(m_Animator, m_VerticalID, m_StateID);
            if(m_Agent)
            {
            m_Agent.speed = m_WalkSpeed;
            m_Agent.angularSpeed = m_RotateSpeed;
            m_Agent.acceleration = 8f;
            m_Agent.updateRotation = true;
            m_Agent.updatePosition = true;

            }
        }

        private void Start()
        {
            if(m_Agent)
            SetRandomDestination(10, UnityEngine.Random.Range(0, 10) < 5);
        }

        private void Update()
        {
            if (m_Agent)
            {
                if (m_HasDestination)
                {
                    float distance = Vector3.Distance(m_Transform.position, m_Target);
                    if (distance <= m_Agent.stoppingDistance + 0.1f)
                    {
                        SetRandomDestination(10, UnityEngine.Random.Range(0, 10) < 5);
                    }
                }

                Vector3 velocity = m_Agent.velocity;
                float speed = velocity.magnitude / (m_IsRun ? m_RunSpeed : m_WalkSpeed);
                speed = Mathf.Clamp01(speed);

                Vector2 animAxis = new Vector2(0, speed);
                m_Animation.Animate(in animAxis, m_IsRun ? 1f : 0f, Time.deltaTime);
            }
        }

        private void OnAnimatorIK()
        {
            if (m_HasDestination)
            {
                m_Animation.AnimateIK(in m_Target, m_LookWeight);
            }
        }
        public void SetDestination(in Vector3 target, bool isRun)
        {
            m_Target = target;
            m_IsRun = isRun;
            m_HasDestination = true;

            m_Agent.speed = isRun ? m_RunSpeed : m_WalkSpeed;
            m_Agent.SetDestination(m_Target);
        }

        public void Stop()
        {
            m_HasDestination = false;
            m_Agent.ResetPath();
            m_Agent.velocity = Vector3.zero;
        }

        public bool SetRandomDestination(float radius, bool isRun = false)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
            randomDirection += m_Transform.position;
            if(m_Agent.isActiveAndEnabled)
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                SetDestination(hit.position, isRun);
                return true;
            }

            return false;
        }

        [Serializable]
        private struct LookWeight
        {
            public float weight;
            public float body;
            public float head;
            public float eyes;

            public LookWeight(float weight, float body, float head, float eyes)
            {
                this.weight = weight;
                this.body = body;
                this.head = head;
                this.eyes = eyes;
            }
        }

        private class AnimationHandler
        {
            private readonly Animator m_Animator;
            private readonly string m_VerticalID;
            private readonly string m_StateID;

            private readonly float k_InputFlow = 4.5f;

            private float m_FlowState;
            private Vector2 m_FlowAxis;

            public AnimationHandler(Animator animator, string verticalID, string stateID)
            {
                m_Animator = animator;
                m_VerticalID = verticalID;
                m_StateID = stateID;
            }

            public void Animate(in Vector2 axis, float state, float deltaTime)
            {
                m_Animator.SetFloat(m_VerticalID, m_FlowAxis.magnitude);
                m_Animator.SetFloat(m_StateID, Mathf.Clamp01(m_FlowState));

                m_FlowAxis = Vector2.ClampMagnitude(m_FlowAxis + k_InputFlow * deltaTime * (axis - m_FlowAxis).normalized, 1f);
                m_FlowState = Mathf.Clamp01(m_FlowState + k_InputFlow * deltaTime * Mathf.Sign(state - m_FlowState));
            }

            public void AnimateIK(in Vector3 target, in LookWeight lookWeight)
            {
                m_Animator.SetLookAtPosition(target);
                m_Animator.SetLookAtWeight(lookWeight.weight, lookWeight.body, lookWeight.head, lookWeight.eyes);
            }
        }
    }
}
