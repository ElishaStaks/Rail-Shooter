using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject m_OptionPanel;

    [SerializeField] 
    private string m_GameplayScene;

    [SerializeField]
    private Button m_StartButton;

    [SerializeField]
    private Button m_OptionButton;

    [SerializeField]
    private Button m_OptionCloseButton;

    [SerializeField]
    private Button m_QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        m_OptionPanel.SetActive(false);
        m_StartButton.Select();

        m_StartButton.onClick.AddListener(StartGame);
        m_OptionButton.onClick.AddListener(OpenOptionPanel);
        m_OptionCloseButton.onClick.AddListener(CloseOptionPanel);
        m_QuitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        if (m_OptionPanel.activeInHierarchy)
            return;

        SceneManager.LoadScene(m_GameplayScene);
    }

    void OpenOptionPanel()
    {
        m_OptionCloseButton.Select();
        m_OptionPanel.SetActive(true);
    }

    void CloseOptionPanel()
    {
        m_OptionPanel.SetActive(false);
        m_OptionButton.Select();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
