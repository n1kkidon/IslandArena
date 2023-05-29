using System;
using System.Collections.Generic;
using UnityEngine;

public partial class WaveSpawner
{
    [System.Serializable]
    public class Wave
    {
        public Transform[] enemies;
        public int[] counts;
        public float rate;
    }
}
