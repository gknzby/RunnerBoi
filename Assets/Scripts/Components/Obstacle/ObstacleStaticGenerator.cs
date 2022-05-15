using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Component
{
    public class ObstacleStaticGenerator : MonoBehaviour
    {
        [Header("Obstacle Properties")]
        [SerializeField] private GameObject ObstaclePrefab_Obj;
        [SerializeField] private Transform ObstacleStart_T;
        [SerializeField] private ObstacleType obstacleType;
        [SerializeField] private Vector3 ObstacleScale;

        private Transform obstacle_T;

        private void Awake()
        {
#if UNITY_EDITOR
            hideGizmos = true;
#endif
        }

        private void OnEnable()
        {
            obstacle_T = GameObject.Instantiate(ObstaclePrefab_Obj, ObstacleStart_T.position, ObstacleStart_T.rotation, ObstacleStart_T.transform).transform;
            obstacle_T.localScale = GetNewScale();

            switch (obstacleType)
            {
                case ObstacleType.Kill:
                    obstacle_T.gameObject.AddComponent<ObstacleKill>();
                    break;
                case ObstacleType.Sticky:
                    obstacle_T.gameObject.AddComponent<ObstacleSticky>();
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            Destroy(obstacle_T.GetComponent<Obstacle>());
            Destroy(obstacle_T.gameObject);
        }


        private Vector3 GetNewScale()
        {
            Vector3 nScale = this.ObstacleScale;
            nScale.x *= this.ObstaclePrefab_Obj.transform.localScale.x;
            nScale.y *= this.ObstaclePrefab_Obj.transform.localScale.y;
            nScale.z *= this.ObstaclePrefab_Obj.transform.localScale.z;

            return nScale;
        }


        private void OnApplicationQuit()
        {
#if UNITY_EDITOR
            hideGizmos = false;
#endif
        }

        #region Gizmos Drawing
#if UNITY_EDITOR
        [Header("Editor Mode Draw")]
        [Range(0f, 1f)]
        [SerializeField] private float GizmosLerp;

        public enum GizmosType
        {
            WireMesh,
            Mesh,
            None
        }
        [SerializeField] private GizmosType GizType;
        private Vector3 gizmosScale;
        private bool hideGizmos = false;

        private void DrawObstacle(Color gizmosColor, Vector3 gizmosPos)
        {
            Gizmos.color = gizmosColor;
            switch (GizType)
            {
                case GizmosType.WireMesh:
                    Gizmos.DrawWireMesh(this.ObstaclePrefab_Obj.GetComponent<MeshFilter>().sharedMesh, gizmosPos, ObstacleStart_T.rotation, gizmosScale);
                    break;
                case GizmosType.Mesh:
                    Gizmos.DrawMesh(this.ObstaclePrefab_Obj.GetComponent<MeshFilter>().sharedMesh, gizmosPos, ObstacleStart_T.rotation, gizmosScale);
                    break;
                default:
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            if (hideGizmos || this.ObstaclePrefab_Obj == null || this.ObstacleStart_T == null)
                return;

            gizmosScale = GetNewScale();

            DrawObstacle(Color.blue, this.ObstacleStart_T.position);
        }
#endif
        #endregion
    }
}
