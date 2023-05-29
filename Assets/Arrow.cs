using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Arrow : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 10);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if(other.gameObject.GetComponent<Enemy>().TakeDamage(PlayerMovement.instance.ArrowDamage(), out var loot))
        {
            PlayerMovement.instance.gameObject.GetComponent<PlayerInventory>().GetMobDrop(loot);
        }
    }
}
