using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RunnerBoi.Navigation
{
    public class NavMeshGenerator : MonoBehaviour
    {
        public static NavMeshGenerator Instance { get; private set; }

        public List<NavMeshBuildSource> nmSources = new List<NavMeshBuildSource>();

        private NavMeshData nmData;
        private NavMeshDataInstance nmDataInstance;
        private NavMeshBuildSettings nmBuildSettings;        
        private Bounds nmBounds;
        private Vector3 nmBoundSize = new Vector3(80.0f, 20.0f, 80.0f);

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            nmData = new NavMeshData();
            nmDataInstance = NavMesh.AddNavMeshData(nmData);
            nmBuildSettings = NavMesh.GetSettingsByID(0);
            nmBounds = QuantizedBounds();
        }

        public void UpdateNavMesh()
        {
            NavMeshBuilder.UpdateNavMeshData(nmData, nmBuildSettings, nmSources, nmBounds);
        }

        private Bounds QuantizedBounds()
        {
            return new Bounds(Quantize(transform.position, 0.1f * nmBoundSize), nmBoundSize);
        }

        private Vector3 Quantize(Vector3 v, Vector3 quant)
        {
            float x = quant.x * Mathf.Floor(v.x / quant.x);
            float y = quant.y * Mathf.Floor(v.y / quant.y);
            float z = quant.z * Mathf.Floor(v.z / quant.z);
            return new Vector3(x, y, z);
        }
    }
}

