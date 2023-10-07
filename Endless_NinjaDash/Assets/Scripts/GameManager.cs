using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;//creating singleton
    public enum GameFlow {gameStart,gamePause,gameEnd}//setting game flow
    public GameFlow gameFlow;
    
    private void Awake()
    {
        instance = this;//bind with this script
        gameFlow = GameFlow.gameStart;
    }

    void Start()
    {

    }


    void Update()
    {
        
    }
}
