using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunnerBoi;

namespace RunnerBoi.Component
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target_T;
        [SerializeField] private Quaternion defaultRotation;

        private float baseOffset;
        private float upY;
        private Vector3 targetPos;

        private void Awake()
        {
            baseOffset = -18;
            upY = 12;
        }

        private void Start()
        {
            Actions.Instance.OnGameStateChange += HandleGameStateChange;
        }

        private void FixedUpdate()
        {
            if (wallpaintStage) return;

            targetPos.y = target_T.position.y + upY;
            targetPos.z = target_T.position.z + baseOffset;
            targetPos.x = target_T.position.x;
            transform.position = targetPos;
        }

        //
        private bool wallpaintStage = false;
        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.OnReady:
                    wallpaintStage = false;
                    this.transform.rotation = defaultRotation;
                    break;
                case GameState.Racing:
                    wallpaintStage = false;
                    this.transform.rotation = defaultRotation;
                    break;
                case GameState.RaceFinished:
                    wallpaintStage = true;
                    break;
                case GameState.Wallpaint:
                    wallpaintStage = true;
                    break;
                default:
                    break;
            }
        }
    }
}

