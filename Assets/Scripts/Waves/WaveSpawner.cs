using Assets.Scripts.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static WaveSpawner;

public partial class WaveSpawner : MonoBehaviour, IDataPersistence
{

    public Wave[] Waves;
    private int nextWave = 0;
    private int enemiesAlive = 0;
    public Transform[] SpawnPoints;
    public float timeBetweenWaves = 5f;
    private float waveCountDown;
    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING;
    public PlayerScreenUI ScreenUI;

    [SerializeField]
    private Transform bossEnemyPrefab;

    void Start()
    {
        waveCountDown = timeBetweenWaves;
    }
    void Update()
    {
        if (state == SpawnState.FINISHED)
            return;

        ScreenUI.SetWave(nextWave+1, enemiesAlive, waveCountDown);
        if(state== SpawnState.WAITING)
        {
            if(!EnemyIsAlive())
            {
                WaveCompleted();
                if (state == SpawnState.FINISHED)
                {
                    ScreenUI.SetWave(1, 1, -1f);
                    return; 
                }
            }
            else
            {
                return;
            }
        }
        if(waveCountDown <= 0)
        {
            if(state!= SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(Waves[nextWave]));
            }
        }
        else
        {
            waveCountDown-=Time.deltaTime;
        }

        // Cheat
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SpawnBossEnemy();
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed");
        DataPersistenceManager.Instance.SaveGame();
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if ((nextWave + 1) % 5 == 0)
        {
            // Spawn a boss enemy
            SpawnBossEnemy();
        }

        if (nextWave + 1 > Waves.Length - 1)
        {
            Debug.Log("All waves completed");
            state = SpawnState.FINISHED;
            GetComponent<GameManager>().GameComplete();
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown<= 0)
        {
            searchCountdown = 1f;
            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemiesAlive==0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave: "+nextWave);
        state = SpawnState.SPAWNING;
        for(int i=0; i<_wave.counts.Length; i++)
        {
            for (int j = 0; j < _wave.counts[i]; j++)
            {
                SpawnEnemy(_wave.enemies[i]);
                yield return new WaitForSeconds(1f / _wave.rate);
            }
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning enemy: " + _enemy.name);
        Transform _sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Instantiate(_enemy, _sp.position, Quaternion.identity);
        enemiesAlive++;
    }

    void SpawnBossEnemy()
    {
        Debug.Log("Spawning boss enemy");
        Transform _sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Transform bossEnemy = Instantiate(bossEnemyPrefab, _sp.position, Quaternion.identity);
        bossEnemy.localScale = new Vector3(3f, 3f, 3f);

        bossEnemy.GetComponent<Enemy>().maxHealth = 300f;
        bossEnemy.GetComponent<Enemy>().attackDamage = 40;

        float bossEnemyHealth = bossEnemy.GetComponent<Enemy>().maxHealth;
        ScreenUI.UpdateBossEnemyHealth(bossEnemyHealth);
        enemiesAlive++;
    }

    public void LoadData(GameData data)
    {
        state = data.state;
        nextWave = data.currentWave;
        ScreenUI.SetWave(1, 1, -1f);
    }

    public void SaveData(ref GameData data)
    {
        data.state = state;
        if(EnemyIsAlive() && nextWave != 0)
            data.currentWave = nextWave -1;
        else data.currentWave = nextWave;
    }
}
