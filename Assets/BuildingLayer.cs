using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class BuildingLayer : MonoBehaviour
    {
        [Header("Connect Settings")]
        [SerializeField] private int layerIndex;
        public int LayerIndex => layerIndex;

        [Header("Content Settings")]
        [SerializeField] private List<GameObject> floor = new List<GameObject>();

        [Space]
        [SerializeField] private Transform hideGroup;
        [SerializeField] private List<MeshRenderer> hideRenderers = new List<MeshRenderer>();

        [Space]
        [SerializeField] private Transform layerContentGroup;
        [SerializeField] private List<MeshRenderer> layerContentRenderers = new List<MeshRenderer>();

        private void Awake()
        {
            if (hideGroup != null)
                hideRenderers.AddRange(hideGroup.GetComponentsInChildren<MeshRenderer>());

            if (layerContentGroup != null)
                layerContentRenderers.AddRange(layerContentGroup.GetComponentsInChildren<MeshRenderer>());
        }

        public void InitializationLayer(int newIndex)
        {
            layerIndex = newIndex;
        }

        [ContextMenu("Hide Layer")]
        public void HideFloor()
        {
            for (int i = 0; i < floor.Count; i++)
            {
                if (floor[i] == null)
                    continue;

                floor[i].SetActive(true);
            }
        }

        [ContextMenu("Show Layer")]
        public void ShowFloor()
        {
            for (int i = 0; i < floor.Count; i++)
            {
                if (floor[i] == null)
                    continue;

                floor[i].SetActive(false);
            }
        }

        [ContextMenu("Show Walls")]
        public void ShowWalls()
        {
            for (int i = 0; i < hideRenderers.Count; i++)
            {
                if (hideRenderers[i] == null)
                    continue;

                hideRenderers[i].enabled = false;
            }
        }

        [ContextMenu("Hide Walls")]
        public void HideWalls()
        {
            for (int i = 0; i < hideRenderers.Count; i++)
            {
                if (hideRenderers[i] == null)
                    continue;

                hideRenderers[i].enabled = true;
            }
        }
    }
}