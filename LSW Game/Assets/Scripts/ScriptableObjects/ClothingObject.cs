using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingType
{
    Shirt,
    Shoes
}

[CreateAssetMenu(fileName = "Clothing", menuName = "ScriptableObjects/ClothingObject", order = 1)]
public class ClothingObject : ScriptableObject
{
    public string clothingName;
    [TextArea] public string description;
    public int price;

    public ClothingType type;
    public GameObject itemPrefab;
}
