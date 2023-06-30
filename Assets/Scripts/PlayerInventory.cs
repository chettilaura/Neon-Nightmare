using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int numberOfCollectibles { get; private set; }

    public void CollectiblesCollected()
    {
        numberOfCollectibles++;
    }
}
