using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputManager : MonoBehaviour
{
    [Header("Character Input Values")]
    public bool jump;
    public bool sprint;
    public bool switchView;
    public bool gamePause;
    public bool openControl;
    public bool enter;
    public bool enterHold;
    public float scroll;
    public Vector2 move;
    public Vector2 look;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [HideInInspector]
    public bool IsBlocked {get; private set;}

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if(cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnScroll(InputValue value)
    {
        ScrollInput(value.Get<float>());
    }

    public void OnSwitchView(InputValue value)
    {
        SwitchViewInput(value.isPressed);
    }

    public void OnGamePause(InputValue value)
    {
        GamePauseInput(value.isPressed);
    }

    public void OnOpenControl(InputValue value)
    {
        OpenControlInput(value.isPressed);
    }

    public void OnEnter(InputValue value)
    {
        EnterInput(value.isPressed);
    }

    public void OnEnterHold(InputValue value)
    {
        EnterHoldInput(value.isPressed);
    }

    public void OnEnterRelease(InputValue value)
    {
        enter = false;
        enterHold = false;

        Debug.Log("Enter Released!");
    }
#endif

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    } 

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void ScrollInput(float newScrollValue)
    {
        scroll = newScrollValue;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void SwitchViewInput(bool newSwitchViewState)
    {
        switchView = newSwitchViewState;
    }

    public void GamePauseInput(bool newGamePauseState)
    {
        gamePause = newGamePauseState;
    }

    public void OpenControlInput(bool newOpenControlState)
    {
        openControl = newOpenControlState;
    }

    public void EnterInput(bool newEnterState)
    {
        enter = newEnterState;
        Debug.Log("Enter Clicked!");
    }

    public void EnterHoldInput(bool newEnterHoldState)
    {
        enterHold = newEnterHoldState;
        enter = false;
        Debug.Log("Enter Holding . . .");
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void BlockMove()
    {
        IsBlocked = true;
        
        move = Vector2.zero;
    }

    public void BlockMoveAndLook()
    {
        IsBlocked = true;

        move = Vector2.zero;
        look = Vector2.zero;
    }

    public void UnlockInput()
    {
        IsBlocked = false;
    }

    public void SetCursor(bool cursor)
    {
        cursorLocked = cursor;
        SetCursorState(cursorLocked);
    }
}
