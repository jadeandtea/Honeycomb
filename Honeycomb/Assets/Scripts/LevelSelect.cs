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
        Main, LevelSelect, Settings
    }
    static screen currentScreen = screen.Main;

    public void Start() {
        levelSelectText = levelSelectObject.GetComponentInChildren<TMP_Text>();
        settingsText = settingsObject.GetComponentInChildren<TMP_Text>();
        quitText = quitObject.GetComponentInChildren<TMP_Text>();
        loadScene();
    }

    public void levelSelectButton() {
        currentScreen = screen.LevelSelect;
        loadScene();
    }

    public void settingsButton() {
        if(currentScreen == screen.Settings) {
            LevelManager.resetProgress();
        } else {
            currentScreen = screen.Settings;
            loadScene();
        }
    }

    public void quitButton() {
        if(currentScreen == screen.Main) {
            #if UNITY_EDITOR
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
            case screen.Main:
                Title.SetActive(true);
                levelSelectObject.SetActive(true);
                settingsObject.SetActive(true);
                quitObject.SetActive(true);
                levelsHost.SetActive(false);
                settingsText.text = "Settings";
                quitText.text = "Quit";
                break;
            case screen.Settings:
            // TODO Setup a settings screen to modify controls (?), change map orientation, color scheme, or other things idk what
                Title.SetActive(false);
                levelSelectObject.SetActive(false);
                settingsObject.SetActive(true);
                levelsHost.SetActive(false);
                settingsText.text = "Reset Progress";
                quitText.text = "Main Menu";
                break;
            case screen.LevelSelect:
                Title.SetActive(false);
                levelSelectObject.SetActive(false);
                settingsObject.SetActive(false);
                levelsHost.SetActive(true);
                quitText.text = "Main Menu";
                break;
            default:
                Title.SetActive(true);
                levelSelectObject.SetActive(true);
                settingsObject.SetActive(true);
                quitObject.SetActive(true);
                levelsHost.SetActive(false);
                settingsText.text = "Settings";
                quitText.text = "Quit";
                break;
        }
    }
}
