using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private RectTransform ui_healthBar;
    private PlayerMovement refToPlayer;

    private void Awake()
    {
        ui_healthBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        refToPlayer = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    void Start()
    {
        ui_healthBar.DOSizeDelta(new Vector2(0, 25), 100/ refToPlayer.decayTime, false).SetEase(Ease.Linear);//connect health bar UI to player health
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
