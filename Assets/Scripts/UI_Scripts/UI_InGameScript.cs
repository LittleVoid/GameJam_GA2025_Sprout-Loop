// Responsible for this script: Tony Meis


using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor.SearchService;

public class UI_InGameScript : MonoBehaviour
{
    [SerializeField] TMP_Text Counter_Text, Header_Text, Subline_Text, ScoreValue_Text;
    [SerializeField] GameObject Pause_GameOver_Panel, Score_Panel, QuitGame_Panel;
    [SerializeField] Button Continue, NextLevel, TryAgain, Settings, MainMenu, Quit;
    [SerializeField] Slider Timer_Slider;
    float TimeLeft, MaxTime;

    int currentSceneIndex;

    void Awake()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void StartNextLevel(float maxTime)
    {
        MaxTime = maxTime;
        TimeLeft = maxTime;

        Pause_GameOver_Panel.SetActive(false);
        Score_Panel.SetActive(false);
        QuitGame_Panel.SetActive(false);
        AudioManager.Instance.SelectBackgroundMusic("Level");
    }

    public void SpawnNextPlant(float maxTime)
    {
        MaxTime = maxTime;
        TimeLeft = maxTime;
        Timer_Slider.value = 1.0f;
        int counter = int.Parse(Counter_Text.text) + 1;
        Counter_Text.text = counter.ToString();
    }

    //Call this once in the main Update function
    public void UpdateTimer(float timeleft)
    {
        TimeLeft = timeleft;
        Timer_Slider.value = TimeLeft / MaxTime;
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0.0f;
        SetButtonsAndPanels();
        NextLevel.gameObject.SetActive(false);
        Header_Text.text = "Pause";
        Subline_Text.text = "Everyone needs a break uwu";
        AudioManager.Instance.SelectBackgroundMusic("Pause");
        Pause_GameOver_Panel.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1.0f;
        Pause_GameOver_Panel.SetActive(false);
        AudioManager.Instance.SelectBackgroundMusic("Level");
        AudioManager.Instance.PlaySFX("Click");
    }

    public void OpenWinScreen(string input)
    {
        Time.timeScale = 0.0f;
        SetButtonsAndPanels();
        Continue.gameObject.SetActive(false);
        TryAgain.gameObject.SetActive(false);
        //ScoreValue_Text.text = input;
        //Score_Panel.gameObject.SetActive(true);
        Header_Text.text = "Wohooo! You won!";
        Subline_Text.text = "Onto the next challenge!";
        AudioManager.Instance.SelectBackgroundMusic("Win");
        AudioManager.Instance.PlaySFX("Victory");
        Pause_GameOver_Panel.SetActive(true);
    }

    public void OpenLooseScreen(string input)
    {
        Time.timeScale = 0.0f;
        SetButtonsAndPanels();
        Continue.gameObject.SetActive(false);
        Header_Text.text = input;
        Subline_Text.text = ":/ That did not go so well";
        AudioManager.Instance.SelectBackgroundMusic("Lose");
        AudioManager.Instance.PlaySFX("GameOver");
        Pause_GameOver_Panel.SetActive(true);
    }

    private void SetButtonsAndPanels()
    {
        Settings.gameObject.SetActive(false); //Settings deactivated until further notice
        Score_Panel.gameObject.SetActive(false); //Score deactivated until further notice
        QuitGame_Panel.SetActive(false);
        Continue.gameObject.SetActive(true);
        NextLevel.gameObject.SetActive(false);
        TryAgain.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(true);
        Quit.gameObject.SetActive(true);
    }

    public void LoadNextLevel()
    {
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadScene(currentSceneIndex++);
        AudioManager.Instance.SelectBackgroundMusic("Level");
    }

    public void RestartLevel()
    {
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadScene(currentSceneIndex);
        AudioManager.Instance.SelectBackgroundMusic("Level");
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadScene(0);
        AudioManager.Instance.SelectBackgroundMusic("MainMenu");
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
        Application.Quit();
    }
}