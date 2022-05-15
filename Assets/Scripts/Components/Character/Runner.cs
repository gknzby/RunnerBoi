using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RunnerBoi.Character
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Runner : MonoBehaviour
    {
        protected Rigidbody RB;
        protected NavMeshAgent NMAgent;
        protected Transform StartPos_T;
        protected Transform FinishPos_T;
        
        protected virtual void SetDefaults()
        {
            this.RB = GetComponent<Rigidbody>();
            this.NMAgent = GetComponent<NavMeshAgent>();
            this.NMAgent.enabled = false;
        }

        public virtual void NewRace(Transform startPos_T, Transform finishPos_T)
        {
            this.StartPos_T = startPos_T;
            this.FinishPos_T = finishPos_T;

            this.SetDefaults();
            this.GoToStartPosition();
        }

        public void GoToStartPosition()
        {
            this.transform.SetPositionAndRotation(this.StartPos_T.position, this.StartPos_T.rotation);
            this.RB.velocity = Vector3.zero;
        }

        public void DestroySelf()
        {
            
            
            Destroy(this.gameObject);
        }
    }
}