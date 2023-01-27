using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private List<BuildingLayer> layers = new List<BuildingLayer>();
        public int LayersCount => layers.Count;

        [Header("Layer Settings")]
        [SerializeField] private bool isActive = false;
        [SerializeField] private ThirdPersonController personController;
        [SerializeField] private float layerHieght;

        public int LayerIndex
        {
            get => layerIndex;
            set
            {
                //Debug.Log("Value - " + value);
                if (value < 0 || value >= layers.Count)
                {
                    HideAll();
                    layerIndex = value;
                    return;
                }

                int newIndex = Mathf.Clamp(value, 0, layers.Count - 1);

                if (newIndex != layerIndex)
                {
                    HideAll();
                    layerIndex = newIndex;
                    ShowToIndex(layerIndex);
                }

            }
        }

        [SerializeField] private int layerIndex = -1;

        private void Awake()
        {
            layers.AddRange(GetComponentsInChildren<BuildingLayer>());

            for (int i = 0; i < layers.Count; i++)
                layers[i].InitializationLayer(i);
        }

        private void HideAll()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].HideFloor();
                layers[i].HideWalls();
            }
        }

        private void ShowToIndex(int index)
        {
            int newIndex = Mathf.Clamp(index, 0, layers.Count - 1);
            for (int i = layers.Count - 1; i > newIndex; i--)
            {
                if (i > 0)
                    layers[i].ShowFloor();

                Debug.Log("Show - " + i);
                if (i > 0)
                    layers[i - 1].ShowWalls();
            }
        }

        private void Update()
        {
            if (!isActive || personController == null)
                return;

            LayerIndex = ConvertToLayer(personController.transform.position);
        }

        private int ConvertToLayer(Vector3 position)
        {
            if (position.y < 0)
                return -99;

            int layer = (int)(position.y / layerHieght);
            return layer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ThirdPersonController personController))
            {
                this.personController = personController;
                isActive = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ThirdPersonController personController))
            {
                this.personController = null;
                isActive = false;
                LayerIndex = -1;
            }
        }
    }
}