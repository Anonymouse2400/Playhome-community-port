using System;
using UnityEngine;

public class GuideDrive : MonoBehaviour
{
	protected GuideDriveManager manager;

	protected Material material;

	protected Color baseColor;

	protected Color highColor;

	protected Vector3 movePrevPos;

	protected bool onCursor;

	protected bool onMove;

	protected bool setColor;

	public bool OnCursor
	{
		get
		{
			return onCursor;
		}
		set
		{
			if (onCursor != value)
			{
				onCursor = value;
				UpdateColor();
			}
		}
	}

	protected void UpdateColor()
	{
		if (!(material == null))
		{
			if (onCursor)
			{
				material.color = highColor;
			}
			else
			{
				material.color = baseColor;
			}
		}
	}

	protected void Init()
	{
		Renderer renderer = base.gameObject.GetComponent<Renderer>();
		if (renderer == null)
		{
			renderer = base.gameObject.GetComponentInChildren<Renderer>();
		}
		material = renderer.material;
		if (!setColor)
		{
			SetBaseColor(material.color);
		}
		else
		{
			UpdateColor();
		}
		onCursor = false;
		onMove = false;
		manager = base.gameObject.GetComponentInParent<GuideDriveManager>();
	}

	public virtual void OnMoveStart(Vector3 clickPos)
	{
		onMove = true;
		movePrevPos = Input.mousePosition;
		manager.OnMoveStart(this);
	}

	public virtual void OnMoveEnd()
	{
		onMove = false;
		manager.OnMoveEnd(this);
	}

	public void SetBaseColor(Color color)
	{
		setColor = true;
		baseColor = color;
		Color color2 = baseColor;
		color2.r *= 1.5f;
		color2.g *= 1.5f;
		color2.b *= 1.5f;
		SetHighColor(color2);
	}

	public void SetHighColor(Color color)
	{
		highColor = color;
		UpdateColor();
	}
}
