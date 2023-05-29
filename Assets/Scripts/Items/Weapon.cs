using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item, IDataPersistence
{
    public int id;
    public float damage;
    public float speedMultiplier;
    public float attackRange;
    public GameObject weaponObject;
    public bool isBow;
    public void LoadData(GameData data)
    {
        type = (ItemType)data.itemType[id];
        if (type == ItemType.EquipedWeapon)
        {
            PlayerMovement.instance.UnequipWeapon();
            PlayerMovement.instance.EquipWeapon(gameObject.GetComponent<Weapon>());
        }
        Debug.Log(type);
    }

    public void SaveData(ref GameData data)
    {
        data.itemType[id] = (int)type;
    }
}
