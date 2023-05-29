using System;
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
        public int gold;
        public int level;
        public int experience;
        public int expToNextLevel;
        public int levelPointsUsed;
        public int levelPointsAvailable;
        public string SkillTreeDictionaryJson;
        public int[] itemType=new int[10];
        public string currentWeapon;
    }
}
