using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private MapSO mapInfo;
    [SerializeField] private GameObject prefabCharacter;

    [SerializeField] private GameObject finalBoss;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject rewardBox;

    public MapSO mapInfo_ => mapInfo;
    void Awake()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        mapInfo = MapInfo.instance.GetMapInfo();
        InstantiatePlayer(prefabCharacter);
        InstantiateBoss(mapInfo.mapBoss_);
        InstantiateBoss(mapInfo.mapMiniBoss_);
        Debug.Log("Time.TimeScale = " + Time.timeScale);

        InstantiateRewardBox(rewardBox);
    }

    void Start()
    {
        TimerInBattle.instance.SetDuration(mapInfo.mapTime_);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCountdownTimer();
        CheckBattle();
    }

    //khởi tạo BOSS
    public void InstantiateBoss(List<GameObject> _boss)
    {
        if(_boss != null)
        {
            for (int i = 0; i < _boss.Count; i++)
            {
                GameObject boss = Instantiate(_boss[i]);
                boss.name = _boss[i].name;
                if (_boss[i] == mapInfo.mapBoss_[mapInfo.mapBoss_.Count - 1])
                {
                    finalBoss = boss;
                }
            }
        }
    }

    //khởi tạo nhân vật
    public void InstantiatePlayer(GameObject _character)
    {
        player = Instantiate(_character);
        player.name = _character.name;
    }

    //bắt đầu bấm giờ
    public void StartCountdownTimer()
    {
        if(!CameraController.instance.introduceBoss_ && !TimerInBattle.instance.timerIsRunning_)
        {
            TimerInBattle.instance.TimerSwitch(true);
        }
    }

    public void InstantiateRewardBox(GameObject _rewardBox){
        Vector3 position = new Vector3(finalBoss.transform.position.x, finalBoss.transform.position.y + 3f, finalBoss.transform.position.z);
        Instantiate(_rewardBox, position, Quaternion.identity);
    }

    //kiểm tra kết quả trận đấu
    public void CheckBattle()
    {
        if(player.GetComponent<PlayerCombat>().currentHealth_ <= 0 || TimerInBattle.instance.timeOut_)
        {
            TimerInBattle.instance.TimerSwitch(false);
            finalBoss.GetComponent<PandoraController>().StopMechanics();
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerCombat>().enabled = false;
            UIController.instance.OnAlertLose();
        }
    }
}
