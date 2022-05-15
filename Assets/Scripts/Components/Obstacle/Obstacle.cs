using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerBoi.Component
{
    public enum ObstacleType
    {
        Kill,
        Sticky
    }

    [System.Serializable]
    public abstract class Obstacle : MonoBehaviour
    {

    }
}