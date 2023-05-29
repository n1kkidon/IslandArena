using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo info = ButtonRef.GetComponent<ButtonInfo>();
        if (info.item.type==ItemType.OwnedWeapon || PlayerInventory.instance.gold >= int.Parse(info.Price.text))
        {
            if(info.item.type==ItemType.HealthPotion || info.item.type == ItemType.Weapon)
            {
                PlayerInventory.instance.SubtractGold(int.Parse(info.Price.text));
            }
            if (info.item.type == ItemType.HealthPotion)
            {
                PlayerHealth.instance.Heal((ButtonRef.GetComponent<ButtonInfo>().item as HealthPotion).healingAmount);
            }
            if (info.item.type ==ItemType.Weapon)
            {
                info.item.type = ItemType.OwnedWeapon;
                info.Price.text = "Owned";
                return;
            }
            if(info.item.type ==ItemType.OwnedWeapon)
            {
                info.item.type = ItemType.EquipedWeapon;
                PlayerMovement.instance.UnequipWeapon();
                PlayerMovement.instance.EquipWeapon((Weapon)info.item);
                info.Price.text = "Equiped";
            }
        }
    }
}
