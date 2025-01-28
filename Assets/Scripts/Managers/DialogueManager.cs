using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Dialogue
{
    public string name;
    public string script;

    public Dialogue(string script)
    {
        this.name = "Unkown";
        this.script = script;
    }

    public Dialogue(string name, string script)
    {
        this.name = name;
        this.script = script;
    }
}

public class DialogueManager : MonoBehaviour
{
    public const int characterIndex_max = 100, contextIndex_max = 1000;
    public int characterIndex, contextIndex;
    public int dialogueIndex;

    private Queue<Dialogue> _dilaogueQueue; // 순차적으로 대화를 출력하기 전, 대화 사전으로부터 Dialogue[]을 받아 와 임시로 저장할 자료구조

    /*
        대화 사전 인덱스 구성
        0 ~ 100: 시스템 메세지(빈번하게 사용될 안내 메세지)    
        1000 ~ 2000: 인게임 퀘스트 메세지, 모든 퀘스트 인덱스는 게임 진행 순서대로 나열된다.
    */
    private Dictionary<int, List<Dialogue>> _dict_Dialogue;
    private List<Dialogue> _dialogueList; // 대화 사전에 Dialogue들을 추가하기 위해 사용할 List

    // Singletone instance
    public static DialogueManager Instance { get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _dict_Dialogue = new Dictionary<int, List<Dialogue>>();
        _dialogueList = new List<Dialogue>();
    }

    private void Start()
    {
        InitializeDict();
    }

    /// <summary>
    /// 대화 사전 키를 받아서, 키가 존재할 경우 해당하는 Dilaogue 구조체의 리스트를 반환하는 함수
    /// </summary>
    /// <param name="context"> 대화 사전에 접근할 키 </param>
    /// <returns></returns>
    public List<Dialogue> LoadContext(int context)
    {
        if (_dict_Dialogue.TryGetValue(context, out List<Dialogue> tempDArray))
        {
            Debug.Log($"Dialogue Manager: 대화 사전으로부터 {context}번째 context를 반환합니다");
            return tempDArray;
        }
        else
        {
            Debug.LogWarning("대화 사전에 존재하지 않는 context입니다.");
            return null;
        }
    }

