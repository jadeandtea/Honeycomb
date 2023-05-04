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
    bool isFading = false;

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
        Debug.Log("Level " + LevelManager.currentLevelNumber + 1 + " complete");
        if(!isRunning) {
            StartCoroutine("animateText");
        }
    }
    
    public void FadeTextToFullAlpha() {
        if(!isFading) {
            StartCoroutine(FadeTextToFullAlpha(1, textHolder));
        }
    }

    public void FadeTextToZeroAlpha() {
        if(!isFading) {
            StartCoroutine(FadeTextToZeroAlpha(1, textHolder));
        }
    }

    IEnumerator animateText() {
        isRunning = true;
        showText();
        float i;

        for(i = 0.05f; i < 1; i *= 1.02f) {
            textHolder.fontSize = Mathf.Lerp(0, 100, i);
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(1f);
        for(i = 0.05f; i < 1; i *= 1.02f) {
            textHolder.fontSize = Mathf.Lerp(100, 0, i);
            yield return new WaitForSeconds(0.005f);
        }
        isRunning = false;
        SceneManager.LoadSceneAsync("Main Menu");
        
    }

    //https://forum.unity.com/threads/fading-in-out-gui-text-with-c-solved.380822/
    IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        showText();
        isFading = true;
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        isFading = false;
    }
 
    IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {  
        isFading = true;
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        hideText();
    }
}
