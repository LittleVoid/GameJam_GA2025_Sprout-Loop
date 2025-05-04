//Responsible for this script: Tony Meis
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;


public class UI_StartMenu : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField] GameObject Credit_Panel, QuitGame_Panel;


    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        Credit_Panel.SetActive(false);
        QuitGame_Panel.SetActive(false);

        audioManager.SelectBackgroundMusic("MainMenu");
    }

    public void OpenCredits()
    {
        audioManager.PlaySFX("Click");
        Credit_Panel.SetActive(true);
    }

    public void CloseCredits()
    {
        audioManager.PlaySFX("Click");
        Credit_Panel.SetActive(false);
    }

    public void StartGame()
    {
        audioManager.SelectBackgroundMusic("Level");
        SceneManager.LoadScene(1);
    }

    public void OpenQuitGameDialogue()
    {
        audioManager.PlaySFX("Click");
        QuitGame_Panel.SetActive(true);
    }

    public void CloseQuitGameDialogue()
    {
        audioManager.PlaySFX("Click");
        QuitGame_Panel.SetActive(false);
    }
    public void QuitGame()
    {
        audioManager.PlaySFX("Click");
        Debug.Log("quit");
        Application.Quit();
    }
}