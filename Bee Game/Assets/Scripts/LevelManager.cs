using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int currentLevelNumber = 0;
    public int level;
    public TMP_Text text;
    Button self;
    
    public void Awake() {
        text.text = level.ToString();

        self = this.GetComponent<Button>();

        //Makes button greyed out if the player hasn't completed the previous level
        self.interactable = PlayerPrefs.GetInt("levelReached", 1) >= level - 1;
    }

    public void onClick() {
        currentLevelNumber = level;
        SceneManager.LoadSceneAsync("Level Loader");
    }

    public static void levelComplete() {
        if(currentLevelNumber > PlayerPrefs.GetInt("levelReached", 0)) {
            PlayerPrefs.SetInt("levelReached", currentLevelNumber);
        }
    }

    public void resetProgress() {
        PlayerPrefs.SetInt("levelReached", 0);
    }
}
