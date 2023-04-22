﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WaveSpawner;

namespace Assets.Scripts.Saving
{
    [Serializable]
    public class GameData
    {
        public float currentHealth;
        public float maxHealth;
        public float[] playerPosition;
        public SpawnState state;
        public int currentWave;
    }
}