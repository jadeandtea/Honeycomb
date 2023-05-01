using UnityEngine;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public static int level;

    public GameObject Title;
    public GameObject levelSelectObject;
    public GameObject settingsObject;
    public GameObject quitObject;
    public GameObject levelsHost;
    TMP_Text levelSelectText;
    TMP_Text settingsText;
    TMP_Text quitText;
    

    enum screen {
        Main, LevelSelect, Settings, Level
    }
    static screen currentScreen = 0;

    public void Start() {
        levelSelectText = levelSelectObject.GetComponentInChildren<TMP_Text>();
        settingsText = settingsObject.GetComponentInChildren<TMP_Text>();
        quitText = quitObject.GetComponentInChildren<TMP_Text>();
        currentScreen = screen.Main;
        loadScene();
    }
    
    // TODO Make it so that a level completion brings the player directly to the level select screen
    // public void Awake(){
    //     DontDestroyOnLoad(gameObject);
    // }

    public void levelSelectScreen() {
        currentScreen = screen.LevelSelect;
        loadScene();
    }

    public void settingsScreen() {
        currentScreen = screen.Settings;
        loadScene();
    }

    public void mainMenuScreen() {
        if(currentScreen == screen.Main) {
            #if UNITY_EDITOR1
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        }
        else {
            currentScreen = screen.Main;
        }
        loadScene();
    }

    private void loadScene() {
        switch (currentScreen) {
            case screen.Settings:
                Title.SetActive(false);
                levelSelectObject.SetActive(false);
                settingsObject.SetActive(false);
                levelsHost.SetActive(false);
                quitText.text = "Main Menu";
                break;
            case screen.LevelSelect:
                Title.SetActive(false);
                levelSelectObject.SetActive(false);
                settingsObject.SetActive(false);
                levelsHost.SetActive(true);
                quitText.text = "Main Menu";
                break;
            case screen.Level:
                Title.SetActive(false);
                levelSelectObject.SetActive(false);
                settingsObject.SetActive(false);
                levelsHost.SetActive(false);
                quitText.text = "Main Menu";
                break;
            default:
                Title.SetActive(true);
                levelSelectObject.SetActive(true);
                settingsObject.SetActive(true);
                quitObject.SetActive(true);
                levelsHost.SetActive(false);
                break;
        }
    }
}
