using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scripts.Game
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactionRange = 3f;
        private Item nearbyItem;
        public GameObject pickupPrompt; // Assign in Inspector

        void Update()
        {
            CheckForItem();

            if (Input.GetKeyDown(KeyCode.E) && nearbyItem != null)
            {
                PickUpItem();
            }
        }

        void CheckForItem()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);
            nearbyItem = null;

            pickupPrompt.SetActive(false); // Hide prompt by default

            foreach (Collider collider in colliders)
            {
                Item item = collider.GetComponent<Item>();
                if (item != null)
                {
                    nearbyItem = item;
                    pickupPrompt.SetActive(true); // Show prompt
                    break;
                }
            }
        }

        public void PickUpItem()
        {
            if (nearbyItem == null)
            {
                Debug.LogWarning("No item to pick up.");
                return;
            }

            Debug.Log("Picked up: " + nearbyItem.itemName);

            // Perform actions like adding to inventory
            AddToInventory(nearbyItem);

            // Destroy the item in the scene
            Destroy(nearbyItem.gameObject);
        }

        void AddToInventory(Item item)
        {
            // Implement inventory logic here
            // Example: inventory.Add(item);
            Debug.Log(item.itemName + " added to inventory.");
        }
    }
}