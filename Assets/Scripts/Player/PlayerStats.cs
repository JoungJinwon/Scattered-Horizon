using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public bool isOxygenSafe;
    private bool isPlayerDead;

    [Header("Raycast")]
    [SerializeField]
    private float rayLength; // 발사할 Ray의 길이
    private float rayYAxisPreset = 1.1f; // Ray 발사 원점을 플레이어의 가슴쪽으로 조정하기 위한 값
    public string rayDetectedName;

    [Header("Oxygen Reduction")]
    [Tooltip("Player's Maximum Oxygen Level")]
    public float maxOxygenLvl = 100f;
    [HideInInspector]
    public float curOxygenLvl;
    public float oxygenDamage = 1f;

    private void Start()
    {
        InitializePlayerStats();
    }

    private void OnEnable()
    {
        InitializeComponents();
    }

    private void Update()
    {
        if (GameManager.Instance.currentSceneIdx < 2)
            return;
        
        RenderRay();
        SetOxygenLvl();
    }

    /// <summary>
    ///  플레이어 앞의 방향으로 레이를 발사하여 물체를 감지한다
    ///  플레이 창과 씬 창에서 동일한 레이를 발사한다
    /// </summary>
    private void RenderRay()
    {
        // Ray 발사 원점
        Vector3 rayPosition = new Vector3(transform.position.x, transform.position.y + rayYAxisPreset, transform.position.z);
        
        Debug.DrawRay(rayPosition, transform.forward * rayLength, Color.red);
        RaycastHit hitInfo;
        Physics.Raycast(rayPosition, transform.forward, out hitInfo, rayLength);

        // 레이캐스팅에 감지된 물체를 검사한다
        if (hitInfo.collider != null)
        {
            Collider tmpCollider = hitInfo.collider;

            if (rayDetectedName != tmpCollider.name) // 감지된 새로운 오브젝트
            {
                /** 후에 tag나 layer를 검사하는 방식으로 수정할 것 **/
                rayDetectedName = tmpCollider.name;

                Debug.Log("상호작용: " + rayDetectedName);
                
                if (tmpCollider.CompareTag("Spaceship"))
                {
                    UiManager.Instance.OnRayDetectEnter("Spaceship");
                    StartCoroutine(GameManager.Instance.LoadSceneWaitForEnter(4));
                }

                // if (rayDetectedName == "rocket"  && !UiManager.Instance.isInDialogue)
                // {
                //     GameManager.Instance.canStartConversation = true;
                // }
            }
        }
        else
        {
            GameManager.Instance.canStartConversation = false;
            rayDetectedName = "";
            UiManager.Instance.OnRayDetectExit();
            StopCoroutine(GameManager.Instance.LoadSceneWaitForEnter(4));
        }
    }

    private void SetOxygenLvl()
    {
        if (!isOxygenSafe && !isPlayerDead && !GameManager.Instance.isGamePaused)
        { // 현재 위험 지대이고 플레이어가 살아 있으며 게임 정지 상태가 아니라면 산소 레벨을 감소시킨다
            if (curOxygenLvl > 0)
                curOxygenLvl -= oxygenDamage * Time.deltaTime;
            else
            {
                curOxygenLvl = 0;
                isPlayerDead = true;
                GameManager.Instance.GameOver();
            }
        }

        /*
            산소 회복 코드 구현
        */
    }

    public float GetOxygenRate() { return curOxygenLvl / maxOxygenLvl; }

#region 초기화
    /// <summary>
    /// 플레이어 초기값 세팅
    /// </summary>
    private void InitializePlayerStats()
    {
        isPlayerDead = false;
        rayDetectedName = "";
        curOxygenLvl = maxOxygenLvl;
    }

    /// <summary>
    /// 씬 이동 시 PlayerInput이 멈추는 경우를 해결하기 위해 컴포넌트를 껐다 켜준다
    /// GameManager의 OnSceneLoaded()에서 사용된다
    /// </summary>
    public void InitializeComponents()
    {
        StartCoroutine(InitializeComponentsCoroutine());
    }

    private IEnumerator InitializeComponentsCoroutine()
    {
        yield return UiManager.Instance.waitForAssignCompleted;

        PlayerInput _playerInput = GetComponent<PlayerInput>();

        if (_playerInput != null)
        {
            _playerInput.enabled = false;
            _playerInput.enabled = true;

            Debug.Log("PlayerStats: Player Input 컴포넌트 초기화 완료.");
        }
        else
            Debug.LogError("Player의 Player Input 컴포넌트를 찾지 못했습니다.");
    }
#endregion

}
