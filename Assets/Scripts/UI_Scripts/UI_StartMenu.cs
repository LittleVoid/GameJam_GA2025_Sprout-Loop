//Responsible for this script: Tony Meis
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_StartMenu : MonoBehaviour
{
    [SerializeField] GameObject Credit_Panel, QuitGame_Panel;

    private void Awake()
    {
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
        Debug.Log("quit");
        Application.Quit();
    }
}
