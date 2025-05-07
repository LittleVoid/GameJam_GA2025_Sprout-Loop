//Responsible for this script: Tony Meis

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    private static AudioManager audioManager;
    [SerializeField] AudioSource backgroundPlayer, SFXPlayer;

    [SerializeField] AudioClip MainMenu, Level, PauseMenu, Win;
    [SerializeField] List<AudioClip> Clicks;
    [SerializeField] AudioClip Splash, Jump, Grow, Sprout, Whoosh, Victory, GameOver;

    public static AudioManager Instance => audioManager;

    private void Awake()
    {
        if (audioManager != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            audioManager = this;
            Debug.Assert(audioManager != null, "NO UI FOUND!!!");
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //backgroundPlayer = transform.Find("BG_Player").GetComponent<AudioSource>();
        //SFXPlayer = transform.Find("SFX_Player").GetComponent<AudioSource>();

        backgroundPlayer.clip = MainMenu;
      
    }

    public void SelectBackgroundMusic(string input)
    {
        switch (input)
        {
            case "Pause":
                backgroundPlayer.clip = PauseMenu;
                break;
            case "MainMenu":
                backgroundPlayer.clip = MainMenu;
                break;
            case "Level":
                backgroundPlayer.clip = MainMenu;
                break;
            case "Win":
                backgroundPlayer.clip = Win;
                break;
            case "Lose":
                backgroundPlayer.Stop();
                break;
            default:
                break;
        }
        backgroundPlayer.Play();
    }

    public void PlaySFX(string input)
    {
        switch (input) 
        {
            case "Jump":
                SFXPlayer.clip = Jump;
                break;           
            case "Splash":
                SFXPlayer.clip = Splash;
                break;   
            case "Grow":
                SFXPlayer.clip = Grow;
                break;            
            case "Sprout":
                SFXPlayer.clip = Sprout;
                break;            
            case "Whoosh":
                SFXPlayer.clip = Whoosh;
                break;            
            case "Victory":
                SFXPlayer.clip = Victory;
                break;            
            case "GameOver":
                SFXPlayer.clip = GameOver;
                break;            
            case "Click":
                int rd = Random.Range(0, Clicks.Count);
                SFXPlayer.clip = Clicks[rd];
                break;            
            default:
                break;
        }
        SFXPlayer.Play();
    }
}