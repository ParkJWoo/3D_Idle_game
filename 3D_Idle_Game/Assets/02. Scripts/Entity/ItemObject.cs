using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = CharacterManager.Instance?.Player;
            if (player == null) return;

            player.itemData = itemData;
            player.addItem?.Invoke();

            Destroy(gameObject);
        }

    }
}
