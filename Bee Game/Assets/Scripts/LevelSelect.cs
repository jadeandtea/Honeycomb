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
    
    int currentScreen = 0;

    public void Start() {
        levelSelectText = levelSelectObject.GetComponentInChildren<TMP_Text>();
        settingsText = settingsObject.GetComponentInChildren<TMP_Text>();
        quitText = quitObject.GetComponentInChildren<TMP_Text>();
        Title.SetActive(true);
        levelSelectObject.SetActive(true);
        settingsObject.SetActive(true);
        quitObject.SetActive(true);
        levelsHost.SetActive(false);
        quitText.text = "Quit";
    }

    public void levelSelectScreen() {
        Title.SetActive(false);
        levelSelectObject.SetActive(false);
        settingsObject.SetActive(false);
        levelsHost.SetActive(true);
        // quitObject.SetActive(false);
        quitText.text = "Main Menu";

        currentScreen++;
    }

    public void settingsScreen() {
        Title.SetActive(false);
        levelSelectObject.SetActive(false);
        settingsObject.SetActive(false);
        levelsHost.SetActive(false);
        // quitObject.SetActive(false);
        quitText.text = "Main Menu";

        currentScreen++;
    }

    public void mainMenuScreen() {
        if(currentScreen == 0) {
            #if UNITY_EDITOR1
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
                Application.Quit();
        } else {
            Title.SetActive(true);
            levelSelectObject.SetActive(true);
            settingsObject.SetActive(true);
            quitObject.SetActive(true);
            levelsHost.SetActive(false);
            quitText.text = "Quit";
            currentScreen--;
        }
    }
}
