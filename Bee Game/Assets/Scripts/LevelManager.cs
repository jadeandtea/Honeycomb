using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int currentLevel;
    public int level;
    public TMP_Text text;
    Button self;
    public void Start() {
        text.text = level.ToString();

        self = this.GetComponent<Button>();
        self.interactable = PlayerPrefs.GetInt("levelReached", 0) >= level - 1;
    }

    public void onClick() {
        currentLevel = level;
        SceneManager.LoadSceneAsync("Level Loader");
    }

    public static void advanceLevel() {
        if(currentLevel > PlayerPrefs.GetInt("levelReached", 0)) {
            PlayerPrefs.SetInt("levelReached", currentLevel);
        }
    }
}
