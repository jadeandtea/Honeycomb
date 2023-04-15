using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI textHolder;
    void Start()
    {
        textHolder = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        hideText();
    }

    private void changeText(string text)
    {
        textHolder.text = text;
    }

    public void hideText() {
        gameObject.SetActive(false);
    }

    public void showText() {
        gameObject.SetActive(true);
    }
}
