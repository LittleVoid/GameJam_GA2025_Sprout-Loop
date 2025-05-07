//Responsible for this script: Tony Meis
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;


public class UI_StartMenu : MonoBehaviour
{
    [SerializeField] GameObject Credit_Panel, QuitGame_Panel;

    private void Start()
    {
        Credit_Panel.SetActive(false);
        QuitGame_Panel.SetActive(false);

        AudioManager.Instance.SelectBackgroundMusic("MainMenu");
    }

    public void OpenCredits()
    {
        AudioManager.Instance.PlaySFX("Click");
        Credit_Panel.SetActive(true);
    }

    public void CloseCredits()
    {
        AudioManager.Instance.PlaySFX("Click");
        Credit_Panel.SetActive(false);
    }

    public void StartGame()
    {
        AudioManager.Instance.SelectBackgroundMusic("Level");
        SceneManager.LoadScene(1);
    }

    public void OpenQuitGameDialogue()
    {
        AudioManager.Instance.PlaySFX("Click");
        QuitGame_Panel.SetActive(true);
    }

    public void CloseQuitGameDialogue()
    {
        AudioManager.Instance.PlaySFX("Click");
        QuitGame_Panel.SetActive(false);
    }
    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX("Click");
        Debug.Log("quit");
        Application.Quit();
    }
}