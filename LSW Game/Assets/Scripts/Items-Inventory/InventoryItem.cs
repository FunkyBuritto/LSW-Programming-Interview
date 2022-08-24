using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericItem", menuName = "ScriptableObjects/GenericItem", order = 1)]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    [TextArea] public string itenDescription;
    public Sprite icon;
}
