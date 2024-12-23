using UnityEngine;

namespace Unity.Scripts.Game
{
    public class Item : MonoBehaviour
    {
        public string itemName;
        public Sprite itemIcon; // Optional, for inventory UI
        public int itemID;

        // Optional: Add other properties such as weight, durability, etc.

        void Start()
        {
            // Initialize item properties
        }

        void Update()
        {
            // Update item state, if needed
        }

        public event System.Action OnVaccineCollected;

        public void OnCollected()
        {
            // Invoke the event when the item is collected
            if (OnVaccineCollected != null)
            {
                OnVaccineCollected.Invoke();
            }
        }
    }
}
