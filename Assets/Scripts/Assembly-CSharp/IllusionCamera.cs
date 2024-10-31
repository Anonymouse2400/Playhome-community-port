using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

public class IllusionCamera : MonoBehaviour
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

	private Camera camera;

	[SerializeField]
	private float mouseSensitivity = 1f;

	[SerializeField]
	private float keySensitivity = 1f;

	[SerializeField]
	private float rotateSpeed = 10f;

	[SerializeField]
	private float translateSpeed = 0.25f;

	[SerializeField]
	private float keyAccel = 5f;

	[SerializeField]
	private float keyBrake = 5f;

	[SerializeField]
	private Vector3 setFocus = new Vector3(0f, 0f, 0f);

	private Vector3 nowFocus = new Vector3(0f, 0f, 0f);

	private Vector3 prevFocus = new Vector3(0f, 0f, 0f);

	private float focusIpoRate = 1f;

	[SerializeField]
	private Vector3 rotate = new Vector3(0f, 0f, 0f);

	[SerializeField]
	private float distance = 1.5f;

	[SerializeField]
	private float defParse = 45f;

	[SerializeField]
	private float minDistance;

	[SerializeField]
	private float maxDistance = 10f;

	[SerializeField]
	private bool mouseRevY;

	[SerializeField]
	private bool mouseRevX;

	[SerializeField]
	private bool keyRevY;

	[SerializeField]
	private bool keyRevX;

	[SerializeField]
	private CameraTarget target;

	[SerializeField]
	private bool showTgt = true;

	[SerializeField]
	private bool dragLock = true;

	[SerializeField]
	private CenterDragMove centerDragMove = CenterDragMove.XZ;

	private Vector2 nowCenterOffset = Vector2.zero;

	public Vector2 setCenterOffset = Vector2.zero;

	public float distanceOffset;

	private Vector3 keyRotSpeed = new Vector3(0f, 0f, 0f);

	private Vector3 keyMovSpeed = new Vector3(0f, 0f, 0f);

	private float keyDisSpeed;

	private float keyFovSpeed;

	private CapsuleCollider viewCollider;

	private ButtonState[] buttonState = new ButtonState[3];

	private bool dragging;

	public float areaLimit = 10f;

	public Vector3? areaCenter;

	public float hitRadius = 0.1f;

	public LayerMask hitLayer = 0;

	public float pitchLimit = 80f;

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
			return nowFocus;
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
			return camera.fieldOfView;
		}
	}

	public CameraTarget Target
	{
		get
		{
			return target;
		}
	}

	public bool OnUI { get; private set; }

	public bool Lock { get; set; }

	private void Awake()
	{
		camera = GetComponent<Camera>();
		viewCollider = base.gameObject.AddComponent<CapsuleCollider>();
		viewCollider.radius = 0.05f;
		viewCollider.isTrigger = true;
		viewCollider.direction = 2;
		viewCollider.gameObject.layer = base.gameObject.layer;
		buttonState[0] = ButtonState.NO;
		buttonState[2] = ButtonState.NO;
		buttonState[1] = ButtonState.NO;
	}

	private void Start()
	{
		defParse = ConfigData.defParse;
		camera.fieldOfView = defParse;
		if ((bool)target)
		{
			target.Move(setFocus, showTgt);
		}
	}

	private void LateUpdate()
	{
		if (camera == null)
		{
			return;
		}
		showTgt = ConfigData.showFocusUI;
		mouseSensitivity = ConfigData.mouseSensitive;
		mouseRevY = ConfigData.mouseRevV;
		mouseRevX = ConfigData.mouseRevH;
		centerDragMove = ConfigData.centerDragMove;
		dragLock = ConfigData.dragLock;
		keySensitivity = ConfigData.keySensitive * 0.5f;
		keyRevY = ConfigData.keyRevV;
		keyRevX = ConfigData.keyRevH;
		defParse = ConfigData.defParse;
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
		
		OnUI = (bool)EventSystem.current && EventSystem.current.IsPointerOverGameObject();
		ButtonStateCheck();
		if (!OnUI && !Lock && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
		{
			dragging = true;
		}
		else if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
		{
			dragging = false;
		}
		if (dragging)
		{
			if (dragLock)
			{
				GameCursor.Lock();
			}
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
				distance += num * translateSpeed * ConfigData.cameraMoveSpeed;
				Translate(new Vector3(0f, num2, 0f), false);
			}
		}
		flag |= KeyInput();
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		nowCenterOffset.x = Tween.Spring(nowCenterOffset.x, setCenterOffset.x, 5f, Time.deltaTime, 0.01f);
		nowCenterOffset.y = Tween.Spring(nowCenterOffset.y, setCenterOffset.y, 5f, Time.deltaTime, 0.01f);
		if (focusIpoRate < 1f)
		{
			focusIpoRate = Tween.Spring(focusIpoRate, 1f, 8f, Time.deltaTime, 0.1f);
			nowFocus = Vector3.Lerp(prevFocus, setFocus, focusIpoRate);
		}
		else
		{
			nowFocus = setFocus;
		}
		base.transform.position = CalcPos();
		viewCollider.height = distance;
		viewCollider.center = Vector3.forward * distance * 0.5f;
		if ((bool)target && flag)
		{
			target.Move(nowFocus, showTgt);
		}
		HitCheck();
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
		nowFocus = focus;
		setFocus = focus;
		focusIpoRate = 1f;
		this.rotate = rotate;
		this.distance = distance;
		if (parse > 0f)
		{
			camera.fieldOfView = parse;
		}
		if ((bool)target)
		{
			target.Move(focus, showTgt);
		}
		areaCenter = focus;
	}

	public void SetFocus(Vector3 focus, bool ipo = false)
	{
		setFocus = focus;
		if (ipo)
		{
			prevFocus = nowFocus;
			focusIpoRate = 0f;
		}
		else
		{
			nowFocus = focus;
			focusIpoRate = 1f;
		}
		if ((bool)target)
		{
			target.Move(focus, showTgt);
		}
	}

	public void SetDistance(float distance)
	{
		this.distance = distance;
	}

	public void SetParse(float parse, bool changeDistance)
	{
		float num = distance * camera.fieldOfView;
		camera.fieldOfView = parse;
		if (changeDistance)
		{
			distance = num / camera.fieldOfView;
		}
	}

	private void Translate(Vector3 vec, bool localTranslate)
	{
		Vector3 vector = translateSpeed * ConfigData.cameraMoveSpeed * vec;
		if (localTranslate)
		{
			Quaternion quaternion = Quaternion.Euler(rotate);
			setFocus += quaternion * vector;
		}
		else
		{
			Quaternion quaternion2 = Quaternion.Euler(0f, rotate.y, 0f);
			setFocus += quaternion2 * vector;
		}
		if (areaCenter.HasValue)
		{
			Vector3 vector2 = setFocus - areaCenter.Value;
			if (vector2.magnitude >= areaLimit)
			{
				vector2.Normalize();
				setFocus = areaCenter.Value + vector2 * areaLimit;
			}
		}
	}

	private void Rotate(Vector3 angles)
	{
		rotate += angles * rotateSpeed * ConfigData.cameraTurnSpeed;
		float value = Mathf.DeltaAngle(0f, rotate.x);
		if (pitchLimit >= 0f)
		{
			rotate.x = Mathf.Clamp(value, 0f - pitchLimit, pitchLimit);
		}
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
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			return false;
		}
		bool flag = false;
		if (Input.GetKey(KeyCode.KeypadPlus))
		{
			keySensitivity += Time.deltaTime * 0.25f;
		}
		else if (Input.GetKey(KeyCode.KeypadMinus))
		{
			keySensitivity = Mathf.Max(keySensitivity - Time.deltaTime * 0.25f, 0f);
		}
		KeyCode addKey = (keyRevX ? KeyCode.Keypad4 : KeyCode.Keypad6);
		KeyCode subKey = (keyRevX ? KeyCode.Keypad6 : KeyCode.Keypad4);
		KeyCode addKey2 = (keyRevY ? KeyCode.Keypad8 : KeyCode.Keypad2);
		KeyCode subKey2 = (keyRevY ? KeyCode.Keypad2 : KeyCode.Keypad8);
		flag |= KeyInput_Value(ref keyRotSpeed.y, keySensitivity, addKey, subKey);
		flag |= KeyInput_Value(ref keyRotSpeed.x, keySensitivity, addKey2, subKey2);
		if (Input.GetKey(KeyCode.Keypad5))
		{
			rotate.x = 0f;
			keyRotSpeed.x = 0f;
			flag = true;
		}
		flag |= KeyInput_Value(ref keyMovSpeed.x, keySensitivity, KeyCode.RightArrow, KeyCode.LeftArrow);
		flag |= KeyInput_Value(ref keyMovSpeed.z, keySensitivity, KeyCode.UpArrow, KeyCode.DownArrow);
		flag |= KeyInput_Value(ref keyMovSpeed.y, keySensitivity, KeyCode.PageUp, KeyCode.PageDown);
		flag |= KeyInput_Value(ref keyDisSpeed, keySensitivity * 0.2f, KeyCode.End, KeyCode.Home);
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
		float num = keySensitivity * 0.2f;
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			num *= 3f;
		}
		flag |= KeyInput_Value(ref keyFovSpeed, num, KeyCode.RightBracket, KeyCode.Equals);
		if (Input.GetKey(KeyCode.Semicolon))
		{
			camera.fieldOfView = defParse;
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
			camera.fieldOfView += keyFovSpeed * 2f;
			camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 5f, 90f);
		}
		if (keyDisSpeed != 0f)
		{
			distance += keyDisSpeed;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ConfigData.showFocusUI = !ConfigData.showFocusUI;
			SystemSE.SE se = (ConfigData.showFocusUI ? SystemSE.SE.YES : SystemSE.SE.NO);
			SystemSE.Play(se);
			Config config = UnityEngine.Object.FindObjectOfType<Config>();
			if ((bool)config)
			{
				config.UpdateFromShortCut();
			}
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

	private void HitCheck()
	{
		int num = 0;
		Vector3 position = base.transform.position;
		Vector3 vector = rotate;
		while (Physics.CheckSphere(position, hitRadius, hitLayer))
		{
			distance += hitRadius * 0.1f;
			position = CalcPos();
			num++;
			if (num == 10)
			{
				distance += hitRadius;
				break;
			}
		}
		base.transform.position = position;
	}

	private Vector3 CalcPos()
	{
		Vector3 screen = Vector2.zero;
		screen.x = (float)camera.pixelWidth * (nowCenterOffset.x * 0.5f + 0.5f);
		screen.y = (float)camera.pixelHeight * (nowCenterOffset.y * 0.5f + 0.5f);
		Vector3 vector = Unproj(screen);
		vector *= 0f - distance;
		vector.z = 0f;
		Quaternion quaternion = Quaternion.Euler(rotate);
		base.transform.rotation = quaternion;
		Vector3 vector2 = quaternion * vector;
		Vector3 vector3 = quaternion * (Vector3.back * distance);
		return nowFocus + vector2 + vector3;
	}

	private Vector3 Unproj(Vector3 screen)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity = Matrix4x4.Inverse(identity);
		Matrix4x4 identity2 = Matrix4x4.identity;
		identity2.m00 = camera.pixelWidth / 2;
		identity2.m30 = camera.pixelWidth / 2;
		identity2.m11 = -camera.pixelHeight / 2;
		identity2.m31 = camera.pixelHeight / 2;
		identity2.m22 = 1f;
		identity2.m33 = 1f;
		Matrix4x4 matrix4x = Matrix4x4.Inverse(identity2);
		Matrix4x4 matrix4x2 = Matrix4x4.Inverse(camera.projectionMatrix);
		Matrix4x4 matrix4x3 = Matrix4x4.Inverse(identity);
		Matrix4x4 identity3 = Matrix4x4.identity;
		identity3.m00 = (identity3.m10 = (identity3.m20 = (identity3.m30 = screen.x)));
		identity3.m01 = (identity3.m11 = (identity3.m21 = (identity3.m31 = screen.y)));
		identity3.m02 = (identity3.m12 = (identity3.m22 = (identity3.m32 = screen.z)));
		identity3.m03 = (identity3.m13 = (identity3.m23 = (identity3.m33 = 1f)));
		Matrix4x4 matrix4x4 = identity3 * matrix4x * matrix4x2 * matrix4x3;
		return new Vector3(matrix4x4.m00, matrix4x4.m01, matrix4x4.m02);
	}

	public void SetDefParse(float defParse)
	{
		this.defParse = defParse;
	}
}
