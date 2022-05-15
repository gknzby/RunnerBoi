using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Component
{
    [RequireComponent(typeof(Collider))]
    public class ObstacleKill : Obstacle
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag(RunnerBoi.ObjTags.Character.ToString()))
            {
                RunnerBoi.Managers.LevelManager.Instance.RunnerHitObstacle(collision.transform);
            }
        }
    }
}

