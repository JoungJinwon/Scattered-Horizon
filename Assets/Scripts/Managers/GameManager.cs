using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isInPlayScene;
    public bool isGamePaused;
    public bool canStartConversation;
    public int currentSceneIdx; // 현재 씬 인덱스
    public bool gmAssignCompleted = false;

    public WaitUntil WaitForEnter;

    public InputManager _input;
    public PlayerStats _player;

    // Singletone instance
    public static GameManager Instance { get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        _input = GetComponent<InputManager>();
        WaitForEnter = new WaitUntil(() => _input.enter == true);
        
        canStartConversation = false;

        StartCoroutine(consumeInput_Esc());
        StartCoroutine(consumeInput_OpenControl());
    }

    /// <summary>
    /// 플레이어 사망시 처리를 담당하는 함수이다
    /// </summary>
    public void GameOver()
    {
        UiManager.Instance.SetGameoverPanel();
    }

#region 씬 관리
    /// <summary>
    /// 씬 로드 함수
    /// </summary>
    /// <param name="sceneIdx">Build Settings에 등록된 씬의 index</param>
    public void LoadScene(int sceneIdx)
    {
        if (sceneIdx >= 0)
        {
            Debug.Log($"{sceneIdx}번 씬을 로드합니다");
            StartCoroutine(LoadSceneAsynchronously(sceneIdx));
        }
        else
            Debug.LogWarning("잘못된 Scene Index로 접근하려 하고 있습니다.");
    }

    // 씬 로드 코루틴 (씬 로드 함수에 의해 사용)
    private IEnumerator LoadSceneAsynchronously(int sceneIdx)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIdx);
        operation.allowSceneActivation = false;

        yield return StartCoroutine(UiManager.Instance.FadeOut());

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f && !UiManager.Instance.isFading)
                operation.allowSceneActivation = true;

            yield return null;
        }
    }

    public IEnumerator LoadSceneWaitForEnter(int sceneIdx)
    {
        yield return WaitForEnter;
        LoadScene(sceneIdx);
    }

    // 씬 로드 시 초기화를 담당한다
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gmAssignCompleted = false;

        currentSceneIdx = scene.buildIndex;
        isInPlayScene = currentSceneIdx > 0;

        if (SceneDirector.Instance != null)
        {
            switch (SceneDirector.Instance.GetGameContext())
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    Debug.Log("Game Manager: 플레이씬 초기화 코드는 여기에");
                    UiManager.Instance.StartDialogue(1001);
                    break;
                default:
                    Debug.Log("Game Manager: Game Context에 해당하는 초기화 코드가 존재하지 않습니다.");
                    break;
            }
        }

        if (scene.buildIndex < 3) // 컷씬을 의미, 추후 수정
        {
            canStartConversation = true;

            PlayerInput _playerInput = GetComponent<PlayerInput>();

            if (_playerInput != null)
            {
                _playerInput.enabled = false;
                _playerInput.enabled = true;
                Debug.Log("Game Manager: Player Input 초기화 완료");
            }
            else
                Debug.LogError("Game Manager의 Player Input 컴포넌트를 찾지 못했습니다.");
            
        }
        else if (scene.buildIndex == 3) // 플레이씬을 의미, 추후 수정
        {
            canStartConversation = true;
            
            Destroy(GetComponent<InputManager>());
            Destroy(GetComponent<PlayerInput>());
            
            _player = FindAnyObjectByType<PlayerStats>();
            // _player.InitializeComponents();
            _input = _player.GetComponent<InputManager>();
        }

        gmAssignCompleted = true;
    }
#endregion

#region 키 입력 소비 코루틴
    // ESC
    public IEnumerator consumeInput_Esc()
    {
        if (!_input)
        {
            Debug.LogWarning("Input{ESC}를 소비할 수 없습니다");
            yield break;
        }
        
        while (true)
        {
            yield return new WaitUntil(() => _input.gamePause);
            _input.gamePause = false;

            if (isInPlayScene)
                UiManager.Instance.SetPausePanel();
        }
    }

    // C
    public IEnumerator consumeInput_OpenControl()
    {
        if (!_input)
        {
            Debug.LogWarning("Input{OpenControl}를 소비할 수 없습니다");
            yield break;
        }
        
        while (true)
        {
            yield return new WaitUntil(() => _input.openControl);
            _input.openControl = false;

            if (isInPlayScene)
                UiManager.Instance.SetControlPanel();
        }
    }
#endregion
}
