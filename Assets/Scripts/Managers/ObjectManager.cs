using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{   
    private int curEnergyPanelNum = 0;

    // 프리팹을 생성할 트랜스폼 정보
    public Transform objTransform;

    // 현재 게임 내에서 선택된 오브젝트
    public static Obj selectedObj;

    // 건축(제작) 대기 시간 코루틴 캐싱
    private WaitForSeconds WaitForBuildTime;

    /// <summary>
    /// 고유하지 않은 모든 Item의 개수를 저장하는 자료구조
    /// 고유한 오브젝트의 개수는 어차피 1로 고정이므로, 따로 Inventory에서 다루지 않는다.
    /// 오브젝트의 인덱스를 key로 사용하여 해당하는 오브젝트의 인벤토리 내 개수를 값으로 받는다.
    /// </summary>
    private Dictionary<int, int> inventory = new Dictionary<int, int>();

    [Header("Object Images")]
    public Sprite _sprite_mainDome;
    public Sprite _sprite_oxygenGenerator;
    public Sprite _sprite_energyPanel;
    public Sprite _sprite_elementSynthesizer;
    public Sprite _sprite_roverS;
    public Sprite _sprite_roverM;
    public Sprite _sprite_roverL;

    [Space(2)]
    public Sprite _sprite_builderRobot;
    public Sprite _sprite_crafterRobot;
    public Sprite _sprite_explorerRobot;
    public Sprite _sprite_minerRobot;
    
    [Space(2)]
    public Sprite _sprite_oxygenCapsule;
    public Sprite _sprite_mineralMiner;
    public Sprite _sprite_dustBombLauncher;
    public Sprite _sprite_atmosphericTransducer;
    public Sprite _sprite_superMicrobialSpreader;

    [Space(2)]
    public Sprite _sprite_copper;
    public Sprite _sprite_iron;
    public Sprite _sprite_sillica;
    public Sprite _sprite_carbon;
    public Sprite _sprite_neodymium;
    public Sprite _sprite_machinedCopper;
    public Sprite _sprite_machinedIron;
    public Sprite _sprite_machinedSillica;
    public Sprite _sprite_machinedNeodymium;
    public Sprite _sprite_advancedAlloy;
    public Sprite _sprite_microChip;

    [Header("Position Presets")]
    public Transform mainDomePos;
    public Transform oxygenGeneratorPos;
    public Transform[] energyPanelPos;
    public Transform roverSPos;
    public Transform roverMPos;
    public Transform roverLPos;
    public Transform robotSpawnPoint;

    [Header("Building Prefabs")]
    public GameObject[] buildings;

    [Header("Robot Prefabs")]
    public GameObject[] robots;

    [Header("Tool Prefabs")]
    public GameObject[] tools;
    
    public QuestManager _questManager;

    // Singletone instance
    public static ObjectManager Instance { get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        selectedObj = new Obj(-1, 0);
        _InitInventory();
    }

// #region Resources
//     private void GetResources()
//     {
//         _sprite_mainDome = Resources.Load<Sprite>("Sprites/profile/craft/building_mainDome");
//         _sprite_oxygenGenerator = Resources.Load<Sprite>("Sprites/profile/craft/building_oxygenGenerator");
//         _sprite_energyPanel = Resources.Load<Sprite>("Sprites/profile/craft/building_energyPanel");
//         // _sprite_elementSynthesizer = Resources.Load<Sprite>("Sprites/profile/craft/");
//         _sprite_roverS = Resources.Load<Sprite>("Sprites/profile/craft/rover_S");
//         _sprite_roverM = Resources.Load<Sprite>("Sprites/profile/craft/rover_M");
//         _sprite_roverL = Resources.Load<Sprite>("Sprites/profile/craft/rover_L");
//         _sprite_builderRobot = Resources.Load<Sprite>("Sprites/profile/craft/robot_Builder");
//         _sprite_crafterRobot = Resources.Load<Sprite>("Sprites/profile/craft/robot_Crafter");
//         _sprite_explorerRobot = Resources.Load<Sprite>("Sprites/profile/craft/robot_Explorer");
//         _sprite_minerRobot = Resources.Load<Sprite>("Sprites/profile/craft/robot_Miner");
//         // _sprite_oxygenCapsule = Resources.Load<Sprite>("Sprites/profile/craft/");
//         // _sprite_mineralMiner = Resources.Load<Sprite>("Sprites/profile/craft/");
//         // _sprite_dustBombLauncher = Resources.Load<Sprite>("Sprites/profile/craft/");
//         // _sprite_atmosphericTransducer = Resources.Load<Sprite>("Sprites/profile/craft/");
//         // _sprite_superMicrobialSpreader = Resources.Load<Sprite>("Sprites/profile/craft/");
//     }

// #endregion

#region Initialize
    // Player Inventory 초기화 메소드
    private void _InitInventory()
    {
        inventory.Add(Obj.code_ENERGYPANEL, 0);
        inventory.Add(Obj.code_OXYGENCAPSULE, 10);
        inventory.Add(Obj.code_COPPER, 0);
        inventory.Add(Obj.code_IRON, 0);
        inventory.Add(Obj.code_SILLICA, 0);
        inventory.Add(Obj.code_CARBON, 0);
        inventory.Add(Obj.code_NEODYMIUM, 0);
        inventory.Add(Obj.code_MACHINEDCOPPER, 0);
        inventory.Add(Obj.code_MACHINEDIRON, 0);
        inventory.Add(Obj.code_MACHINEDSILLICA, 0);
        inventory.Add(Obj.code_MACHINEDNEODYMIUM, 0);
        inventory.Add(Obj.code_ADVANCEDALLOY, 0);
        inventory.Add(Obj.code_MICROCHIP, 0);
    }
#endregion

#region Build
    private GameObject IdxToPrefab(int idx)
    {
        SetObjTransform(idx);

        // index에 해당하는 프리팹 반환
        switch (idx)
        {
            case 0:
                return buildings[0];
            case 1:
                return buildings[1];
            case 2:
                curEnergyPanelNum++;
                return buildings[2];
            case 3:
                return buildings[3];
            case 4:
                return buildings[4];
            case 5:
                return buildings[5];
            case 6:
                return buildings[6];
            case 100:
                return robots[0];
            case 101:
                return robots[1];
            case 102:
                objTransform.position += new Vector3(0f, 1f, 0f); // 탐사 로봇의 포지션은 +1 해줘야 함
                return robots[2];
            case 103:
                return robots[3];
            case 1001:
                return tools[0];
            case 1002:
                return tools[1];
            case 1003:
                return tools[2];
            case 1004:
                return tools[3];
            default:
                Debug.LogWarning("존재하지 않는 Object Index입니다.");
                return null;
        }
    }

    /// <summary>
    /// 생성할 오브젝트 인덱스에 따라 트랜스폼을 설정해준다.
    /// </summary>
    /// <param name="idx"></param>
    public void SetObjTransform(int idx)
    {
        // index 범위에 따라 프리팹의 Transform 조정
        if (idx <= 10)
        {
            switch (idx)
            {
                case 0:
                    objTransform = mainDomePos;
                    break;
                case 1:
                    objTransform = oxygenGeneratorPos;
                    break;
                case 2:
                    objTransform = energyPanelPos[curEnergyPanelNum];
                    break;
                case 3:
                    break;
                case 4:
                    objTransform = roverSPos;
                    break;
                case 5:
                    objTransform = roverMPos;
                    break;
                case 6:
                    objTransform = roverLPos;
                    break;
            }
        }
        else if (idx >= 100 && idx <= 103) // Robot
            objTransform = robotSpawnPoint;
    }

    // Craft 메뉴에서 "제작" 버튼 클릭 시 호출된다
    public void BuildStart()
    {
        GameObject buildObj = IdxToPrefab(selectedObj.Idx);
        WaitForBuildTime = new WaitForSeconds(selectedObj.BuildTime);

        StartCoroutine(BuildStartCoroutine(buildObj));
    }

    public IEnumerator BuildStartCoroutine(GameObject buildObj)
    {
        Instantiate(buildObj, objTransform.position, objTransform.rotation);
        yield return WaitForBuildTime;
        Debug.Log($"Building Start: {buildObj}");

        if (_questManager == null)
            _questManager = FindObjectOfType<QuestManager>();
        if (_questManager != null)
            _questManager.UpdateBuildObjective(selectedObj.Idx);
    }

    private void BuildFinish()
    {
        
    }
#endregion

#region Inventory
    private int GetItemNum(int idx)
    {
        if (inventory.TryGetValue(idx, out int itemNum))
            return itemNum;
        else
        {
            Debug.LogWarning("존재하지 않는 Item입니다.");
            return -1;
        }

    }
#endregion

    /// <summary>
    /// 선택된 오브젝트의 index에 해당하는 정보를 세팅한다
    /// </summary>
    /// <param name="idx"> Index Range: 0-1004</param>
    public void SetObjInfo(int idx, int num)
    {
        
        selectedObj.SetObj(idx, num);
    }
}