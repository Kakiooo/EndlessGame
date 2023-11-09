using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;//creating singleton
    public enum GameFlow {gameStart,gamePause,gameEnd}//setting game flow
    public GameFlow gameFlow;
    public enum Wave {wave_1,wave_2,wave_3, wave_4 };
    public Wave[] wave=new Wave [4];
    public Wave currentWave;
    [SerializeField] private bool canSpawnEnemy;
    [SerializeField]private Transform[]enemySpawnPoints = new Transform[4];
    [SerializeField]private GameObject[]enemyType=new GameObject[3];
    private PlayerMovement refToPlayerScript;
    private int wave_Index,num_Enemy;
    public Camera refToCM;
    [SerializeField] private float spawnDelay,timer;
    private int spawnPointIndex,enemyIndex;
    
    private void Awake()
    {
        instance = this;//connect to this script
        gameFlow = GameFlow.gameStart;
        refToCM=GameObject.Find("Main Camera").GetComponent<Camera>();
        refToPlayerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        spawnDelay = 5;
        timer = spawnDelay;
        num_Enemy = 4;
    }

    void Start()
    {
    }


    void Update()
    {
        currentWave = wave[wave_Index];//load in the value of wave
        if (canSpawnEnemy)
        {
            switch (currentWave)//define the enemy instantiate function according to wave
            {
                case Wave.wave_1:
                    StartCoroutine(waveSpawn_Learning(3, 0));
                    break;
                case Wave.wave_2:
                    StartCoroutine(waveSpawn_Learning(3, 1));
                    break;
                case Wave.wave_3:
                    StartCoroutine(waveSpawn_Learning(3, 2));
                    break;
                case Wave.wave_4:
                    StartCoroutine(waveSpawn(num_Enemy));
                    break;
            }
        }


    }

    private IEnumerator waveSpawn_Learning(int num_Enemy,int index_EnemyType)
    {
        for (int i = 0; i < num_Enemy; i++)
        {
            spawnPointIndex = Random.Range(0, enemySpawnPoints.Length);
            Instantiate(enemyType[index_EnemyType], enemySpawnPoints[spawnPointIndex].position, Quaternion.identity);
        }
        yield return new WaitForEndOfFrame();
        canSpawnEnemy = false;
        wave_Index += 1;//switching the wave
        yield return new WaitForSeconds(spawnDelay);
        canSpawnEnemy = true;

    }

    private IEnumerator waveSpawn(int num_Enemy)
    {      
        for (int i=0; i<num_Enemy; i++)
        {
            enemyIndex=Random.Range(0, enemyType.Length);
            spawnPointIndex = Random.Range(0, enemySpawnPoints.Length);
            Instantiate(enemyType[enemyIndex], enemySpawnPoints[spawnPointIndex].position, Quaternion.identity);           
            print(i);
        }
        yield return new WaitForEndOfFrame();
        wave_Index = wave.Length-1;//last state of wave is repeating until the end of game
        canSpawnEnemy = false;
        spawnDelay = 10;
        yield return new WaitForSeconds(spawnDelay);  
        canSpawnEnemy=true;
       
    }


}
