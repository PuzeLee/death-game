using UnityEngine;

/// <summary>
/// Now is which paragraph
/// </summary>

public class ParagraphNavigation : MonoBehaviour
{
    public Paragraph _currentParagraph;
	private ParagraphController _paragraphController;

	private void Awake()
	{
		_paragraphController = GetComponent<ParagraphController>();
	}

	public void UnpackExitsInParagraph()
	{
		for (int i = 0; i < _currentParagraph._exits.Length; i++)
		{
			_paragraphController._interactionDescriptionInParagraph.Add(_currentParagraph._exits[i]._exitDescription);
		}
	}
}
