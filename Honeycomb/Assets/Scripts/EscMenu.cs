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
    

    enum screen {
        Game, Esc, Settings
    }
    static screen currentScreen = screen.Game;

    void Start() {
        currentScreen = screen.Game;
        returnText = returnObject.GetComponentInChildren<TMP_Text>();
        settingsText = settingsObject.GetComponentInChildren<TMP_Text>();
        menuText = menuObject.GetComponentInChildren<TMP_Text>();
        loadScene();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            returnButton();
        }
    }

    void returnButton() {
        if(currentScreen == screen.Game) {
            currentScreen = screen.Esc;
        } else{
            currentScreen = screen.Game;
        }
        loadScene();
    }

    void settingsButton() {
        currentScreen = screen.Settings;
        menuText.text = "Back";
        loadScene();
    }

    void menuButton() {
        if(currentScreen == screen.Esc) {
            SceneManager.LoadSceneAsync("Main Menu");
        }
        else {
            currentScreen = screen.Esc;
            menuText.text = "Menu";
        }
        loadScene();
    }

    void loadScene() {
        switch (currentScreen) {
            case(screen.Game):
                returnObject.SetActive(false);
                settingsObject.SetActive(false);
                menuObject.SetActive(false);
                inMenu = false;
                break;
            case(screen.Esc):
                returnObject.SetActive(true);
                settingsObject.SetActive(true);
                menuObject.SetActive(true);
                inMenu = true;
                break;
            case(screen.Settings):
                returnObject.SetActive(false);
                settingsObject.SetActive(false);
                menuObject.SetActive(true);
                inMenu = true;
                break;
            default:
                returnObject.SetActive(false);
                settingsObject.SetActive(false);
                menuObject.SetActive(false);
                inMenu = false;
                break;
        }
    }
}
