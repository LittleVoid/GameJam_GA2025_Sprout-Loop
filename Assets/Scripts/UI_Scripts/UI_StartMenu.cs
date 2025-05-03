//Responsible for this script: Tony Meis
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_StartMenu : MonoBehaviour
{
    GameObject Credit_Panel, QuitGame_Panel;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        Credit_Panel = transform.Find("Credit_Panel").gameObject;
        QuitGame_Panel = transform.Find("QuitGame_Panel").gameObject;
        Credit_Panel.SetActive(false);
        QuitGame_Panel.SetActive(false);
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
        audioManager.PlaySFX("Click");
        SceneManager.LoadScene(1);
        audioManager.SelectBackgroundMusic("Level");
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
        Application.Quit();
    }
}
