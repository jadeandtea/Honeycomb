using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    public static bool inMenu;
    public GameObject returnObject;
    public GameObject settingsObject;
    public GameObject menuObject;
    TMP_Text returnText;
    TMP_Text settingsText;
    TMP_Text menuText;

    public TextMeshProUGUI prompt;
    

    enum screen {
        Game, Esc, Settings, Confirmation
    }
    static screen currentScreen = screen.Game;

    public void Start() {
        currentScreen = screen.Game;
        returnText = returnObject.GetComponentInChildren<TMP_Text>();
        settingsText = settingsObject.GetComponentInChildren<TMP_Text>();
        menuText = menuObject.GetComponentInChildren<TMP_Text>();
        loadScene();
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            returnButton();
        }
    }

    public void returnButton() {
        if (currentScreen == screen.Confirmation) {
            currentScreen = screen.Settings;
        }
        else if(currentScreen == screen.Game || currentScreen == screen.Settings) {
            currentScreen = screen.Esc;
        } else{
            currentScreen = screen.Game;
        }
        loadScene();
    }

    public void settingsButton() {
        if (currentScreen == screen.Confirmation) {
            LevelManager.resetProgress();
        } else if(currentScreen == screen.Settings) {
            currentScreen = screen.Confirmation;
            loadScene();
        } else{
            currentScreen = screen.Settings;
            loadScene();
        }
    }

    public void menuButton() {
        if(currentScreen == screen.Esc) {
            SceneManager.LoadSceneAsync("Main Menu");
        }
        else if (currentScreen == screen.Confirmation) {
            currentScreen = screen.Settings;
        }
        else {
            currentScreen = screen.Esc;
        }
        loadScene();
    }

    public void loadScene() {
        switch (currentScreen) {
            case(screen.Game):
                returnObject.SetActive(false);
                settingsObject.SetActive(false);
                menuObject.SetActive(false);
                prompt.enabled = false;
                inMenu = false;
                break;
            case(screen.Esc):
                returnObject.SetActive(true);
                settingsText.text = "Settings";
                settingsObject.SetActive(true);
                menuText.text = "Menu";
                menuObject.SetActive(true);
                prompt.enabled = false;
                inMenu = true;
                break;
            case(screen.Settings):
                returnObject.SetActive(false);
                settingsText.text = "Reset Progress";
                settingsObject.SetActive(true);
                menuText.text = "Back";
                menuObject.SetActive(true);
                prompt.enabled = false;
                inMenu = true;
                break;
            case(screen.Confirmation):
                returnObject.SetActive(false);
                settingsText.text = "Yes";
                settingsObject.SetActive(true);
                menuText.text = "No";
                menuObject.SetActive(true);
                prompt.enabled = true;
                inMenu = true;
                break;
            default:
                returnObject.SetActive(false);
                settingsObject.SetActive(false);
                menuObject.SetActive(false);
                prompt.enabled = false;
                inMenu = false;
                break;
        }
    }
}
