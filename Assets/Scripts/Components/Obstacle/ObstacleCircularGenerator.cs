using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Component
{
    public class ObstacleCircularGenerator : MonoBehaviour
    {
        [Header("Obstacle Properties")]
        [SerializeField] private GameObject ObstaclePrefab_Obj;
        [SerializeField] private Transform ObstacleCenter_T;
        [SerializeField] private Transform ObstacleOnCircle_T;
        [SerializeField] private ObstacleType obstacleType;
        [SerializeField] private Vector3 ObstacleScale;
        [SerializeField] private float AngularSpeed;

        public enum MotionType
        {
            Linear,
            EasyOut,
            EasyIn,
            EasyInOut
        }
        [SerializeField] private MotionType MoType;

        [SerializeField] private bool Clockwise = false;
        private int rotateDir;

        public float timer;
        public float elapseTime;
        public float moveLerp;

        private float oldRotation = 0;
        private float newRotation = 0;

        private Transform obstacle_T;
        private void Awake()
        {
#if UNITY_EDITOR
            hideGizmos = true;
#endif
        }

        private void OnEnable()
        {
            rotateDir = (Clockwise) ? -1 : 1;
            timer = 0f;
            elapseTime = 360f / this.AngularSpeed;
            ObstacleCenter_T.LookAt(ObstacleOnCircle_T);
            obstacle_T = GameObject.Instantiate(this.ObstaclePrefab_Obj, this.ObstacleOnCircle_T.position, this.transform.rotation, this.ObstacleCenter_T).transform;

            obstacle_T.localScale = GetNewScale();
            obstacle_T.LookAt(ObstacleCenter_T);

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
            GetLerp(ref moveLerp);

            newRotation = moveLerp * 360;
            this.ObstacleCenter_T.Rotate(ObstacleCenter_T.up, rotateDir * (newRotation - oldRotation));
            oldRotation = newRotation;
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
            timer += Time.deltaTime;
            if (elapseTime < timer)
            {
                timer -= elapseTime;
            }

            lerp = timer / elapseTime;

            switch (MoType)
            {
                case MotionType.EasyOut:
                    lerp = 2 * lerp - lerp * lerp;
                    break;
                case MotionType.EasyIn:
                    lerp *= lerp;
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

        private float gizmosRot = 0;
        private float gizmosDistance;
        private void OnDrawGizmos()
        {
            
            if (hideGizmos || this.ObstaclePrefab_Obj == null || this.ObstacleCenter_T == null || this.ObstacleOnCircle_T == null)
                return;

            gizmosRot = GizmosLerp * 360;
            gizmosRot *= (Clockwise) ? -1 : 1; 
            this.ObstacleCenter_T.LookAt(this.ObstacleOnCircle_T.position);
            this.ObstacleCenter_T.Rotate(this.ObstacleCenter_T.up, gizmosRot);

            gizmosScale = GetNewScale();
            gizmosDistance = Vector3.Distance(this.ObstacleCenter_T.position, this.ObstacleOnCircle_T.position);
            
            Vector3 drawPos = this.ObstacleCenter_T.forward.normalized * gizmosDistance;
            drawPos += this.ObstacleCenter_T.position;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(ObstacleCenter_T.position, 0.5f);
            Gizmos.DrawWireSphere(this.ObstacleCenter_T.position, gizmosDistance);

            DrawObstacle(Color.blue, drawPos);
        }
#endif
    #endregion
    }
}

