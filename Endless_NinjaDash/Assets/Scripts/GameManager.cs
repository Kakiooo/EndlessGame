using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;//creating singleton
    public enum GameFlow {gameStart,gamePause,dashMode,gameEnd}//setting game flow
    public GameFlow gameFlow;
    public Camera refToCM;
    
    private void Awake()
    {
        instance = this;//connect to this script
        gameFlow = GameFlow.gameStart;
        refToCM=GameObject.Find("Main Camera").GetComponent<Camera>();  
    }

    void Start()
    {

    }


    void Update()
    {
    }


}
