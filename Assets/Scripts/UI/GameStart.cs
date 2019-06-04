using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    [Tooltip("Play Game Btn")]
    public Button startBtn;

    public List<Button> levelBtns;

    public GameObject levelPanel;
    public GameObject quitPanel;

    public Button sureBtn;
    public Button cancelBtn;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;

        startBtn.onClick.AddListener(() =>
        {
            levelPanel.SetActive(true);
        });
        for(int i = 0; i < levelBtns.Count; ++i)
        {
            int num = i;
            levelBtns[num].onClick.AddListener(() =>
            {
                SceneManager.LoadScene(num + 1);
            });
        }
        sureBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        cancelBtn.onClick.AddListener(() =>
        {
            quitPanel.SetActive(false);
        });
    }


    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (levelPanel.activeSelf)
            {
                levelPanel.SetActive(false);
            }
            else 
            {
                quitPanel.SetActive(!quitPanel.activeSelf);
            }
        }
    }
}
