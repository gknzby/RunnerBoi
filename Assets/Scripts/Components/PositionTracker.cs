using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RunnerBoi;

namespace RunnerBoi.Workers
{
    public class PositionTracker : MonoBehaviour
    {
        private List<Transform> racers_T;
        private Transform player_T;
        private Transform finish_T;

        private int position = 0;

        private void OnEnable()
        {
            racers_T = Managers.LevelManager.Instance.Data.Runners_T;
            player_T = Managers.LevelManager.Instance.Data.Player_T;
            finish_T = Managers.LevelManager.Instance.CurrentLevel.FinishPos_T;
        }

        private void FixedUpdate()
        {
            int nPos = 1;

            for(int i = 0; i < racers_T.Count; i++)
            {
                if(finish_T.position.z - racers_T[i].position.z < finish_T.position.z - player_T.position.z)
                    nPos++;
            }

            if(nPos != position)
            {
                Actions.Instance.OnPositionChange?.Invoke(nPos);
                position = nPos;
            }
        }
    }
}

