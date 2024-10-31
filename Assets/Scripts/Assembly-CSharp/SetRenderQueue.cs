using System;
using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]
public class SetRenderQueue : MonoBehaviour
{
	[SerializeField]
	protected int[] m_queues = new int[1] { 3000 };

	private Renderer rend;

	protected void Awake()
	{
		rend = GetComponent<Renderer>();
		Set();
	}

	private void OnValidate()
	{
		Set();
	}

	public void Set()
	{
		if (Application.isPlaying && rend != null)
		{
			for (int i = 0; i < rend.materials.Length && i < m_queues.Length; i++)
			{
				rend.materials[i].renderQueue = m_queues[i];
			}
		}
	}

	public void Set(int[] queues)
	{
		m_queues = new int[queues.Length];
		queues.CopyTo(m_queues, 0);
		if (Application.isPlaying && rend != null)
		{
			for (int i = 0; i < rend.materials.Length && i < m_queues.Length; i++)
			{
				rend.materials[i].renderQueue = m_queues[i];
			}
		}
	}

	public int[] Get()
	{
		return m_queues;
	}
}
