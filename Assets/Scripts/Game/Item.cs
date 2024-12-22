using UnityEngine;

namespace Unity.Scripts.Game
{
    public class Item : MonoBehaviour
    {
        public string itemName;
        public Sprite itemIcon; // Optional, for inventory UI
        public int itemID;

        // Optional: Add other properties such as weight, durability, etc.
    }
}
