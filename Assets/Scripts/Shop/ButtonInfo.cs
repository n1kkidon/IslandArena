using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ButtonInfo : MonoBehaviour
{
    public Text Price;
    public Text Name;
    public int price;
    public Item item;
    public GameObject ShopManager;
    private void Start()
    {
        Price.text = price.ToString();
        Name.text = item.itemName;
    }
    void Update()
    {
        if(item.type==ItemType.HealthPotion)
        {
            if (PlayerHealth.instance.currentHealth == PlayerHealth.instance.maxHealth || PlayerInventory.instance.gold<int.Parse(Price.text))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else this.GetComponent<Button>().interactable = true;
        }
        if(item.type == ItemType.EquipedWeapon)
        {
            this.GetComponent<Button>().interactable = false;
        }
        else this.GetComponent<Button>().interactable = true;
    }
}
