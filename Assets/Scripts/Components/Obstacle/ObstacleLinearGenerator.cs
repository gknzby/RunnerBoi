using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Component
{
    public class ObstacleLinearGenerator : MonoBehaviour
    {
        [Header("Obstacle Properties")]
        [SerializeField] private GameObject ObstaclePrefab_Obj;
        [SerializeField] private Transform ObstacleStart_T;
        [SerializeField] private Transform ObstacleEnd_T;
        [SerializeField] private ObstacleType obstacleType;
        [SerializeField] private Vector3 ObstacleScale;
        [SerializeField] private float MoveSpeed;
        [Tooltip("Unscaled speed\t=> Always same elapse time\nScaled speed\t=> Always same motion speed")]
        [SerializeField] private bool ScaleSpeed;
        public enum MotionType
        {
            Linear,
            EasyOut,
            EasyIn,
            EasyInOut
        }
        [SerializeField] private MotionType MoType;
        private float timer;
        private float elapseTime;
        private float moveLerp;

        private Transform obstacle_T;

        private void Awake()
        {
#if UNITY_EDITOR
            hideGizmos = true;
#endif
        }

        private void OnEnable()
        {
            timer = 0f;
            elapseTime = 1f / this.MoveSpeed;
            elapseTime *= (this.ScaleSpeed) ? Vector3.Distance(ObstacleStart_T.position, ObstacleEnd_T.position) : 1f;
            obstacle_T = GameObject.Instantiate(ObstaclePrefab_Obj, ObstacleStart_T.position, this.transform.rotation, this.transform).transform;
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

        private void FixedUpdate()
        {
            timer += Time.deltaTime;
            if(2 * elapseTime < timer)
            {
                timer -= 2 * elapseTime;
            }

            moveLerp = timer / elapseTime;
            if (1 < moveLerp)
                moveLerp = 2 - moveLerp;
            GetLerp(ref moveLerp);

            obstacle_T.position = Vector3.Lerp(ObstacleStart_T.position, ObstacleEnd_T.position, moveLerp);
        }

        private Vector3 GetNewScale()
        {
            Vector3 nScale = this.ObstacleScale;
            nScale.x *= this.ObstaclePrefab_Obj.transform.localScale.x;
            nScale.y *= this.ObstaclePrefab_Obj.transform.localScale.y;
            nScale.z *= this.ObstaclePrefab_Obj.transform.localScale.z;

            return nScale;
        }

        private void GetLerp(ref float lerp) //You can find parabola graphs in this link https://www.desmos.com/calculator/lo6568bucy
        {
            switch (MoType)
            {
                case MotionType.EasyOut:
                    lerp = 2 * lerp - lerp * lerp;
                    break;
                case MotionType.EasyIn:
                    lerp = lerp * lerp;
                    break;
                case MotionType.EasyInOut:
                    lerp = lerp * lerp * (3.0f - 2.0f * lerp);
                    break;
                default:
                    break;
            }
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
                    Gizmos.DrawWireMesh(this.ObstaclePrefab_Obj.GetComponent<MeshFilter>().sharedMesh, gizmosPos, this.transform.rotation, gizmosScale);
                    break;
                case GizmosType.Mesh:
                    Gizmos.DrawMesh(this.ObstaclePrefab_Obj.GetComponent<MeshFilter>().sharedMesh, gizmosPos, this.transform.rotation, gizmosScale);
                    break;
                default:
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            if (hideGizmos || this.ObstaclePrefab_Obj == null || this.ObstacleStart_T == null || this.ObstacleEnd_T == null)
                return;

            gizmosScale = GetNewScale();

            DrawObstacle(Color.blue, this.ObstacleStart_T.position);
            DrawObstacle(Color.green, this.ObstacleEnd_T.position);

            Vector3 posLerp = Vector3.Lerp(this.ObstacleStart_T.position, this.ObstacleEnd_T.position, this.GizmosLerp);
            DrawObstacle(Color.cyan, posLerp);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.ObstacleStart_T.position, this.ObstacleEnd_T.position);
        }
#endif
        #endregion
    }
}

