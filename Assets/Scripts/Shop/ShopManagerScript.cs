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
        if (PlayerInventory.instance.gold >= int.Parse(info.Price.text))
        {
            PlayerInventory.instance.SubtractGold(int.Parse(info.Price.text));
            if (info.Type == ButtonInfo.ItemType.HealthPotion)
            {
                PlayerHealth.instance.Heal(ButtonRef.GetComponent<ButtonInfo>().HealingAmount);
            }
        }
    }
}
