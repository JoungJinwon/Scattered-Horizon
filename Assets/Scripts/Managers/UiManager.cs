using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
#region Field
    public bool isFading = false; // 페이드 진행 중인지의 여부
    public bool isInDialogue; // 현재 대화 중인지의 여부
    private bool assignCompleted; // 씬의 로드 직후 UI 요소들의 할당 완료 여부

    private WaitUntil waitForEnter; // enter 입력 대기 코루틴
    public WaitUntil waitForAssignCompleted; // 씬 UI 요소 할당 대기 코루틴
    private WaitUntil waitForGmAssignCompleted; // 게임 매니저 씬 로드 시 요소 할당 대기 코루틴

    private TypeEffect typer;
    private PlayerStats _player;
    private InputManager _input;

    [Header("Canvas")]
    [SerializeField] [Tooltip("Canvas to manage all static UI elements")]
    private GameObject canvasStatic;
    [SerializeField] [Tooltip("Canvas to manage all dynamic UI elements")]
    private GameObject canvasDynamic;
    [SerializeField] [Tooltip("Canvas which is used for all other UI elements")]
    private GameObject canvasGeneral;
    [Space(5)]

    [Header("System Panels")]
    [SerializeField]
    private GameObject pausePanel; // 일시정지 패널
    [SerializeField]
    private Button mainMenuBtn; // 일시정지_메인메뉴 버튼
    [SerializeField]
    private Button settingsBtn; // 일시정지_설정정 버튼
    [SerializeField]
    private Button saveAndQuitBtn; // 일시정지_저장 후 종료 버튼
    [SerializeField]
    private GameObject gameoverPanel; // 게임오버 패널
    [SerializeField]
    private GameObject systemMessagePanel; // 시스템 패널
    [SerializeField]
    private TextMeshProUGUI systemMessage; // 시스템 메세지 텍스트트
    [SerializeField]
    private GameObject novaPanels; // 노바 접근 시 활성화되는 패널

    [Header("Control Panel")]
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject divider;
    [SerializeField]
    private GameObject detailPanel;

    [Space(5)]
    [SerializeField]
    private Button charBtn;
    [SerializeField]
    private Button mapBtn;
    [SerializeField]
    private Button resourceBtn;
    [SerializeField]
    private Button questBtn;
    [SerializeField]
    private Button craftBtn;

    [Space(5)]
    [SerializeField]
    private GameObject charTap;
    [SerializeField]
    private GameObject mapTap;
    [SerializeField]
    private GameObject resourceTap;
    [SerializeField]
    private GameObject questTap;
    [SerializeField]
    private GameObject craftTap;

    [Space(5)]
    [SerializeField]
    private TextMeshProUGUI headerTxt;
    private Image itemImage;
    private TextMeshProUGUI descriptionTxt;
    private TextMeshProUGUI requiredTxt;

    [Header("HUD elements")]
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI dateText;
    [SerializeField]
    private Image oxygenImage;
    
    [Header("Dialogue Box elements")]
    [SerializeField]
    private GameObject dialogueBox;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [Space(5)]

    [Header("Fade Effect")]
    public GameObject fadePanel;
    public Animator _fadeAnimator;

    private string currentDateText;

    private CanvasGroup dialogueBoxCanvasGroup;

    // Singletone instance
    public static UiManager Instance { get; private set;}
#endregion

#region Unity Events
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

    private void OnEnable()
    {
        // 씬 로드 리스너 추가
        SceneManager.sceneLoaded += Initialize;
    }

    private void Start()
    {
        // 코루틴 객체 캐싱
        waitForEnter = new WaitUntil(() => _input.enter == true);
        waitForAssignCompleted = new WaitUntil(() => assignCompleted);
        waitForGmAssignCompleted = new WaitUntil(() => GameManager.Instance.gmAssignCompleted);
    }

    private void Update()
    {   
        UpdateHUD();
    }
#endregion

