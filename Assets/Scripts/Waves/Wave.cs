using UnityEngine;

public partial class WaveSpawner
{
    [System.Serializable]
    public class Wave
    {
        public Transform enemy;
        public int count;
        public float rate;
    }
}
