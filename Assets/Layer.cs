using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class Layer : MonoBehaviour
    {
        [Header("Connect Settings")]
        [SerializeField] private Building building;

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

        public void HideFloor()
        {
            for (int i = 0; i < floor.Count; i++)
            {
                if (floor[i] == null)
                    continue;

                floor[i].SetActive(true);
            }
        }
        public void ShowFloor()
        {
            for (int i = 0; i < floor.Count; i++)
            {
                if (floor[i] == null)
                    continue;

                floor[i].SetActive(false);
            }
        }

        public void InitializationLayer(Building building, int newIndex)
        {
            this.building = building;
            layerIndex = newIndex;
        }

        //public void SendIndex()
        //{
        //    int index = layerIndex;
        //    if (layerIndex == building.LayersCount - 1)
        //        index = 99;

        //    building.LayerIndex = index;
        //}

        //public void ResetIndex()
        //{
        //    int index = layerIndex;
        //    if (layerIndex == 0)
        //        index = -1;

        //    building.LayerIndex = index;
        //}

        public void ShowObjects()
        {
            for (int i = 0; i < hideRenderers.Count; i++)
            {
                if (hideRenderers[i] == null)
                    continue;

                hideRenderers[i].enabled = false;
            }
        }

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