#region Initialization
    private void Initialize(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeCoroutine(scene));
    }


    private IEnumerator InitializeCoroutine(Scene scene)
    {
        isInDialogue = false;
        assignCompleted = false;

        typer = GetComponent<TypeEffect>();
        yield return waitForGmAssignCompleted;
        _player = GameManager.Instance._player;
        
        yield return new WaitUntil(() => scene.isLoaded);

        switch (GameManager.Instance.currentSceneIdx)
        {
            case 0: // 메인 씬
                break;
            case 1: // 컷씬
                Debug.Log("UI Manager: 컷씬에 진입하였으므로 UI 요소 할당을 시도합니다.");
                AssignCutsceneUIElements();
                break;
            case 2: // 컷씬
                Debug.Log("UI Manager: 컷씬에 진입하였으므로 UI 요소 할당을 시도합니다.");
                AssignCutsceneUIElements();
                break;
            case 3: // 플레이씬
                Debug.Log("UI Manager: 플레이씬에 진입하였으므로 UI 요소 할당을 시도합니다.");
                AssignPlaySceneUIElements();
                break;
            case 4: // 플레이씬
                Debug.Log("UI Manager: 플레이씬에 진입하였으므로 UI 요소 할당을 시도합니다.");
                AssignPlaySceneUIElements();
                break;
            default:
                Debug.Log("UI Manager: 알 수 없는 씬입니다.");
                break;
        }

        _input = GameManager.Instance._input;
    }

    private void AssignCutsceneUIElements()
    {
        canvasStatic = null;
        canvasDynamic = null;
        timeText = null;
        dateText = null;
        oxygenImage = null;
        
        canvasGeneral = FindObjectOfType<Canvas>().gameObject;
        if (canvasGeneral != null)
        {
            dialogueBox = canvasGeneral.transform.Find("Dialogue Box").gameObject;
            fadePanel = canvasGeneral.transform.Find("Fade Panel").gameObject;
            _fadeAnimator = fadePanel.GetComponent<Animator>();
            if (dialogueBox != null && fadePanel != null)
            {
                dialogueBoxCanvasGroup = dialogueBox.GetComponent<CanvasGroup>();
                nameText = dialogueBox.transform.GetComponentsInChildren<TextMeshProUGUI>()[0];
                dialogueText = dialogueBox.transform.GetComponentsInChildren<TextMeshProUGUI>()[1];
                typer.dialogueText = dialogueText;

                DisableDialogueBox();
                StartCoroutine(FadeIn());

                assignCompleted = true;
                Debug.Log("UI Manager: 컷씬의 UI 요소 할당을 성공적으로 완료했습니다");
            }
        }
    }

    private void AssignPlaySceneUIElements()
    {
        canvasStatic = GameObject.Find("HUD Static Canvas").gameObject;
        canvasDynamic = GameObject.Find("HUD Dynamic Canvas").gameObject;
        timeText = canvasDynamic.transform.GetComponentsInChildren<TextMeshProUGUI>()[0];
        dateText = canvasDynamic.transform.GetComponentsInChildren<TextMeshProUGUI>()[1];
        oxygenImage = canvasDynamic.transform.GetComponentInChildren<Image>();
        
        canvasGeneral = GameObject.Find("Canvas").gameObject;
        if (canvasGeneral != null)
        {
            dialogueBox = canvasGeneral.transform.Find("Dialogue Box").gameObject;
            fadePanel = canvasGeneral.transform.Find("Fade Panel").gameObject;
            _fadeAnimator = fadePanel.GetComponent<Animator>();
            if (dialogueBox != null && fadePanel != null)
            {
                dialogueBoxCanvasGroup = dialogueBox.GetComponent<CanvasGroup>();
                nameText = dialogueBox.transform.GetComponentsInChildren<TextMeshProUGUI>()[0];
                dialogueText = dialogueBox.transform.GetComponentsInChildren<TextMeshProUGUI>()[1];
                typer.dialogueText = dialogueText;

                DisableDialogueBox();
                StartCoroutine(FadeIn());
            }

            // Button들의 Index는 인스펙터 상의 순서와 일치해야 한다
            pausePanel = canvasGeneral.transform.Find("Pause Menu").gameObject;
            controlPanel = canvasGeneral.transform.Find("Control Panel").gameObject;
            gameoverPanel = canvasGeneral.transform.Find("Gameover Panel").gameObject;
            systemMessagePanel = canvasGeneral.transform.Find("System Message Panel").gameObject;
            novaPanels = canvasGeneral.transform.Find("Nova Panels").gameObject;

            if (pausePanel != null)
            {
                mainMenuBtn = pausePanel.transform.GetComponentsInChildren<Button>()[0];
                settingsBtn = pausePanel.transform.GetComponentsInChildren<Button>()[1];
                saveAndQuitBtn = pausePanel.transform.GetComponentsInChildren<Button>()[2];

                mainMenuBtn.onClick.RemoveAllListeners();
                mainMenuBtn.onClick.AddListener(() => GameManager.Instance.LoadScene(0));
            }

            if (controlPanel != null)
            {
                Transform btns = controlPanel.transform.Find("Buttons");
                Button[] tmpBtns = btns.GetComponentsInChildren<Button>();

                for (int i = 0; i < tmpBtns.Length; i++)
                    tmpBtns[i].onClick.RemoveAllListeners();

                charBtn = tmpBtns[0];
                mapBtn = tmpBtns[1];
                resourceBtn = tmpBtns[2];
                questBtn = tmpBtns[3];
                craftBtn = tmpBtns[4];

                charBtn.onClick.AddListener(OnCharTapClicked);
                mapBtn.onClick.AddListener(OnMapTapClicked);
                resourceBtn.onClick.AddListener(OnResourceTapClicked);
                questBtn.onClick.AddListener(OnQuestTapClicked);
                craftBtn.onClick.AddListener(OnCraftTapClicked);

                charTap = controlPanel.transform.Find("Char Tap").gameObject;
                mapTap = controlPanel.transform.Find("Map Tap").gameObject;
                resourceTap = controlPanel.transform.Find("Resource Tap").gameObject;
                questTap = controlPanel.transform.Find("Quest Tap").gameObject;
                craftTap = controlPanel.transform.Find("Craft Tap").gameObject;

                // Allocate All Detail Panel Components in Control Panel
                divider = controlPanel.transform.Find("Divider").gameObject;
                detailPanel = controlPanel.transform.Find("Detail Panel").gameObject;
                headerTxt = detailPanel.transform.Find("Header").GetComponent<TextMeshProUGUI>();
                itemImage = detailPanel.transform.Find("Item Image").GetComponent<Image>();
                descriptionTxt = detailPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                requiredTxt = detailPanel.transform.Find("Required Elements Text").GetComponent<TextMeshProUGUI>();
            }

            if (systemMessagePanel != null)
            {
                systemMessage = systemMessagePanel.GetComponentInChildren<TextMeshProUGUI>();
            }

            pausePanel.SetActive(false);
            controlPanel.SetActive(false);
            gameoverPanel.SetActive(false);
            systemMessagePanel.SetActive(false);
            novaPanels.SetActive(false);
        }

        assignCompleted = true;
        Debug.Log("UI Manager: 플레이씬의 UI 요소 할당을 성공적으로 완료했습니다");
    }
