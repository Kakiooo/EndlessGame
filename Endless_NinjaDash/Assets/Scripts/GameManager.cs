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
    [SerializeField] private bool canSpawnEnemy;
    [SerializeField]private Transform[]enemySpawnPoints = new Transform[4];
    [SerializeField]private GameObject[]enemyType=new GameObject[3];
    public enum Wave { wave_1, wave_2,wave_3}
    public Wave wave;
    private PlayerMovement refToPlayerScript;
    public Camera refToCM;
    [SerializeField] private float spawnDelay;
    private int spawnPointIndex,enemyIndex;
    
    private void Awake()
    {
        instance = this;//connect to this script
        gameFlow = GameFlow.gameStart;
        refToCM=GameObject.Find("Main Camera").GetComponent<Camera>();
        refToPlayerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        spawnDelay = 10;

    }

    void Start()
    {
        
    }


    void Update()
    {
        if (canSpawnEnemy)
        {
            switch (wave)
            {
                case Wave.wave_1:
                    StartCoroutine(waveSpawn_Learning(3,0));
                    break;
                case Wave.wave_2:
                    StartCoroutine(waveSpawn_Learning(3,1));
                    break;
                case Wave.wave_3:
                    StartCoroutine(waveSpawn(5));
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
            print(i);
        }
        yield return new WaitForEndOfFrame();
        canSpawnEnemy = false;
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
        canSpawnEnemy = false;
        yield return new WaitForSeconds(spawnDelay);  
        canSpawnEnemy=true;
       
    }
}
