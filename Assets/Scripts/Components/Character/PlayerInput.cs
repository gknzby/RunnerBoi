using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunnerBoi;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float touchSensitivity = 1f;

    private Vector3 lastV;
    private Vector2 dif;
    private Vector2 oldDif;
    private float tolerance;

    private void Awake()
    {
        dif = new Vector2(0, 0);
        oldDif = new Vector2(0, 0);
        tolerance = 0.05f;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lastV = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            dif = Input.mousePosition - lastV;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            dif.Set(0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (tolerance < Vector2.Distance(oldDif,dif))
        {
            oldDif = dif;
            Actions.Instance.OnPlayerInput?.Invoke(dif * touchSensitivity);
        }
        lastV = Input.mousePosition;
    }
}
