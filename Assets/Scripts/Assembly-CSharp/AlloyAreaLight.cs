using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
[AddComponentMenu("Alloy/Area Light")]
public class AlloyAreaLight : MonoBehaviour
{
	private const float c_minimumLightSize = 1E-05f;

	[SerializeField]
	private Color m_color = new Color(1f, 1f, 1f, 0f);

	[SerializeField]
	private float m_intensity = 1f;

	[FormerlySerializedAs("m_size")]
	[SerializeField]
	private float m_radius;

	[SerializeField]
	private float m_length;

	[SerializeField]
	private bool m_hasSpecularHightlight = true;

	[SerializeField]
	private bool m_isAnimatedByClip;

	private Light m_light;

	private float m_lastRange;

	[HideInInspector]
	public Texture2D DefaultSpotLightCookie;

	private Light Light
	{
		get
		{
			if (m_light == null)
			{
				m_light = GetComponent<Light>();
			}
			return m_light;
		}
	}

	public float Radius
	{
		get
		{
			return m_radius;
		}
		set
		{
			if (m_radius != value)
			{
				m_radius = value;
				UpdateBinding();
			}
		}
	}

	public float Length
	{
		get
		{
			return m_length;
		}
		set
		{
			if (m_length != value)
			{
				m_length = value;
				UpdateBinding();
			}
		}
	}

	public float Intensity
	{
		get
		{
			return m_intensity;
		}
		set
		{
			if (m_intensity != value)
			{
				m_intensity = value;
				UpdateBinding();
			}
		}
	}

	public Color Color
	{
		get
		{
			return m_color;
		}
		set
		{
			if (m_color != value)
			{
				m_color = value;
				UpdateBinding();
			}
		}
	}

	public bool HasSpecularHighlight
	{
		get
		{
			return m_hasSpecularHightlight;
		}
		set
		{
			if (m_hasSpecularHightlight != value)
			{
				m_hasSpecularHightlight = value;
				UpdateBinding();
			}
		}
	}

	public bool IsAnimatedByClip
	{
		get
		{
			return m_isAnimatedByClip;
		}
		set
		{
			m_isAnimatedByClip = value;
		}
	}

	public void UpdateBinding()
	{
		Light light = Light;
		float range = light.range;
		float num = range * 2f;
		m_length = Mathf.Clamp(m_length, 0f, num);
		m_intensity = Mathf.Max(m_intensity, 0f);
		Color color = light.color;
		color.r = m_color.r;
		color.g = m_color.g;
		color.b = m_color.b;
		color *= m_intensity;
		if (light.type == LightType.Directional)
		{
			m_radius = Mathf.Clamp(m_radius, 0f, 1f);
			color.a = m_radius * 10f;
		}
		else
		{
			m_radius = Mathf.Clamp(m_radius, 0f, range);
			float a = m_radius / range;
			float num2 = m_length / num;
			color.a = Mathf.Ceil(num2 * 1000f) + Mathf.Min(a, 0.999f);
		}
		color.a = Mathf.Max(1E-05f, color.a);
		color.a *= ((!m_hasSpecularHightlight) ? (-1f) : 1f);
		light.color = color;
		light.intensity = 1f;
		m_lastRange = range;
	}

	private void Reset()
	{
		Light component = GetComponent<Light>();
		if (component != null)
		{
			m_intensity = Mathf.LinearToGammaSpace(component.intensity);
			m_color.r = component.color.r;
			m_color.g = component.color.g;
			m_color.b = component.color.b;
		}
		else
		{
			m_color.r = 1f;
			m_color.g = 1f;
			m_color.b = 1f;
			m_intensity = 1f;
		}
		m_hasSpecularHightlight = true;
		m_color.a = 1E-05f;
		m_radius = 0f;
		UpdateBinding();
	}

	private void Update()
	{
		if (m_isAnimatedByClip || Light.range != m_lastRange)
		{
			UpdateBinding();
		}
	}
}
