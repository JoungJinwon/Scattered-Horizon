using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeEffect : MonoBehaviour
{
    private const float typeDeltaTime = 0.1f;
    
    private float typeSpeed = 1f;
    public float typeSpeedFast = 10f;
    public float typeSpeedNormal = 1f;

    private InputManager _input;

    WaitForSeconds WaitForNextType;

    public TextMeshProUGUI dialogueText;

    private void Awake()
    {
        WaitForNextType = new WaitForSeconds(typeDeltaTime * typeSpeed);
        
        _input = GameManager.Instance._input;
    }

    private void Update()
    {
        if (_input == null || !UiManager.Instance.isInDialogue)
            return;
        
        typeSpeed = _input.enterHold ? typeSpeedFast : typeSpeedNormal;
    }

    public IEnumerator TypeCoroutine(string txt)
    {
        if (_input == null)
            _input = GameManager.Instance._input;

        if (txt.Equals(""))
            yield break;
        
        dialogueText.text = null;

        for (int i = 0; i < txt.Length; i++)
        {
            WaitForNextType = new WaitForSeconds(typeDeltaTime * (1 / typeSpeed));

            dialogueText.text += txt[i];
            yield return WaitForNextType;
        }
    }
}
