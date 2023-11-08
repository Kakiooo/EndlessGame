using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI refToTextMesh, refToTimeInfo;
    public RectTransform ui_healthBar,ui_dashCharge;
    public Image ui_healthBarSprite;
    private PlayerMovement refToPlayerScript;
    private GameObject indicatorDash, mouse, direction,refToPlayer;
    private CinemachineVirtualCamera refToVirtualCM;
    public float duration, totalTime,shakeIntensity=2;
    private float timer_ColorChange, num_Time;
    public int num_eliminated;

    public bool isEnemyDestroied;
    [SerializeField] CinemachineBasicMultiChannelPerlin shakeProperties;


    private void Awake()
    {
        timer_ColorChange = 1;
        ui_healthBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        ui_healthBarSprite = GameObject.Find("HealthBar").GetComponent<Image>();
        refToPlayerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        mouse = GameObject.Find("Mouse");
        indicatorDash = GameObject.Find("DashIndicator");
        direction = GameObject.Find("direction");
        refToPlayer = GameObject.Find("Player");
        refToVirtualCM = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        shakeProperties = refToVirtualCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        totalTime = 3;
        refToTimeInfo = GameObject.Find("TimeSurvive").GetComponent<TextMeshProUGUI>();
        refToTextMesh = GameObject.Find("Text_Enemy").GetComponent<TextMeshProUGUI>();

        //duration = totalTime;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (GameManager.instance.gameFlow==GameManager.GameFlow.gameStart)
        {
           
            refToTextMesh.text = ""+num_eliminated;          
            mouse.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            indicatorDash.transform.position = refToPlayer.transform.position; //dash direction indicator follow the player
            DirectionIndicator();
            CameraEffect();
            HealthBar();
            timeCounting();
            DashCharging();


        }
    }

    private void DirectionIndicator()
    {
        float degree = Mathf.Rad2Deg * Mathf.Atan2(mouse.transform.position.y - indicatorDash.transform.position.y, mouse.transform.position.x - indicatorDash.transform.position.x);
        //indicatorDash.transform.up = mouse.transform.position - indicatorDash.transform.position;
        indicatorDash.transform.localRotation = Quaternion.AngleAxis(degree, Vector3.forward);
    }
    private void DashCharging()
    {
        if (refToPlayerScript.dashPower > 10)
        {
            direction.transform.localScale += new Vector3(0, Time.deltaTime,0);//when charging the direction mark will become larger to indicate stronger dash
            if (refToPlayerScript.dashPower >= 30)
            {
                direction.transform.localScale = new Vector3(1, 3, 0);//limit the maximum size of direction mark
            }
        }
        if (!refToPlayerScript.isDuringCharging)
        {
            direction.transform.localScale = new Vector3(1, 1, 0);//reset to original size,and make player understand the dash is ready
        }
    }
    void timeCounting()
    {
        //num_Time_milliSec += Time.deltaTime;
        //if (num_Time_milliSec >=9)
        //{
        //    num_Time_Second += 1;
        //    num_Time_milliSec = -1;
        //    refToTimeInfo.text = "0" + num_Time_Minute.ToString("F0") + ":" + num_Time_Second.ToString("F0") + "0";
        //}
        //if(num_Time_Second > 5)
        //{
        //    num_Time_Minute += 1;
        //    num_Time_Second = 0;
        //}  
        //refToTimeInfo.text = "0" + num_Time_Minute.ToString("F0") + ":"+ num_Time_Second.ToString("F0")+ num_Time_milliSec.ToString("F0");


    }

    void HealthBar()
    {
          
        if(ui_healthBarSprite.color == Color.red)//by using red to show player health bar is losing
        {
            ui_healthBar.sizeDelta -= new Vector2(refToPlayerScript.decayTime * 15, 0) * Time.deltaTime;//scalling health bar according to players health decay and time
            print("Works");
        }
        if (ui_healthBarSprite.color == Color.green)//by using green to show player restore the health
        {
            timer_ColorChange -= Time.deltaTime;
            if (timer_ColorChange < 0)
            {
                ui_healthBarSprite.color = Color.red;
                timer_ColorChange = 1;
            } //when enemy is eliminated, change color to show health bar got bonus
        }
       

    }

    private void CameraEffect()
    {
        float zoomInTime = 1.5f;
        float zoomOutTime = 3f;
        if (refToPlayerScript.isDuringCharging)//when dashing...
        { 
            refToVirtualCM.m_Lens.OrthographicSize-= zoomInTime * Time.deltaTime;//zoom in virtual camera when player is dashing
            if (refToVirtualCM.m_Lens.OrthographicSize <= 5)
            {
                refToVirtualCM.m_Lens.OrthographicSize=5;//when zoom in camera limit the size of virtual camera
            }
        }
        else //when is not dashing...
        { 
            refToVirtualCM.m_Lens.OrthographicSize += zoomOutTime * Time.deltaTime;//zoom out virtual camera when player is not dashing
            if (refToVirtualCM.m_Lens.OrthographicSize >=8)
            {
                refToVirtualCM.m_Lens.OrthographicSize =8;//when zoom out camera limit the size of virtual camera
            }
        }

    }
    public void CameraShake()
    {        
        shakeProperties.m_FrequencyGain = 1;
        shakeProperties.m_AmplitudeGain = 5;
    }
    public void CameraStopShake()
    {
        shakeProperties.m_AmplitudeGain = 0;
    }


}
