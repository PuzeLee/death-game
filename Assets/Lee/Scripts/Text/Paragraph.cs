using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Paragraph Content Def
/// </summary>

[CreateAssetMenu(menuName = "DeathGame/Paragraph")]

public class Paragraph : ScriptableObject
{
	[TextArea]
	public string _description;
	public string _paragraphName;
	public Exit[] _exits;
	public Sprite _speakerAvatar;
	public string _speakerName;
}