#endregion

    #region Dialogue
    /// <summary>
    /// 대화 시작 함수
    /// </summary>
    public void StartDialogue(int idx)
    {
        if (GameManager.Instance.canStartConversation && !isInDialogue)
        {
            _input.enter = false;
            GameManager.Instance.canStartConversation = false;
            isInDialogue = true;
            StartCoroutine(StartDialogueCoroutine(idx));
        }
    }

    // 대화 시작 코루틴
    private IEnumerator StartDialogueCoroutine(int idx)
    {
        yield return waitForAssignCompleted;
        EnableDialogueBox();

        Debug.Log($"UI Manager: 대화 사전으로부터 {idx}번째의 대화를 가져오려고 시도합니다");
        Queue<Dialogue> _dialoguesQueue = new Queue<Dialogue>(DialogueManager.Instance.LoadContext(idx));

        if (_dialoguesQueue == null)
            Debug.Log("UI Manager: 대화 큐에 대화를 불러오는 데 실패했습니다");
        else
        {
            int queueLength = _dialoguesQueue.Count;
            Debug.Log($"UI Manager: 대화 큐에 대화를 불러오는 데 성공했습니다. (Queue Length: {queueLength})");
            
            for (int i = 0; i < queueLength; i++)
            {
                Dialogue currentDialogue = _dialoguesQueue.Dequeue();
                
                nameText.text = currentDialogue.name;
                yield return StartCoroutine(typer.TypeCoroutine(currentDialogue.script));

                yield return waitForEnter;
                _input.enter = false;
            }
        }

        isInDialogue = false;
        DisableDialogueBox();

        if (idx == 1000)
            SceneDirector.Instance.IncreaseGameContext(); // 대화가 끝나면 게임 콘텍스트를 증가시킨다
    }

    private void EnableDialogueBox()
    {
        if (dialogueBox == null)
            return;

        if (!dialogueBox.activeSelf)
            dialogueBox.SetActive(true);

        dialogueBoxCanvasGroup.alpha = 1;
    }

    private void DisableDialogueBox()
    {
        if (dialogueBox == null)
            return;

        if (dialogueBox.activeSelf)
            dialogueBoxCanvasGroup.alpha = 0;
    }
