using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instace { get; private set; }
    [SerializeField] private MapSO mapInfo;
    [SerializeField] private GameObject prefabCharacter;

    [SerializeField] private List<GameObject> bossList;
    [SerializeField] private GameObject currentBoss;

    [SerializeField] private GameObject player;

    [SerializeField] private Transform teleport;

    [SerializeField] private bool resultBattle;
    [SerializeField] private GameObject rewardBox;

    public MapSO mapInfo_ => mapInfo;
    public bool resultBattle_ => resultBattle;
    void Awake()
    {
        if(instace != null && instace != this)
        {
            Destroy(this);
        }
        else
        {
            instace = this;
        }
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        resultBattle = false;
        mapInfo = MapInfo.instance.GetMapInfo();
        InstantiatePlayer(prefabCharacter);
        InstantiateBoss(mapInfo.mapMiniBoss_);
        InstantiateBoss(mapInfo.mapBoss_);
        //Debug.Log("Time.TimeScale = " + Time.timeScale);
        bool a = GameObject.Find("Teleport") ? teleport = GameObject.Find("Teleport").transform : teleport = null;
        if(teleport != null )
        {
            teleport.gameObject.SetActive(false);
        }
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
    private void InstantiateBoss(List<Boss> _boss)
    {
        if(_boss != null)
        {
            for (int i = 0; i < _boss.Count; i++)
            {
                GameObject boss = Instantiate(_boss[i].bossPrefab_);
                boss.GetComponent<BossController>().SetDataForBoss(_boss[i].bossInfo_);
                boss.name = _boss[i].bossPrefab_.name;
                bossList.Add(boss);
                if (currentBoss == null)
                {
                    currentBoss = boss;
                }
            }
        }
    }

    //khởi tạo nhân vật
    private void InstantiatePlayer(GameObject _character)
    {
        player = Instantiate(_character, new Vector3(-12f, -1f, 0f), Quaternion.identity);
        player.GetComponent<PlayerCombat>().enabled = true;
        player.name = _character.name;
    }

    //bắt đầu bấm giờ
    private void StartCountdownTimer()
    {
        if(!CameraController.instance.introduceBoss_ && !TimerInBattle.instance.timerIsRunning_ && !resultBattle)
        {
            //Debug.Log("StartCountdownTimer");
            TimerInBattle.instance.TimerSwitch(true);
            UIController.instance.SetInforBoss(currentBoss);
            currentBoss.GetComponent<BossController>().StartMechanics();
        }
    }

    private void InstantiateRewardBox(GameObject _rewardBox){
        Vector3 position = new Vector3(bossList[bossList.Count - 1].transform.position.x, bossList[bossList.Count - 1].transform.position.y + 3f, bossList[bossList.Count - 1].transform.position.z);
        Instantiate(_rewardBox, position, Quaternion.identity);
    }

    //kiểm tra kết quả trận đấu
    private void CheckBattle()
    {
        if (!resultBattle)
        {
            if (player.GetComponent<PlayerCombat>().currentHealth_ <= 0 || TimerInBattle.instance.timeOut_)
            {
                TimerInBattle.instance.TimerSwitch(false);
                currentBoss.GetComponent<BossController>().StopMechanics();
                foreach(Transform obj in GameObject.Find("MobPool").transform)
                {
                    //Debug.Log(obj.name);
                    obj.GetComponent<MobController>().ResetIdle();
                }
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<PlayerCombat>().enabled = false;
                UIController.instance.OnAlertLose();
                AudioManager.instance.StopBackgroundMusic();
                AudioManager.instance.StopBattleSuccessSFX();
                resultBattle = true;
            }
            if (currentBoss.GetComponent<BossController>().isDead_ && TimerInBattle.instance.timeRemaining_ > 0)
            {
                if (currentBoss == bossList[bossList.Count - 1])
                {
                    TimerInBattle.instance.TimerSwitch(false);
                    currentBoss.GetComponent<BossController>().StopMechanics();
                    UIController.instance.OnAlertWin();
                    GameObject.Find("BulletPool").SetActive(false);
                    foreach (Transform obj in GameObject.Find("MobPool").transform)
                    {
                        obj.GetComponent<MobController>().Die();
                    }
                    InstantiateRewardBox(rewardBox);
                    AudioManager.instance.StopBackgroundMusic();
                    AudioManager.instance.StopBattleSuccessSFX();
                    resultBattle = true;
                }
                else
                {
                    if(teleport != null)
                    {
                        teleport.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public void ActiveNextBoss()
    {
        //Debug.Log("currentBoss = bossList[indexNextBoss];");
        int indexNextBoss = bossList.FindIndex(x => x == currentBoss) + 1;
        currentBoss = bossList[indexNextBoss];
        UIController.instance.SetInforBoss(currentBoss);
        currentBoss.GetComponent<BossController>().StartMechanics();
    }
}
