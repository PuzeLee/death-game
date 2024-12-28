using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
	[SerializeField]
	Collider2D thisCollider;

	[SerializeField]
	Collider2D[] colliderToIgnore;

	private void Start()
	{
		foreach (Collider2D otherCollider in colliderToIgnore)
		{
			Physics2D.IgnoreCollision(thisCollider, otherCollider, true);
		}
	}
}
