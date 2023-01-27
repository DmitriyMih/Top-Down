using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class Layer : MonoBehaviour
    {
        [Header("Connect Settings")]
        [SerializeField] private int layerIndex;
        public int LayerIndex => layerIndex;

        [Header("Content Settings")]
        [SerializeField] private Transform hideGroup;
        [SerializeField] private List<MeshRenderer> hideRenderers = new List<MeshRenderer>();

        [SerializeField] private List<GameObject> floor = new List<GameObject>();

        private void Awake()
        {
            hideRenderers.AddRange(hideGroup.GetComponentsInChildren<MeshRenderer>());
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

        [ContextMenu("Show Objects")]
        public void ShowObjects()
        {
            for (int i = 0; i < hideRenderers.Count; i++)
            {
                if (hideRenderers[i] == null)
                    continue;

                hideRenderers[i].enabled = false;
            }
        }

        [ContextMenu("Hide Objects")]
        public void HideObjects()
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