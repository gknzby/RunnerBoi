using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Component
{
    [RequireComponent(typeof(Collider))]
    public class ObstacleSticky : Obstacle
    {
        [SerializeField] private float stickTime = 3f;
        [SerializeField] private float stickCooldown = 0.5f;
        private bool sticky = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag(RunnerBoi.ObjTags.Character.ToString()))
            {
                if(sticky)
                    StartCoroutine(StickRunner(collision.transform));
            }
        }

        private IEnumerator StickRunner(Transform runner_T)
        {
            sticky = false;
            Vector3 firstPos = runner_T.position - this.transform.position;
            float startTime = Time.time;
            yield return new WaitForFixedUpdate();
            while (Time.time - startTime < stickTime)
            {
                runner_T.position = this.transform.position + firstPos;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(stickCooldown);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
