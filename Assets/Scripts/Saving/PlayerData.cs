using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Saving
{
    [Serializable]
    public class PlayerData
    {
        public float MaxHealth;
        public float CurrentHealth;
        float[] Location;

        public PlayerData(PlayerHealth hp) 
        {            
            var pos = hp.GetComponent<Transform>().position;
            Location = new float[3];
            Location[0] = pos.x;
            Location[1] = pos.y;
            Location[2] = pos.z;
            MaxHealth = hp.maxHealth;
            CurrentHealth = hp.currentHealth;
        }
        public Vector3 Pos { get { return new Vector3(Location[0], Location[1], Location[2]); } }
    }
}
