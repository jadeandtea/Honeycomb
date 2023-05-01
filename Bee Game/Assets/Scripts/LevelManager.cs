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
    
    public void Start() {
        text.text = level.ToString();

        self = this.GetComponent<Button>();
        // self.interactable = PlayerPrefs.GetInt("levelReached", 0) >= level - 1;
    }

    public void onClick() {
        currentLevelNumber = level;
        Level.loadLevel();
        SceneManager.LoadSceneAsync("Level Loader");
    }

    public static void levelComplete() {
        if(currentLevelNumber > PlayerPrefs.GetInt("levelReached", 0)) {
            PlayerPrefs.SetInt("levelReached", currentLevelNumber);
        }
    }
}