#endregion

#region Fade Effect
    public IEnumerator FadeIn()
    {
        isFading = true;

        if (!fadePanel.activeSelf)
            fadePanel.SetActive(true);
        _fadeAnimator.SetTrigger("FadeIn");

        Debug.Log("UI Manager: Fade In . . .");
        yield return new WaitUntil(() => _fadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        isFading = false;
    }

    public IEnumerator FadeOut()
    {
        isFading = true;

        if (!fadePanel.activeSelf)
            fadePanel.SetActive(true);
        _fadeAnimator.SetTrigger("FadeOut");

        Debug.Log("UI Manager: Fade Out . . .");
        yield return new WaitUntil(() => _fadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        isFading = false;
    }
#endregion

#region Control UI

    // HUD 요소 업데이트 함수
    private void UpdateHUD()
    {
        // 날짜 & 시간 텍스트 업데이트
        if (dateText != null && timeText != null)
        {
            if (dateText.text != null && dateText.text != currentDateText)
            {
                dateText.text = string.Format("{0:D2}", TimeManager.Instance.currentDay) + " / " + 
                                string.Format("{0:D2}", TimeManager.Instance.currentMonth) + " / " + 
                                TimeManager.Instance.currentYear.ToString();
                currentDateText = dateText.text;
            }

            timeText.text = string.Format("{0:D2}", TimeManager.Instance.currentHour) + " : " + 
                            string.Format("{0:D2}", TimeManager.Instance.currentMinute) + " : " + 
                            string.Format("{0:D2}", (int)TimeManager.Instance.elapsedTime);
        }

        // 산소 레벨을 받아와 산소 레벨 이미지를 업데이트한다
        if (oxygenImage != null)
        {
            float oxygenRate = _player.GetOxygenRate();
            oxygenImage.fillAmount = oxygenRate;
        }
    }

    /// <summary>
    /// 모든 캔버스를 비활성화시키는 함수
    /// </summary>
    public void DisableAllCanvas()
    {
        canvasStatic.SetActive(false);
        canvasDynamic.SetActive(false);
        canvasGeneral.SetActive(false);
    }

    /// <summary>
    /// PlayScene에서 Esc키 입력 시 일시정지 패널을 활성화/비활성화하는 함수
    /// </summary>
    /// Used by GameManager.consumeInput_Esc()
    public void SetPausePanel()
    {
        if (pausePanel == null || !GameManager.Instance.isInPlayScene)
            return;

        if (!pausePanel.activeSelf) // 패널 활성화
        {
            pausePanel.SetActive(true);
            _input.BlockMoveAndLook();
            _input.SetCursor(false);
            GameManager.Instance.isGamePaused = true;
        }
        else // 패널 비활성화
        {
            pausePanel.SetActive(false);
            _input.UnlockInput();
            _input.SetCursor(true);
            GameManager.Instance.isGamePaused = false;
        }
    }
    
    /// <summary>
    /// PlayScene에서 C키 입력시 컨트롤 패널을 활성화/비활성화하는 함수
    /// Used by GameManager.consumeInput_OpenControl()
    /// </summary>
    public void SetControlPanel()
    {
        if (controlPanel == null || !GameManager.Instance.isInPlayScene)
            return;

        if (!controlPanel.activeSelf) // 패널 활성화
        {
            controlPanel.SetActive(true);
            OnCharTapClicked();
            _input.BlockMoveAndLook();
            _input.SetCursor(false);
            GameManager.Instance.isGamePaused = true;
        }
        else // 패널 비활성화
        {
            controlPanel.SetActive(false);
            _input.UnlockInput();
            _input.SetCursor(true);
            GameManager.Instance.isGamePaused = false;
        }
    }

    /// <summary>
    /// Control Panel내 Detail Panel을 활성화하고 현재 선택된 오브젝트의 정보를 넣어주는 함수
    /// </summary>
    public void SetDetailPanel()
    {
        if (!detailPanel.activeSelf)
        {
            divider.SetActive(true);
            detailPanel.SetActive(true);
        }
        
        Obj curObj = ObjectManager.selectedObj;

        if (curObj != null)
        {
            Debug.Log("UI Manager: 현재 선택된 오브젝트가 존재하므로 정보를 불러옵니다.");
            headerTxt.text = curObj.Name;
            itemImage.sprite = curObj.Image;
            descriptionTxt.text = curObj.Description;
            requiredTxt.text = "";
        }
        else
        {
            Debug.LogWarning("UI Manager: 선택된 오브젝트의 정보를 불러오는데 실패했습니다");
        }
    }

    /// <summary>
    /// Control Panel내 Detail Panel을 초기화하고 비활성화하는 함수
    /// </summary>
    public void ClearDetailPanel()
    {
        headerTxt.text = "";
        itemImage.sprite = null;
        descriptionTxt.text = "";
        requiredTxt.text = "";

        divider.SetActive(false);
        detailPanel.SetActive(false);
    }

    /// <summary>
    /// 플레이어 사망 시 GameOver 패널을 띄운다
    /// </summary>
    public void SetGameoverPanel()
    {
        if (gameoverPanel == null)
            return;
        
        _input.BlockMoveAndLook();
        gameoverPanel.SetActive(true);
    }

    public void SetSystemMessagePanel()
    {
        if (systemMessagePanel == null)
            return;
        
        StartCoroutine(SystemMessageCoroutine());
    }

    private IEnumerator SystemMessageCoroutine()
    {
        if (!systemMessagePanel.activeSelf)
            systemMessagePanel.SetActive(true);
        
        systemMessage.text = "";

        yield return new WaitForSeconds(3f);
    }

    public void OnRayDetectEnter(string rayHitName)
    {
        switch (rayHitName)
        {
            case "Spaceship":
                novaPanels.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void OnRayDetectExit()
    {
        if (novaPanels != null)
            novaPanels.SetActive(false);
    }
#endregion

#region 버튼 이벤트
    private void DisableAllTaps()
    {
        if (charTap != null && mapTap != null && resourceTap != null && questTap != null && craftTap != null)
        {
            charTap.SetActive(false);
            mapTap.SetActive(false);
            resourceTap.SetActive(false);
            questTap.SetActive(false);
            craftTap.SetActive(false);
            ClearDetailPanel();
        }
        else
            Debug.LogError("Tap을 찾을 수 없습니다");
    }

    public void OnCharTapClicked()
    {
        DisableAllTaps();

        if (charTap != null)
            charTap.SetActive(true);
    }

    public void OnMapTapClicked()
    {
        DisableAllTaps();

        if (mapTap != null)
            mapTap.SetActive(true);
    }

    public void OnResourceTapClicked()
    {
        DisableAllTaps();
        SetDetailPanel();

        if (resourceTap != null)
            resourceTap.SetActive(true);
    }
    
    public void OnQuestTapClicked()
    {
        DisableAllTaps();
        SetDetailPanel();

        if (questTap != null)
            questTap.SetActive(true);
    }

    public void OnCraftTapClicked()
    {
        DisableAllTaps();
        SetDetailPanel();

        if (craftTap != null)
            craftTap.SetActive(true);
    }
#endregion

}
