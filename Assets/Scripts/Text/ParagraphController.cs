using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Controll the paragraphs
/// </summary>

public class ParagraphController : MonoBehaviour
{
    public Paragraph _currentParagraph;
    public TextMeshProUGUI _displayText;
    public GameObject[] _buttons = new GameObject[2];
    public TextMeshProUGUI[] _buttonText = new TextMeshProUGUI[2];
    private Dictionary<string, Paragraph> _exitDictionary = new Dictionary<string, Paragraph>();
    // [HideInInspector] public List<string> _exitsDescriptionInParagraph = new List<string>();
    private int _buttonNum;

    private void Awake()
    {

    }

    private void Start()
    {
        DisplayParagraphText();
    }

    public void DisplayParagraphText()
    {
        UnpackDescription();    // get paragraph descriptions & display
        UnpackExits();          // get exit descriptions 
        ShowButtons();          // display at buttons
    }

    public void UnpackDescription()
    {
        string combinedText = _currentParagraph._description + "\n";
        _displayText.text = combinedText;
    }

    public void UnpackExits()
    {
        _buttonNum = _currentParagraph._exits.Length;
        _exitDictionary.Clear();

        for (int i = 0; i < _buttonNum; i++)
        {
            // set button text
            _buttonText[i].text = _currentParagraph._exits[i]._exitDescription;

            // set _exitDictionary
            _exitDictionary.Add(_currentParagraph._exits[i]._exitDescription, _currentParagraph._exits[i]._moveToParagraph);
        }
    }

    public void ShowButtons()
    {
        if (_buttonNum == 1)
        {
            _buttonText[1].text = _buttonText[0].text;
            _buttons[0].SetActive(false);
            _buttons[1].SetActive(true);
        }
        else if (_buttonNum == 2)
        {
            _buttons[0].SetActive(true);
            _buttons[1].SetActive(true);
        }
        else
        {
            _buttons[0].SetActive(false);
            _buttons[1].SetActive(false);
        }
    }

    public void ChangeParagraph(TextMeshProUGUI keyTMPro)
    {
        // compare the dictionary, and move to designated paragraph
        if (_exitDictionary.ContainsKey(keyTMPro.text))
        {
            _currentParagraph = _exitDictionary[keyTMPro.text];
            DisplayParagraphText();
        }
    }
}