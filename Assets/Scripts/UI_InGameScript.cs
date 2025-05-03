// Responsible for this script: Tony Meis


using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_InGameScript : MonoBehaviour
{
    UI_InGameScript inGameUI;
    TMP_Text Counter_Text, Header_Text, Subline_Text, ScoreValue_Text;
    GameObject Pause_GameOver_Panel, Score_Panel, QuitGame_Panel;
    Button Continue, NextLevel, TryAgain, Settings, MainMenu, Quit;
    Slider Timer_Slider;
    float TimeLeft, MaxTime;

    int currentSceneIndex;

    void Awake()
    {
        if (inGameUI != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            inGameUI = GameObject.Find("inGameUI").GetComponent<UI_InGameScript>();
            Debug.Assert(inGameUI != null, "NO UI FOUND!!!");
        }
        DontDestroyOnLoad(this.gameObject);

        Pause_GameOver_Panel = transform.Find("Pause_GameOver_Panel").gameObject;
        Score_Panel = Pause_GameOver_Panel.transform.Find("Notification_Panel/Score_Panel").gameObject;
        QuitGame_Panel = Pause_GameOver_Panel.transform.Find("QuitGame_Panel").gameObject;
        Counter_Text = transform.Find("LoopCounter_Panel/Counter_Text").GetComponent<TMP_Text>();
        Header_Text = Pause_GameOver_Panel.transform.Find("Notification_Panel/Header_Text").GetComponent<TMP_Text>();
        Subline_Text = Pause_GameOver_Panel.transform.Find("Notification_Panel/Subline_Text").GetComponent<TMP_Text>();
        ScoreValue_Text = Pause_GameOver_Panel.transform.Find("Notification_Panel/ScoreValue_Text").GetComponent<TMP_Text>();
        Timer_Slider = transform.Find("Timer_Panel/Timer_Slider").GetComponent<Slider>();
        Continue = transform.Find("Button_Panel/Continue").GetComponent<Button>();
        NextLevel = transform.Find("Button_Panel/NextLevel").GetComponent<Button>();
        TryAgain = transform.Find("Button_Panel/TryAgain").GetComponent<Button>();
        Settings = transform.Find("Button_Panel/Settings").GetComponent<Button>();
        MainMenu = transform.Find("Button_Panel/MainMenu").GetComponent<Button>();
        Quit = transform.Find("Button_Panel/Quit").GetComponent<Button>();

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void StartNextLevel( float maxTime)
    {
        MaxTime = maxTime;
        TimeLeft = maxTime;
    }

    public void SpawnNextPlant (float maxTime)
    {
        MaxTime = maxTime;
        TimeLeft = maxTime;
        Timer_Slider.value = 1.0f;
        int counter = int.Parse(Counter_Text.text) + 1;
        Counter_Text.text = counter.ToString();
    }

    //Call this once in the main Update function
    public void UpdateTimer()
    {
        TimeLeft -= Time.deltaTime;
        Timer_Slider.value = TimeLeft / MaxTime;
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0.0f;
        SetButtonsAndPanels();
        NextLevel.gameObject.SetActive(false);
        Header_Text.text = "Pause";
        Subline_Text.text = "Everyone needs a break uwu";
        Pause_GameOver_Panel.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1.0f;
        Pause_GameOver_Panel.SetActive(false);
    }

    public void OpenWinScreen( string input)
    {
        Time.timeScale = 0.0f;
        SetButtonsAndPanels();
        Continue.gameObject.SetActive(false);
        TryAgain.gameObject.SetActive(false);
        //ScoreValue_Text.text = input;
        //Score_Panel.gameObject.SetActive(true);
        Header_Text.text = "Wohooo! You won!";
        Subline_Text.text = "Onto the next challenge!";
        Pause_GameOver_Panel.SetActive(true);
    }

    public void OpenLooseScreen(string input)
    {
        Time.timeScale = 0.0f;
        SetButtonsAndPanels();
        Continue.gameObject.SetActive(false);
        Header_Text.text = input;
        Subline_Text.text = ":/ That did not go so well";
        Pause_GameOver_Panel.SetActive(true);
    }

    private void SetButtonsAndPanels()
    {
        Settings.gameObject.SetActive(false); //Settings deactivated until further notice
        Score_Panel.gameObject.SetActive(false); //Score deactivated until further notice
        QuitGame_Panel.SetActive(false);
        Continue.gameObject.SetActive(true);
        NextLevel.gameObject.SetActive(true);
        TryAgain.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(true);
        Quit.gameObject.SetActive(true);
    }

    public void LoadNextLevel() 
    {
        SceneManager.LoadScene(currentSceneIndex++);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
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