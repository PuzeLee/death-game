using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParagraphController : MonoBehaviour
{
	public TextMeshProUGUI _displayText;
	[HideInInspector] public ParagraphNavigation _paragraphNavigation;
	[HideInInspector] public List<string> _interactionDescriptionInParagraph = new List<string>();

	private List<string> _actionLog = new List<string>();

	private void Awake()
	{
		_paragraphNavigation = GetComponent<ParagraphNavigation>();
	}

	private void Start()
	{
		DisplayParagraphText();
		DisplayLoggedText();
	}

	public void DisplayLoggedText()
	{
		string logAsText = string.Join("\n", _actionLog.ToArray());
		_displayText.text = logAsText;
	}

	public void DisplayParagraphText()
	{
		UnpackParagraph();

		string joinedInteractionDescriptions = string.Join("\n", _interactionDescriptionInParagraph.ToArray());

		string combinedText = _paragraphNavigation._currentParagraph._description + "\n" + joinedInteractionDescriptions;
		LogStringWithReturn(combinedText);
	}

	private void UnpackParagraph()
	{
		_paragraphNavigation.UnpackExitsInParagraph();
	}

	public void LogStringWithReturn(string stringToAdd)
	{
		_actionLog.Add(stringToAdd + "\n");
	}
}
