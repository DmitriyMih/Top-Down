using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownAim : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform player;
    [SerializeField] private Transform target;

    private void Update()
    {
        if(target!=null)
        {
        }

        if (lineRenderer != null)
            //if (Input.GetMouseButton(1))
            {
                lineRenderer.positionCount = 2;

                if (player != null)
                    lineRenderer.SetPosition(0, player.position);

                if (target != null)
                    lineRenderer.SetPosition(1, target.position);
            }
    }
}
