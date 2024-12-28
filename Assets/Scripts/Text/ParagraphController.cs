using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controll the paragraphs
/// </summary>

public class ParagraphController : MonoBehaviour
{
    public TextMeshProUGUI _displayText;
    public TextMeshProUGUI[] _buttonText;
    public Paragraph _currentParagraph;
    private Dictionary<string, Paragraph> _exitDictionary = new Dictionary<string, Paragraph>();
    // [HideInInspector] public List<string> _exitsDescriptionInParagraph = new List<string>();

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
        UnpackExits();          // get exit descriptions & display at buttons
    }

    public void UnpackDescription()
    {
        string combinedText = _currentParagraph._description + "\n";
        _displayText.text = combinedText;
    }

    public void UnpackExits()
    {
        if (_currentParagraph._exits.Length == 1)
        {
            _buttonText[1].text = _currentParagraph._exits[0]._exitDescription;
        }
        else if (_currentParagraph._exits.Length == 2)
        {
            _buttonText[0].text = _currentParagraph._exits[0]._exitDescription;
            _buttonText[1].text = _currentParagraph._exits[1]._exitDescription;
        }

        for (int i = 0; i < _currentParagraph._exits.Length; i++)
        {
            _exitDictionary.Add(_currentParagraph._exits[i]._exitDescription, _currentParagraph._exits[i]._moveToParagraph);
        }
    }

    public void ClickButton()
    {
        // change paragraph
        if (_currentParagraph._exits.Length == 1)
        {
            _currentParagraph = _currentParagraph._exits[0]._moveToParagraph;
        }
        else if (_currentParagraph._exits.Length == 2)
        {
            _currentParagraph = _currentParagraph._exits[0]._moveToParagraph;

        }
        // _currentParagraph = _exitDictionary[directionNoun];
    }
}