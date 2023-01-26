using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private List<Layer> layers = new List<Layer>();
        public int LayersCount => layers.Count;

        public int LayerIndex
        {
            get => layerIndex;
            set
            {
                //Debug.Log("Value - " + value);
                if (value < 0 || value > layers.Count - 1)
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
                }

                ShowToIndex(newIndex);
            }
        }

        [SerializeField] private int layerIndex = -1;

        private void Awake()
        {
            layers.AddRange(GetComponentsInChildren<Layer>());
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].InitializationLayer(this, i);
            }
        }

        private void HideAll()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].HideFloor();
                layers[i].HideObjects();
            }
        }

        private void ShowToIndex(int index)
        {
            int newIndex = Mathf.Clamp(index, 0, layers.Count - 1);
            for (int i = layers.Count - 1; i > newIndex - 1; i--)
            {
                if (i > 0)
                    layers[i].ShowFloor();

                layers[i].ShowObjects();
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

            int layer = -1;
            layer = Mathf.RoundToInt(position.y / layerHieght);
            return layer;
        }

        [SerializeField] private bool isActive = false;
        [SerializeField] private ThirdPersonController personController;
        [SerializeField] private float layerHieght;

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
            }
        }
    }
}