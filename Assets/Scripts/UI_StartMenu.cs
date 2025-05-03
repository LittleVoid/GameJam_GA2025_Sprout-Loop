//Responsible for this script: Tony Meis
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_StartMenu : MonoBehaviour
{
    GameObject Credit_Panel, QuitGame_Panel;

    private void Awake()
    {
        Credit_Panel = transform.Find("Credit_Panel").gameObject;
        QuitGame_Panel = transform.Find("QuitGame_Panel").gameObject;
        Credit_Panel.SetActive(false);
        QuitGame_Panel.SetActive(false);
    }

    public void OpenCredits()
    {
        Credit_Panel.SetActive(true);
    }

    public void CloseCredits()
    {
        Credit_Panel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenQuitGameDialogue()
    {
        QuitGame_Panel.SetActive(true);
    }

    public void CloseQuitGameDialogue()
    {
        QuitGame_Panel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
