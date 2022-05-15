using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RunnerBoi.Navigation
{
    [RequireComponent(typeof(MeshFilter))]
    public class NavMeshComponent : MonoBehaviour
    {
        NavMeshBuildSource nmSource;

        private bool gameBegin = true;

        private void Start()
        {
            gameBegin = false;
            if(this.gameObject.activeInHierarchy)
                OnEnable();
        }

        private void OnEnable()
        {
            if (gameBegin) return;

            nmSource = new NavMeshBuildSource();
            nmSource.shape = NavMeshBuildSourceShape.Box;
            nmSource.sourceObject = this.GetComponent<MeshFilter>().sharedMesh;
            nmSource.transform = this.transform.localToWorldMatrix;
            nmSource.area = 0;

            NavMeshGenerator.Instance.nmSources.Add(nmSource);
        }

        private void OnDisable()
        {
            NavMeshGenerator.Instance.nmSources.Remove(nmSource);
        }
    }
}

