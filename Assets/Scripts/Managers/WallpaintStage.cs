using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RunnerBoi.Paint
{
    public class WallpaintStage : MonoBehaviour
    {
        [SerializeField] private Transform PlayerPos_T;
        [SerializeField] private Transform Sacrifice_T;
        [SerializeField] private Transform Wallpaint_T;

        [SerializeField] private float ChangeTime;

        private Transform player_T;
        private Transform camera_T;

        [SerializeField] Transform[] cameraBezier_T;

        private void Start()
        {
            Actions.Instance.OnGameStateChange += HandleGameStateChange;
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.RaceWon:
                    OnRaceWon();
                    break;
                case GameState.Wallpaint:
                    OnWallPaint();
                    break;
                default:
                    break;
            }
        }

        private void OnRaceWon()
        {
            player_T = Managers.LevelManager.Instance.Data.Player_T;
            camera_T = Camera.main.transform;

            StartCoroutine(CameraMove());
            StartCoroutine(PlayerMove());
        }

        private IEnumerator PlayerMove()
        {
            float timer = 0;
            Vector3 firstPos = player_T.position;
            Vector3 jackal = PlayerPos_T.position;
            jackal.y = 0;
            player_T.LookAt(jackal);

            player_T.GetComponent<Character.CharacterMovement>().playerAnimator.SetFloat("RunSpeed", 0.5f);
            while(timer < ChangeTime - 0.3f)
            {
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;

                player_T.position = Vector3.Lerp(firstPos, PlayerPos_T.position, timer / (ChangeTime - 0.3f));
            }

            jackal = cameraBezier_T[3].position;
            jackal.y = 0;
            Quaternion firstLook = player_T.rotation;
            player_T.LookAt(jackal);
            Quaternion aimLook = player_T.rotation;
            player_T.rotation = firstLook;
            timer = 0;

            player_T.GetComponent<Character.CharacterMovement>().playerAnimator.SetFloat("RunSpeed", 0f);
            while (timer < 0.3f)
            {
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;

                player_T.rotation = Quaternion.Lerp(firstLook, aimLook, timer / 0.3f);
            }
            player_T.LookAt(jackal);
        }

        private Vector3 xVec, yVec, zVec, tVec;
        private void SetBezierCurvePos(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            xVec = (3 * b) - (3 * c) + d - a;
            yVec = (3 * c) - (6 * b) + (3 * a);
            zVec = (3 * b) - (3 * a);
            tVec = a;
        }

        private Vector3 GetBezierPos(float lerp)
        {
            Vector3 sum;

            sum = xVec;
            sum = sum * lerp + yVec;
            sum = sum * lerp + zVec;
            sum = sum * lerp + tVec;            

            return sum;
        }

        private IEnumerator CameraMove()
        {
            float timer = 0;
            float elapseTime = ChangeTime;

            cameraBezier_T[0].position = camera_T.position;

            SetBezierCurvePos(cameraBezier_T[0].position, cameraBezier_T[1].position, cameraBezier_T[2].position, cameraBezier_T[3].position);

            Sacrifice_T.position = camera_T.position;
            Sacrifice_T.LookAt(Wallpaint_T.position);
            Quaternion aimTo = Sacrifice_T.rotation;
            Quaternion first = Camera.main.transform.rotation;

            while(timer < 1f)
            {
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;
                camera_T.rotation = Quaternion.Lerp(first, aimTo, timer/2);
            }

            timer = 0;
            while(timer < elapseTime)
            {
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;

                camera_T.position = GetBezierPos(timer / elapseTime);
                camera_T.LookAt(Wallpaint_T.position);
            }

            Managers.GameManager.Instance.PaintWall();
        }
        

        private void OnWallPaint()
        {

        }

#if UNITY_EDITOR
        [Header("Gizmos")]
        [SerializeField] private GameObject GizmosMesh;
        [Range(0f, 1f)]
        [SerializeField] private float GizmosLerp;
        private void OnDrawGizmos()
        {
            if (cameraBezier_T == null || cameraBezier_T.Length < 4)
                return;

            Gizmos.color = Color.cyan;
            SetBezierCurvePos(cameraBezier_T[0].position, cameraBezier_T[1].position, cameraBezier_T[2].position, cameraBezier_T[3].position);
            Vector3 gizmosPos = GetBezierPos(GizmosLerp);

            Gizmos.DrawMesh(GizmosMesh.GetComponent<MeshFilter>().sharedMesh, gizmosPos, this.transform.rotation, Vector3.one);
        }
#endif
    }
}

