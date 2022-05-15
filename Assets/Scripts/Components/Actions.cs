using UnityEngine;
using System;

namespace RunnerBoi
{
    public class Actions : MonoBehaviour
    {
        #region Singleton
        public static Actions Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        public Action<GameState> OnGameStateChange;
        public Action<int> OnPositionChange;
        public Action<Vector2> OnPlayerInput;
        public Action<int> OnPaintPercantageChange;
    }
}

