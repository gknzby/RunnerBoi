using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunnerBoi;

namespace RunnerBoi.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float vSpeed;
        [SerializeField] private float hSpeed;

        private Rigidbody rb;
        private Vector3 nSpd;
        private Vector3 vel = Vector3.zero;
        [SerializeField] private float smooth = 0.05f;

        [SerializeField] private GameObject PlayerObj;
        public Animator playerAnimator;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            playerAnimator = PlayerObj.GetComponent<Animator>();
        }

        private void Start()
        {
            Actions.Instance.OnGameStateChange += HandleGameStateChange;
        }

        private void FixedUpdate()
        {
            nSpd.y = rb.velocity.y;
            this.rb.velocity = Vector3.SmoothDamp(this.rb.velocity, this.nSpd, ref this.vel, smooth);
        }

        private void HandlePlayerInput(Vector2 xy)
        {
            nSpd.x = hSpeed < xy.x ? hSpeed : (xy.x < -hSpeed ? -hSpeed : xy.x);
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Racing:
                    nSpd.z = vSpeed;
                    Actions.Instance.OnPlayerInput += HandlePlayerInput;
                    playerAnimator.SetFloat("RunSpeed", 1);
                    break;
                case GameState.RaceFinished:
                    nSpd = Vector3.zero;
                    Actions.Instance.OnPlayerInput -= HandlePlayerInput;
                    playerAnimator.SetFloat("RunSpeed", 0);
                    break;
                default:
                    break;
            }
        }
    }

}
