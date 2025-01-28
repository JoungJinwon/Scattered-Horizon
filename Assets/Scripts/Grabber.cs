using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grabber : MonoBehaviour
{
    public GameManager _GM;
    public UiManager _UM;
    public SceneDirector _SD;
    public ObjectManager _OM;

    private void Awake()
    {
        SceneManager.sceneLoaded += InitGrabber;
    }

#region Initialize
    private void InitGrabber(Scene scene, LoadSceneMode mode)
    {
        if (_GM == null)
            _GM = GameManager.Instance;
        if (_UM == null)
            _UM = UiManager.Instance;
        if (_SD == null)
            _SD = SceneDirector.Instance;
        if (_OM == null)
            _OM = ObjectManager.Instance;
    }
#endregion

#region Game Manager
    public void LoadScene_G(int sceneIdx)
    {
        _GM.LoadScene(sceneIdx);
    }
#endregion

#region UI Manager
    public void StartDialogue_G(int dialogueIdx)
    {
        _UM.StartDialogue(dialogueIdx);
    }

    public void SetControlPanel_G()
    {
        _UM.SetControlPanel();
    }

    public void SetDetailPanel_G()
    {
        _UM.SetDetailPanel();
    }
#endregion

#region Object Manager
    public void GetObjIdx_G()
    {
        ObjectManager.selectedObj.GetObjIdx();
    }

    public void GetObjNum_G()
    {
        ObjectManager.selectedObj.GetobjNum();
    }

    public void SetObjInfo_G(int idx)
    {
        _OM.SetObjInfo(idx, 0);
        _UM.SetDetailPanel();
    }
#endregion

#region Scene Director
    public void IncreaseGameContext_G()
    {
        _SD.IncreaseGameContext();
    }
#endregion

}
