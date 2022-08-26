using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClosetSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;
    [SerializeField] private ClothingItem item;

    private bool isBought;

    private void Start() 
    {
        itemImage.sprite = item.icon;
        itemImage.color = item.clothingColor;
        priceText.text = "$" + item.value.ToString();

        // Add extra offset if its a shoe sprite
        if (item.type == ClothingType.Shoes) {
            itemImage.rectTransform.position = new Vector2(itemImage.rectTransform.position.x, itemImage.rectTransform.position.y + 0.55f);
        }
    }

    private void Buy()
    {
        // If the player has enough money buy it
        if (PlayerController.instance.Money - item.value > 0) {
            PlayerController.instance.Money -= item.value;
            buttonText.text = "Equip";
            priceText.text = "";
            isBought = true;
        }
    }

    private void Equip()
    {
        // Check the type of the item and assign to correct spot
        if (item.type == ClothingType.Shirt)
            PlayerController.instance.shirt = item;
        else
            PlayerController.instance.shoes = item;

        PlayerController.instance.UpdateClothing();
    }

    public void Pressed() {
        if (isBought)
            Equip();
        else
            Buy();
    }
}
