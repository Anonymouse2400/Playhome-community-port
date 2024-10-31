using UnityEngine;

public class HairSetClipboardCopy : MonoBehaviour
{
	public Female fem;

	private void Awake()
	{
		fem = GetComponent<Female>();
	}
}
