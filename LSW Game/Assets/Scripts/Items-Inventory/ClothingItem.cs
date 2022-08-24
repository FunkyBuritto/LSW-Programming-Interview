using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingType
{
    Shirt,
    Shoes
}

[CreateAssetMenu(fileName = "Clothing", menuName = "ScriptableObjects/ClothingItem", order = 2)]
public class ClothingItem : InventoryItem
{
    public int price;
    public ClothingType type;
    public GameObject clothingPrefab;
}
