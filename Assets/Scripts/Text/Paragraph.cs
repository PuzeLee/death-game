using UnityEngine;

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
}
