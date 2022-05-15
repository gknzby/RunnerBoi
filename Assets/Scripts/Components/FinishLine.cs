using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(RunnerBoi.ObjTags.Character.ToString()))
        {
            RunnerBoi.Managers.LevelManager.Instance.RunnerFinishedRace(other.transform);
        }
    }
}
