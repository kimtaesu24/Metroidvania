using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject DoubleJumpPanel;
    public GameObject ThrowAttackPanel;
    public GameObject EndingPanel;

    private void ShowDoubleJumpPanel()
    {
        DoubleJumpPanel.SetActive(true);
    }

    private void ShowThrowAttackPanel()
    {
        ThrowAttackPanel.SetActive(true);
    }
    private void ShowEndingPanel()
    {
        EndingPanel.SetActive(true);
    }

    public void DoubleJumpPanelOff()
    {
        DoubleJumpPanel.SetActive(false);
    }

    public void ThrowAttackPanelOff()
    {
        ThrowAttackPanel.SetActive(false);
    }

    public void EndingPanelOff()
    {
        ExitGame();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // 어플리케이션 종료
        #endif
    }

    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
