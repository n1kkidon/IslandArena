using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public void Buy()
    {
        GameObject ButtonRef=GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo info = ButtonRef.GetComponent<ButtonInfo>();
        if (info.Type==ItemType.OwnedWeapon || PlayerInventory.instance.gold >= int.Parse(info.Price.text))
        {
            if(info.Type==ItemType.HealthPotion || info.Type == ItemType.Weapon)
            {
                PlayerInventory.instance.SubtractGold(int.Parse(info.Price.text));
            }
            if (info.Type == ItemType.HealthPotion)
            {
                PlayerHealth.instance.Heal((ButtonRef.GetComponent<ButtonInfo>().item as HealthPotion).healingAmount);
            }
            if (info.Type==ItemType.Weapon)
            {
                info.Type = ItemType.OwnedWeapon;
                info.Price.text = "Owned";
                return;
            }
            if(info.Type==ItemType.OwnedWeapon)
            {
                info.Type = ItemType.EquipedWeapon;
                PlayerMovement.instance.UnequipWeapon();
                PlayerMovement.instance.EquipWeapon((Weapon)info.item);
                info.Price.text = "Equiped";
            }
        }
    }
}