    private void InitializeDict()
    {
        // 1000
        _dialogueList.Add(new Dialogue("Hailey", ". . . 님"));
        _dialogueList.Add(new Dialogue("Hailey", ". . . . ."));
        _dialogueList.Add(new Dialogue("Hailey", ". . . Avon님?"));
        _dialogueList.Add(new Dialogue("Avon", "으윽 . . . Hailey, 아직 멀었어?"));
        _dialogueList.Add(new Dialogue("Hailey", "안 그래도 그걸 알려드릴 참이었는데 . . . \n한참을 불러도 대답이 없으셨어요"));
        _dialogueList.Add(new Dialogue("Hailey", "임무 시작도 전에 Avon님이 까딱 돌아가신 줄로만 알았다니까요"));
        _dialogueList.Add(new Dialogue("Avon", "아 . . . 미안 Hailey.\n그렇지만 이젠 이 따분한 여행도 슬슬 지쳐 간단 말이지"));
        _dialogueList.Add(new Dialogue("Hailey", "그럼 따분한 Avon님께는 좋은 소식이겠네요"));
        _dialogueList.Add(new Dialogue("Hailey", "현재 위치에서 도달 가능한 항성계의 스캔과 임무 수행 적합도 평가가 모두 완료되었습니다"));
        _dialogueList.Add(new Dialogue("Hailey", "후보 행성은 총 3개이며, 모두 암석 행성입니다"));
        _dialogueList.Add(new Dialogue("Hailey", "각 행성의 특징과, 그에 따른 해당 행성에서의 임무 성공률을 계산해드렸어요"));
        _dialogueList.Add(new Dialogue("Hailey", "Avon님의 첫 임무인 만큼 나이도 적당하고, 규모가 가장 작은 2번째 행성을 추천드립니다"));
        _dialogueList.Add(new Dialogue("Avon", "음 . . . 첫번째 행성은 행성의 표면 대부분이 물로 덮여있고, 두번째 행성은 표면 온도가 상당하고, 세번째 행성은 날씨의 변화가 극단적이라 . . ."));
        _dialogueList.Add(new Dialogue("Avon", "잠깐 근데, 임무 성공률 97.7%?"));
        _dialogueList.Add(new Dialogue("Avon", "생각해본 적이 없어서 그런건데, 혹시 임무에 실패하면 나는 어떻게 되는거지?"));
        _dialogueList.Add(new Dialogue("Hailey", "뭐 별 일은 없을 거에요. 우주 청소부 Avon의 명성이 떨어질 뿐이죠"));
        _dialogueList.Add(new Dialogue("Hailey", ". . . 물론 최악의 경우엔 불의의 사고로 Avon님이 목숨을 잃을 수도 있고요"));
        _dialogueList.Add(new Dialogue("Avon", "그건 좀 . . . 무서운데"));
        _dialogueList.Add(new Dialogue("Hailey", "걱정마세요, Avon. 그 최악의 상황을 막기 위해 제가 있는 거기도 하니까요"));
        _dialogueList.Add(new Dialogue("Avon", "그래, 시작부터 주눅들 필요는 없겠지!"));
        _dialogueList.Add(new Dialogue("Hailey", "그래서 어느 행성으로 하시겠어요?"));
        _dialogueList.Add(new Dialogue("Avon", "더운 건 딱 질색이라 내키진 않지만, 아무래도 Hailey의 말을 듣는 게 좋겠어"));
        _dialogueList.Add(new Dialogue("Avon", "뭐, 꾸리꾸리한 색깔도 마음에 들고, 여기서 2Jump면 도달 가능할 정도로 가까우니까"));
        _dialogueList.Add(new Dialogue("Hailey", "알겠습니다, 그럼 Nova Odyssey의 Jump 목표 지점을 두번째 후보 행성으로 설정할까요?"));
        _dialogueList.Add(new Dialogue("Avon", "잠깐, 출발하기 전에 저 꾸리꾸리한 녀석의 이름을 지어줘야겠어"));
        _dialogueList.Add(new Dialogue("Hailey", "이름이라 . . . 별로 중요한 일 같진 않지만, Avon님이 원하신다면야 그러시죠"));
        _dialogueList.Add(new Dialogue("Avon", "음 . . ."));
        _dialogueList.Add(new Dialogue("Avon", "보는 것만으로도 푹푹 찌는 듯 해 보이는 저 녀석의 이름은... \n이제부터 Muggy다!"));
        _dialogueList.Add(new Dialogue("Hailey", "훌륭한 이름이네요, Avon님. \n그럼 Muggy로의 Jump를 시작하겠습니다"));
        _dict_Dialogue.Add(1000, new List<Dialogue>(_dialogueList));
        Debug.Log("context를 대화 사전에 저장합니다 (key: 1000)");
        _dialogueList.Clear();

        // 1001
        _dialogueList.Add(new Dialogue("Hailey", "Avon, Grey Point에 무사히 도착했습니다"));
        _dialogueList.Add(new Dialogue("Hailey", "이번 첫 임무를 무사히 완수하기 위해 제가 필요한 모든 지원을 해드릴 거예요"));
        _dialogueList.Add(new Dialogue("Hailey", "우선 현재 Nova Odyssey의 위치가 당신의 거점 위치라는 걸 기억해두세요."));
        _dialogueList.Add(new Dialogue("Hailey", "우선 거점에 필수 시설들의 건설을 먼저 완료한 뒤, 이 행성의 탐사를 시작해야 해요"));
        _dialogueList.Add(new Dialogue("Hailey", "Avon, Grey Point에 도착했습니다"));
        _dict_Dialogue.Add(1001, new List<Dialogue>(_dialogueList));
        Debug.Log("context를 대화 사전에 저장합니다 (key: 1001)");
        _dialogueList.Clear();

    }

    public void printDialogue(int context)
    {
        if (_dict_Dialogue.TryGetValue(context, out List<Dialogue> tempDialogueList))
        {
            for (int i = 0; i < tempDialogueList.Count; i++)
                Debug.Log($"{i}: {tempDialogueList[i].script}");
        }
    }
}
