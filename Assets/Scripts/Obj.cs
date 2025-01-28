using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj
{
    public bool isUnique;

    public const int code_MAINDOME = 0;
    public const int code_OXYGENGENERATOR = 1;
    public const int code_ELEMENTSYNTHESIZER = 2;
    public const int code_ENERGYPANEL = 3;
    public const int code_ROVERS = 4;
    public const int code_ROVERM = 5;
    public const int code_ROVERL = 6;
    public const int code_BUILDERROBOT = 100;
    public const int code_CRAFTERROBOT = 101;
    public const int code_EXPLORERROBOT = 102;
    public const int code_MINERROBOT = 103;
    public const int code_OXYGENCAPSULE = 1000;
    public const int code_MINERALMINER = 1001;
    public const int code_DUSTBOMBLAUNCHER = 1002;
    public const int code_ATMOSPHERICTRANSDUCER = 1003;
    public const int code_SUPERMICROBIALSPREADER = 1004;
    public const int code_COPPER = 10000;
    public const int code_IRON = 10001;
    public const int code_SILLICA = 10002;
    public const int code_CARBON = 10003;
    public const int code_NEODYMIUM = 10004;
    public const int code_MACHINEDCOPPER = 10005;
    public const int code_MACHINEDIRON = 10006;
    public const int code_MACHINEDSILLICA = 10007;
    public const int code_MACHINEDNEODYMIUM = 10008;
    public const int code_ADVANCEDALLOY = 10009;
    public const int code_MICROCHIP = 10010;
    public const int code_ENERGY = 10011;
    public const int code_OXYGEN = 10012;
    public const int code_MICROBIALCYLINDER = 10013;

    // Object의 인덱스
    public int Idx { get; set; }
    
    // Object의 개수
    public int Num { get; set; }

    // Object의 제작 시간
    public float BuildTime { get; set; }

    // Object의 이름
    public string Name { get; set; }

    // Object의 설명
    public string Description { get; set; }
    
    // Object의 이미지
    public Sprite Image { get; set; }

    // 이 Object 제작 시 필요한 모든 오브젝트의의 리스트
    public List<Obj> requiredObjs = new List<Obj>();

    public Obj(int idx, int num)
    {
        SetObj(idx, num);
    }

    public int GetObjIdx()
    {
        return Idx;
    }

    public int GetobjNum()
    {
        return Num;
    }

    public float GetBuildTime()
    {
        return BuildTime;
    }
    
    public string GetobjName()
    {
        return Name;
    }
    
    public string GetobjDescription()
    {
        return Description;
    }
    
    public Sprite GetobjImg()
    {
        return Image;
    }

    public void SetObj(int idx, int num)
    {
        Idx = idx;
        Num = num;
        requiredObjs.Clear();

        // case 1: Resources
        if ((Idx / 10000) == 1)
        {
            if (Idx == code_MICROBIALCYLINDER)
                isUnique = true;
            else
                isUnique = false;
        }
        // case 2: Tools
        else if ((Idx / 1000) == 1)
        {
            if (Idx == code_OXYGENCAPSULE)
                isUnique = false;
            else
                isUnique = true;
        }
        // case 3: Robots
        else if ((Idx / 100) == 1)
        {
            isUnique = true;
        }
        // case 4: Buildings
        else
        {
            if (Idx == code_ENERGYPANEL)
                isUnique = false;
            else
                isUnique = true;
        }

        switch (Idx)
        {
            case code_MAINDOME:
                SetObjTo_MainDome();
                break;
            case code_OXYGENGENERATOR:
                SetObjTo_OxygenGenerator();
                break;
            case code_ELEMENTSYNTHESIZER:
                SetObjTo_ElementSynthesizer();
                break;
            case code_ENERGYPANEL:
                SetObjTo_EnergyPanel();
                break;
            case code_ROVERS:
                SetObjTo_RoverS();
                break;
            case code_ROVERM:
                SetObjTo_RoverM();
                break;
            case code_ROVERL:
                SetObjTo_RoverL();
                break;
            case code_BUILDERROBOT:
                SetObjTo_BuilderRobot();
                break;
            case code_CRAFTERROBOT:
                SetObjTo_CrafterRobot();
                break;
            case code_EXPLORERROBOT:
                SetObjTo_ExplorerRobot();
                break;
            case code_MINERROBOT:
                SetObjTo_MinerRobot();
                break;
            case code_OXYGENCAPSULE:
                SetObjTo_OxygenCapsule();
                break;
            case code_MINERALMINER:
                SetObjTo_MineralMiner();
                break;
            case code_DUSTBOMBLAUNCHER:
                SetObjTo_DustBombLauncher();
                break;
            case code_ATMOSPHERICTRANSDUCER:
                SetObjTo_AtmosphericTransducer();
                break;
            case code_SUPERMICROBIALSPREADER:
                SetObjTo_SuperMicrobialSpreader();
                break;
            case code_COPPER:
                SetObjTo_Copper();
                break;
            case code_IRON:
                SetObjTo_Iron();
                break;
            case code_SILLICA:
                SetObjTo_Sillica();
                break;
            case code_CARBON:
                SetObjTo_Carbon();
                break;
            case code_NEODYMIUM:
                SetObjTo_Neodymium();
                break;
            case code_MACHINEDCOPPER:
                SetObjTo_MachinedCopper();
                break;
            case code_MACHINEDIRON:
                SetObjTo_MachinedIron();
                break;
            case code_MACHINEDSILLICA:
                SetObjTo_MachinedSillica();
                break;
            case code_MACHINEDNEODYMIUM:
                SetObjTo_MachinedNeodymium();
                break;
            case code_ADVANCEDALLOY:
                SetObjTo_AdvancedAlloy();
                break;
            case code_MICROCHIP:
                SetObjTo_MicroChip();
                break;
            case code_MICROBIALCYLINDER:
                SetObjTo_MicrobialCylinder();
                break;
            default:
                Debug.LogWarning("존재하지 않는 Obj입니다.");
                SetObjDefault();
                break;
        }
    }

    private void SetObjDefault()
    {
        Idx = -1;
        BuildTime = 0f;
        Name = " - ";
        Description = " - ";
        Image = null;
    }

    private void SetObjTo_MainDome()
    {
        Idx = code_MAINDOME;
        BuildTime = 30f;
        Name = "Main Dome";
        Description = "대용량의 산소 저장이 가능하고, 간단한 생활이 가능하여 기지로 사용할 수 있는 건축물이다";
        Image = ObjectManager.Instance._sprite_mainDome;
    }

    private void SetObjTo_OxygenGenerator()
    {
        Idx = code_OXYGENGENERATOR;
        BuildTime = 20f;
        Name = "Oxygen Generator";
        Description = "생존에 필요한 산소를 생성할 수 있는 장치이다.";
        Image = ObjectManager.Instance._sprite_oxygenGenerator;
        requiredObjs.Add(new Obj(code_MACHINEDCOPPER, 50));
        requiredObjs.Add(new Obj(code_MACHINEDIRON, 50));
        requiredObjs.Add(new Obj(code_MACHINEDSILLICA, 10));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 10));
    }

    private void SetObjTo_ElementSynthesizer()
    {
        Idx = code_ELEMENTSYNTHESIZER;
        BuildTime = 10f;
        Name = "Element Synthesizer";
        Description = "자원을 분해하고, 합성하여 필요한 원소를 만들어주는 혁신적인 장치이다.";
        Image = ObjectManager.Instance._sprite_elementSynthesizer;
        requiredObjs.Add(new Obj(code_MACHINEDCOPPER, 50));
        requiredObjs.Add(new Obj(code_MACHINEDIRON, 50));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 20));
    }

    private void SetObjTo_EnergyPanel()
    {
        Idx = code_ENERGYPANEL;
        BuildTime = 5f;
        Name = "Energy Panel";
        Description = "항성에서 발생한 대용량의 에너지를 수신할 수 있는 장치이다.";
        Image = ObjectManager.Instance._sprite_energyPanel;
        requiredObjs.Add(new Obj(code_MACHINEDIRON, 10));
        requiredObjs.Add(new Obj(code_MACHINEDSILLICA, 10));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 5));
    }

    private void SetObjTo_RoverS()
    {
        Idx = code_ROVERS;
        BuildTime = 15f;
        Name = "Light Rover";
        Description = "가까운 곳을 탐사하기 좋은 기본 로버이다.";
        Image = ObjectManager.Instance._sprite_roverS;
        requiredObjs.Add(new Obj(code_MACHINEDCOPPER, 10));
        requiredObjs.Add(new Obj(code_MACHINEDIRON, 20));
        requiredObjs.Add(new Obj(code_MACHINEDSILLICA, 10));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 5));
    }

    private void SetObjTo_RoverM()
    {
        Idx = code_ROVERM;
        BuildTime = 20f;
        Name = "Medium Rover";
        Description = "더 빠르고, 더 많은 양의 산소를 저장할 수 있게 되었다. 조금 더 먼 지역까지 쉽게 이동할 수 있다.";
        Image = ObjectManager.Instance._sprite_roverM;
        requiredObjs.Add(new Obj(code_MACHINEDCOPPER, 20));
        requiredObjs.Add(new Obj(code_MACHINEDIRON, 40));
        requiredObjs.Add(new Obj(code_MACHINEDSILLICA, 20));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 10));
    }

    private void SetObjTo_RoverL()
    {
        Idx = code_ROVERL;
        BuildTime = 30f;
        Name = "Large Rover";
        Description = "대용량의 산소 저장이 가능하고, 간단한 생활이 가능하여 기지로 사용할 수 있는 건축물이다";
        Image = ObjectManager.Instance._sprite_roverL;
        requiredObjs.Add(new Obj(code_MACHINEDCOPPER, 40));
        requiredObjs.Add(new Obj(code_ADVANCEDALLOY, 50));
        requiredObjs.Add(new Obj(code_MACHINEDSILLICA, 40));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 20));
    }

    private void SetObjTo_BuilderRobot()
    {
        Idx = code_BUILDERROBOT;
        BuildTime = 10f;
        Name = "Builder Robot";
        Description = "건축물들의 건설을 도와주는 로봇이다.";
        Image = ObjectManager.Instance._sprite_builderRobot;
        requiredObjs.Add(new Obj(code_ENERGY, 100));
    }

    private void SetObjTo_CrafterRobot()
    {
        Idx = code_CRAFTERROBOT;
        BuildTime = 10f;
        Name = "Crafter Robot";
        Description = "제작을 도와주는 로봇이다.";
        Image = ObjectManager.Instance._sprite_crafterRobot;
        requiredObjs.Add(new Obj(code_ENERGY, 100));
    }

    private void SetObjTo_ExplorerRobot()
    {
        Idx = code_EXPLORERROBOT;
        BuildTime = 10f;
        Name = "Explorer Robot";
        Description = "탐사를 도와주는 로봇이다.";
        Image = ObjectManager.Instance._sprite_explorerRobot;
        requiredObjs.Add(new Obj(code_ENERGY, 100));
    }

    private void SetObjTo_MinerRobot()
    {
        Idx = code_MINERROBOT;
        BuildTime = 10f;
        Name = "Miner Robot";
        Description = "채굴을 도와주는 로봇이다.";
        Image = ObjectManager.Instance._sprite_minerRobot;
        requiredObjs.Add(new Obj(code_ENERGY, 100));
    }

    private void SetObjTo_OxygenCapsule()
    {
        Idx = code_OXYGENCAPSULE;
        BuildTime = 3f;
        Name = "Oxygen Capsule";
        Description = "생존에 필요한 산소를 충전해주는 캡슐이다.";
        Image = ObjectManager.Instance._sprite_oxygenCapsule;
        requiredObjs.Add(new Obj(code_OXYGEN, 1000));
        requiredObjs.Add(new Obj(code_MACHINEDCOPPER, 1));
    }

    private void SetObjTo_MineralMiner()
    {
        Idx = code_MINERALMINER;
        BuildTime = 0f;
        Name = "Mineral Miner";
        Description = "광물을 채굴하기 위한 편리한 장비이다.";
        Image = ObjectManager.Instance._sprite_mineralMiner;
    }

    private void SetObjTo_DustBombLauncher()
    {
        Idx = code_DUSTBOMBLAUNCHER;
        BuildTime = 10f;
        Name = "Dust Bomb Launcher";
        Description = "먼지 폭탄을 발사하기 위한 장비이다. 먼지 폭탄의 미세한 입자는 대기중에 순식간에 퍼진다.";
        Image = ObjectManager.Instance._sprite_dustBombLauncher;
        requiredObjs.Add(new Obj(code_MACHINEDIRON, 10));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 10));
    }

    private void SetObjTo_AtmosphericTransducer()
    {
        Idx = code_ATMOSPHERICTRANSDUCER;
        BuildTime = 25f;
        Name = "Atmospheric Transducer";
        Description = "대기 조성을 바꿀 수 있는 장치이다. 무거운만큼 성능은 확실해 보인다.";
        Image = ObjectManager.Instance._sprite_atmosphericTransducer;
        requiredObjs.Add(new Obj(code_MACHINEDSILLICA, 20));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 5));
        requiredObjs.Add(new Obj(code_ADVANCEDALLOY, 20));
    }

    private void SetObjTo_SuperMicrobialSpreader()
    {
        Idx = code_SUPERMICROBIALSPREADER;
        BuildTime = 15f;
        Name = "Super-Microbial Spreader";
        Description = "슈퍼 미생물을 살포할 수 있는 장치. 적당한 지역에 살포해야 한다.";
        Image = ObjectManager.Instance._sprite_superMicrobialSpreader;
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 10));
        requiredObjs.Add(new Obj(code_ADVANCEDALLOY, 10));
        requiredObjs.Add(new Obj(code_MICROBIALCYLINDER, 1));
    }

    private void SetObjTo_Copper()
    {
        Idx = code_COPPER;
        Name = "Copper";
        Description = "흔하게 발견되는 금속 광물의 원석";
        Image = ObjectManager.Instance._sprite_copper;
    }

    private void SetObjTo_Iron()
    {
        Idx = code_IRON;
        Name = "Iron";
        Description = "종종 발견되는 금속 광물의 원석";
        Image = ObjectManager.Instance._sprite_iron;
    }

    private void SetObjTo_Sillica()
    {
        Idx = code_SILLICA;
        Name = "Sillica";
        Description = "주로 모래나 암석 형태로 발견되는 광물의 원석";
        Image = ObjectManager.Instance._sprite_sillica;
    }

    private void SetObjTo_Carbon()
    {
        Idx = code_CARBON;
        Name = "Carbon";
        Description = "산소 발생기에서 채집 가능한 자원이다";
        Image = ObjectManager.Instance._sprite_carbon;
    }

    private void SetObjTo_Neodymium()
    {
        Idx = code_NEODYMIUM;
        Name = "Neodymium";
        Description = "희귀하게 발견되는 금속 광물의 원석. 자기장을 형성하고 있는 듯하다";
        Image = ObjectManager.Instance._sprite_neodymium;
    }

    private void SetObjTo_MachinedCopper()
    {
        Idx = code_MACHINEDCOPPER;
        Name = "Machined Copper";
        Description = "구리 원석을 가공하여 만들었다";
        Image = ObjectManager.Instance._sprite_machinedCopper;
    }

    private void SetObjTo_MachinedIron()
    {
        Idx = code_MACHINEDIRON;
        Name = "Machined Iron";
        Description = "철 원석을 가공하여 만들었다";
        Image = ObjectManager.Instance._sprite_machinedIron;
    }

    private void SetObjTo_MachinedSillica()
    {
        Idx = code_MACHINEDSILLICA;
        Name = "Machined Sillica";
        Description = "실리카 원석을 가공하여 만들었다";
        Image = ObjectManager.Instance._sprite_machinedSillica;
    }

    private void SetObjTo_MachinedNeodymium()
    {
        Idx = code_MACHINEDNEODYMIUM;
        Name = "Machined Neodymium";
        Description = "네오디뮴 원석을 가공하여 만들었다.";
        Image = ObjectManager.Instance._sprite_machinedNeodymium;
    }

    private void SetObjTo_AdvancedAlloy()
    {
        Idx = code_ADVANCEDALLOY;
        Name = "Advanced Alloy";
        Description = "탄소와 철 원석을 가공하여 만든 강력한 합금이다";
        Image = ObjectManager.Instance._sprite_advancedAlloy;
        requiredObjs.Add(new Obj(code_IRON, 1));
        requiredObjs.Add(new Obj(code_CARBON, 1));
    }

    private void SetObjTo_MicroChip()
    {
        Idx = code_MICROCHIP;
        Name = "Micro Chip";
        Description = "흔하게 발견되는 금속 광물의 원석이다.";
        Image = ObjectManager.Instance._sprite_microChip;
        requiredObjs.Add(new Obj(code_MACHINEDIRON, 1));
        requiredObjs.Add(new Obj(code_MACHINEDSILLICA, 1));
        requiredObjs.Add(new Obj(code_MACHINEDNEODYMIUM, 1));
    }

    private void SetObjTo_MicrobialCylinder()
    {
        Idx = code_MICROBIALCYLINDER;
        Name = "Microbial Cylinder";
        Description = "어떤 환경에서도 견뎌 생존하고 진화할 수 있는 생명체이다.";
    }
}
