using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemoCamera : MonoBehaviour
{
	private enum MouseButton
	{
		MB_LEFT = 0,
		MB_RIGHT = 1,
		MB_MIDDLE = 2,
		NUM = 3
	}

	private enum ButtonState
	{
		NO = 0,
		DOWN = 1,
		HOLD = 2,
		UP = 3
	}

	public enum CenterDragMove
	{
		XY = 0,
		XZ = 1
	}

	[SerializeField]
	private float mouseSensitivity = 3f;

	[SerializeField]
	private float keySensitivity = 3f;

	[SerializeField]
	private float rotateSpeed = 1f;

	[SerializeField]
	private float translateSpeed = 1f;

	[SerializeField]
	private float keyAccel = 5f;

	[SerializeField]
	private float keyBrake = 5f;

	[SerializeField]
	private Vector3 focus = new Vector3(0f, 0f, 0f);

	[SerializeField]
	private Vector3 rotate = new Vector3(0f, 0f, 0f);

	[SerializeField]
	private float distance = 1.5f;

	[SerializeField]
	private float defParse = 45f;

	[SerializeField]
	private bool mouseRevY;

	[SerializeField]
	private bool mouseRevX;

	[SerializeField]
	private bool keyRevY;

	[SerializeField]
	private bool keyRevX;

	[SerializeField]
	private Transform target;

	[SerializeField]
	private bool showTgt = true;

	[SerializeField]
	private bool dragLock = true;

	[SerializeField]
	private CenterDragMove centerDragMove;

	private Vector3 nowCenterOffset = Vector3.zero;

	public Vector3 setCenterOffset = Vector3.zero;

	[SerializeField]
	private float centerDisOffset;

	private Vector3 keyRotSpeed = new Vector3(0f, 0f, 0f);

	private Vector3 keyMovSpeed = new Vector3(0f, 0f, 0f);

	private float keyDisSpeed;

	private float keyFovSpeed;

	private CapsuleCollider viewCollider;

	private ButtonState[] buttonState = new ButtonState[3];

	private bool dragging;

	public float Distance
	{
		get
		{
			return distance;
		}
	}

	public Vector3 Focus
	{
		get
		{
			return focus;
		}
	}

	public Vector3 Rotation
	{
		get
		{
			return rotate;
		}
	}

	public float FOV
	{
		get
		{
			return GetComponent<Camera>().fieldOfView;
		}
	}

	private void Awake()
	{
		viewCollider = base.gameObject.AddComponent<CapsuleCollider>();
		viewCollider.radius = 0.05f;
		viewCollider.isTrigger = true;
		viewCollider.direction = 2;
		buttonState[0] = ButtonState.NO;
		buttonState[2] = ButtonState.NO;
		buttonState[1] = ButtonState.NO;
	}

	private void Start()
	{
	}

	private void LateUpdate()
	{
		if (GetComponent<Camera>() == null)
		{
			return;
		}
		float num = Input.GetAxis("Mouse X") * mouseSensitivity;
		float num2 = Input.GetAxis("Mouse Y") * mouseSensitivity;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			num *= 3f;
			num2 *= 3f;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			num *= 0.25f;
			num2 *= 0.25f;
		}
		bool flag = false;

		bool flag3 = (bool)EventSystem.current && EventSystem.current.IsPointerOverGameObject();
		ButtonStateCheck();
		if (!flag3 && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
		{
			dragging = true;
		}
		else if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
		{
			dragging = false;
		}
		if (dragging)
		{
			if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
			{
				flag = true;
				if (centerDragMove == CenterDragMove.XY)
				{
					Translate(new Vector3(num, 0f, num2), true);
				}
				else
				{
					Translate(new Vector3(num, num2, 0f), true);
				}
			}
			else if (Input.GetMouseButton(0))
			{
				if (mouseRevX)
				{
					num *= -1f;
				}
				if (mouseRevY)
				{
					num2 *= -1f;
				}
				flag = true;
				Rotate(new Vector3(0f - num2, num, 0f));
			}
			else if (Input.GetMouseButton(2))
			{
				flag = true;
				if (centerDragMove == CenterDragMove.XY)
				{
					Translate(new Vector3(num, num2, 0f), true);
				}
				else
				{
					Translate(new Vector3(num, 0f, num2), false);
				}
			}
			else if (Input.GetMouseButton(1))
			{
				flag = true;
				distance += num * translateSpeed;
				distance = Mathf.Clamp(distance, 0.01f, 100f);
				Translate(new Vector3(0f, num2, 0f), false);
			}
			if (!dragLock)
			{
			}
		}
		flag |= KeyInput();
		Vector3 vector = nowCenterOffset;
		if (centerDisOffset != 0f)
		{
			vector.x *= centerDisOffset * distance;
			vector.y *= centerDisOffset * distance;
			vector.z *= centerDisOffset * distance;
		}
		Quaternion quaternion = Quaternion.Euler(rotate);
		base.transform.rotation = quaternion;
		Vector3 vector2 = quaternion * vector;
		Vector3 vector3 = quaternion * (Vector3.back * distance);
		base.transform.position = focus + vector2 + vector3;
		viewCollider.height = distance;
		viewCollider.center = Vector3.forward * distance * 0.5f;
		if (showTgt && (bool)target && !flag)
		{
		}
	}

	private void OnGUI()
	{
	}

	private void ButtonStateCheck()
	{
		for (int i = 0; i < 3; i++)
		{
			if (Input.GetMouseButtonDown(i))
			{
				buttonState[i] = ButtonState.DOWN;
			}
			else if (Input.GetMouseButtonUp(i))
			{
				buttonState[i] = ButtonState.UP;
			}
			else if (Input.GetMouseButton(i))
			{
				buttonState[i] = ButtonState.HOLD;
			}
			else
			{
				buttonState[i] = ButtonState.NO;
			}
		}
	}

	public void Set(Vector3 focus, Vector3 rotate, float distance, float parse = 0f)
	{
		this.focus = focus;
		this.rotate = rotate;
		this.distance = distance;
		if (parse > 0f)
		{
			GetComponent<Camera>().fieldOfView = parse;
		}
	}

	public void SetFocus(Vector3 focus)
	{
		this.focus = focus;
	}

	private void Translate(Vector3 vec, bool localTranslate)
	{
		if (localTranslate)
		{
			Quaternion quaternion = Quaternion.Euler(rotate);
			focus += quaternion * (vec * translateSpeed);
		}
		else
		{
			Quaternion quaternion2 = Quaternion.Euler(0f, rotate.y, 0f);
			focus += quaternion2 * (vec * translateSpeed);
		}
	}

	private void Rotate(Vector3 angles)
	{
		rotate += angles * rotateSpeed;
	}

	private bool KeyInput_Value(ref float val, float move, KeyCode addKey, KeyCode subKey)
	{
		if (Input.GetKey(addKey))
		{
			KeyInput_Accel(ref val, move);
			return true;
		}
		if (Input.GetKey(subKey))
		{
			KeyInput_Accel(ref val, 0f - move);
			return true;
		}
		KeyInput_Brake(ref val, move);
		return false;
	}

	private bool KeyInputDown_Value(ref float val, float move, KeyCode addKey, KeyCode subKey)
	{
		if (Input.GetKeyDown(addKey))
		{
			val += move;
			return true;
		}
		if (Input.GetKeyDown(subKey))
		{
			val -= move;
			return true;
		}
		return false;
	}

	private void KeyInput_Accel(ref float val, float move)
	{
		val += move * keyAccel * Time.deltaTime;
		if (move > 0f)
		{
			val = Mathf.Min(val, move);
		}
		else
		{
			val = Mathf.Max(val, move);
		}
	}

	private void KeyInput_Brake(ref float val, float move)
	{
		float a = Mathf.Abs(val) - move * keyBrake * Time.deltaTime;
		if (val > 0f)
		{
			val = Mathf.Max(a, 0f);
		}
		else
		{
			val = 0f - Mathf.Max(a, 0f);
		}
	}

	private bool KeyInput()
	{
		bool flag = false;
		KeyCode addKey = (keyRevX ? KeyCode.Keypad4 : KeyCode.Keypad6);
		KeyCode subKey = (keyRevX ? KeyCode.Keypad6 : KeyCode.Keypad4);
		KeyCode addKey2 = (keyRevY ? KeyCode.Keypad8 : KeyCode.Keypad2);
		KeyCode subKey2 = (keyRevY ? KeyCode.Keypad2 : KeyCode.Keypad8);
		flag |= KeyInput_Value(ref keyRotSpeed.y, keySensitivity, addKey, subKey);
		flag |= KeyInput_Value(ref keyRotSpeed.x, keySensitivity, addKey2, subKey2);
		flag |= KeyInput_Value(ref keyMovSpeed.x, keySensitivity, KeyCode.RightArrow, KeyCode.LeftArrow);
		flag |= KeyInput_Value(ref keyMovSpeed.z, keySensitivity, KeyCode.UpArrow, KeyCode.DownArrow);
		flag |= KeyInput_Value(ref keyMovSpeed.y, keySensitivity, KeyCode.PageUp, KeyCode.PageDown);
		flag |= KeyInput_Value(ref keyDisSpeed, keySensitivity * 0.02f, KeyCode.End, KeyCode.Home);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			bool flag2 = KeyInputDown_Value(ref rotate.z, 45f, KeyCode.Period, KeyCode.Backslash);
			flag = flag || flag2;
			if (flag2)
			{
				keyRotSpeed.z = 0f;
			}
		}
		else
		{
			flag |= KeyInput_Value(ref keyRotSpeed.z, keySensitivity * 0.5f, KeyCode.Period, KeyCode.Backslash);
		}
		if (Input.GetKey(KeyCode.Slash))
		{
			rotate.z = 0f;
			keyRotSpeed.z = 0f;
			flag = true;
		}
		flag |= KeyInput_Value(ref keyFovSpeed, keySensitivity * 0.2f, KeyCode.RightBracket, KeyCode.Equals);
		if (Input.GetKey(KeyCode.Semicolon))
		{
			GetComponent<Camera>().fieldOfView = defParse;
			keyFovSpeed = 0f;
			flag = true;
		}
		if (keyRotSpeed.magnitude != 0f)
		{
			Rotate(keyRotSpeed);
		}
		if (keyMovSpeed.magnitude != 0f)
		{
			Vector3 vec = new Vector3(keyMovSpeed.x, keyMovSpeed.y, 0f);
			Vector3 vec2 = new Vector3(0f, 0f, keyMovSpeed.z);
			Translate(vec, false);
			Translate(vec2, true);
			flag = true;
		}
		if (keyFovSpeed != 0f)
		{
			GetComponent<Camera>().fieldOfView += keyFovSpeed;
			GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 0.01f, 90f);
		}
		if (keyDisSpeed != 0f)
		{
			distance += keyDisSpeed;
		}
		return flag;
	}

	public Vector3 GetDirection()
	{
		return base.transform.rotation * Vector3.forward;
	}

	public Vector3 GetTargetPos()
	{
		return target.transform.position;
	}
}
