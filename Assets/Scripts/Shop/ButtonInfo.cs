using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public Text Price;
    public Text Name;
    public GameObject ShopManager;
    public int HealingAmount;
    public ItemType Type;
    void Update()
    {
        if(Type==ItemType.HealthPotion)
        {
            if (PlayerHealth.instance.currentHealth == PlayerHealth.instance.maxHealth || PlayerInventory.instance.gold<int.Parse(Price.text))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else this.GetComponent<Button>().interactable = true;
        }
    }
}
