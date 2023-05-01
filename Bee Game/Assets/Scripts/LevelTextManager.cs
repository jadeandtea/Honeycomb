using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTextManager : MonoBehaviour
{
    public TextMeshProUGUI textHolder;
    bool isRunning = false;
    // public float fontSize = 36;
    void Start()
    {
        textHolder = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        hideText();
    }

    void Update() {
    }

    private void changeText(string text)
    {
        textHolder.text = text;
    }

    public void hideText() {
        textHolder.enabled = false;
    }

    public void showText() {
        textHolder.enabled = true;
    }

    public void animate() {
        if(!isRunning) {
            StartCoroutine("animateText");
        }
    }

    IEnumerator animateText() {
        isRunning = true;
        showText();
        //TODO make text expand from nothing and shake or something cool, the shrink away
        //StartCoroutine("animateText");
        float refSize = 2;
        float i;
        for(i = 3; i < 6; i += 0.04f) {
            textHolder.fontSize = Mathf.Pow(refSize, i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        for(i = 6; i > 3; i -= 0.04f) {
            textHolder.fontSize = Mathf.Pow(refSize, i);
            yield return new WaitForSeconds(0.01f);
        }
        hideText();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadSceneAsync("Main Menu");
        
    }
}
