using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuideDriveUI : MonoBehaviour
{
	[SerializeField]
	private string num_format = "000.00";

	[SerializeField]
	private float showMul_pos = 1f;

	[SerializeField]
	private float showMul_rot = 1f;

	[SerializeField]
	private float showMul_scl = 1f;

	[SerializeField]
	private float dragMul_pos = 1f;

	[SerializeField]
	private float dragMul_rot = 360f;

	[SerializeField]
	private float dragMul_scl = 1f;

	[SerializeField]
	private InputField[] inputPos = new InputField[3];

	[SerializeField]
	private InputField[] inputRot = new InputField[3];

	[SerializeField]
	private InputField[] inputScl = new InputField[3];

	[SerializeField]
	private DragInputUI[] dragPos = new DragInputUI[3];

	[SerializeField]
	private DragInputUI[] dragRot = new DragInputUI[3];

	[SerializeField]
	private DragInputUI[] dragScl = new DragInputUI[3];

	[SerializeField]
	private Toggle modeMove;

	[SerializeField]
	private Toggle modeRotate;

	[SerializeField]
	private Toggle modeScaling;

	private bool invoke = true;

	private Vector3 val_pos;

	private Vector3 val_eul;

	private Vector3 val_scl;

	private bool isLimitPos;

	private Vector3 limitPosMin;

	private Vector3 limitPosMax;

	private bool isLimitScl;

	private Vector3 limitSclMin;

	private Vector3 limitSclMax;

	private UnityAction<Vector3, Vector3, Vector3> onMoveAct;

	private void Awake()
	{
		dragPos[0].AddListener(OnDrag_PosX);
		dragPos[1].AddListener(OnDrag_PosY);
		dragPos[2].AddListener(OnDrag_PosZ);
		dragRot[0].AddListener(OnDrag_RotX);
		dragRot[1].AddListener(OnDrag_RotY);
		dragRot[2].AddListener(OnDrag_RotZ);
		dragScl[0].AddListener(OnDrag_SclX);
		dragScl[1].AddListener(OnDrag_SclY);
		dragScl[2].AddListener(OnDrag_SclZ);
		inputPos[0].onEndEdit.AddListener(OnInputField_PosX);
		inputPos[1].onEndEdit.AddListener(OnInputField_PosY);
		inputPos[2].onEndEdit.AddListener(OnInputField_PosZ);
		inputRot[0].onEndEdit.AddListener(OnInputField_RotX);
		inputRot[1].onEndEdit.AddListener(OnInputField_RotY);
		inputRot[2].onEndEdit.AddListener(OnInputField_RotZ);
		inputScl[0].onEndEdit.AddListener(OnInputField_SclX);
		inputScl[1].onEndEdit.AddListener(OnInputField_SclY);
		inputScl[2].onEndEdit.AddListener(OnInputField_SclZ);
	}

	public void Setup(Vector3 pos, Vector3 eul, Vector3 scl, UnityAction<Vector3, Vector3, Vector3> onMoveAct)
	{
		val_pos = pos;
		val_eul = eul;
		val_scl = scl;
		this.onMoveAct = onMoveAct;
		ToText();
	}

	public void SetLimit(bool isLimitPos, Vector3 limitPosMin, Vector3 limitPosMax, bool isLimitScl, Vector3 limitSclMin, Vector3 limitSclMax)
	{
		this.isLimitPos = isLimitPos;
		this.limitPosMin = limitPosMin;
		this.limitPosMax = limitPosMax;
		this.isLimitScl = isLimitScl;
		this.limitSclMin = limitSclMin;
		this.limitSclMax = limitSclMax;
	}

	public void SetUnlimit()
	{
		isLimitPos = false;
		isLimitScl = false;
	}

	private void ToText()
	{
		inputPos[0].text = (val_pos.x * showMul_pos).ToString(num_format);
		inputPos[1].text = (val_pos.y * showMul_pos).ToString(num_format);
		inputPos[2].text = (val_pos.z * showMul_pos).ToString(num_format);
		inputRot[0].text = (val_eul.x * showMul_rot).ToString(num_format);
		inputRot[1].text = (val_eul.y * showMul_rot).ToString(num_format);
		inputRot[2].text = (val_eul.z * showMul_rot).ToString(num_format);
		inputScl[0].text = (val_scl.x * showMul_scl).ToString(num_format);
		inputScl[1].text = (val_scl.y * showMul_scl).ToString(num_format);
		inputScl[2].text = (val_scl.z * showMul_scl).ToString(num_format);
	}

	private void OnInputField_PosX(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_pos.x = StringToFloat(str) * (1f / showMul_pos);
			Move();
			inputPos[0].text = (val_pos.x * showMul_pos).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_PosY(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_pos.y = StringToFloat(str) * (1f / showMul_pos);
			Move();
			inputPos[1].text = (val_pos.y * showMul_pos).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_PosZ(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_pos.z = StringToFloat(str) * (1f / showMul_pos);
			Move();
			inputPos[2].text = (val_pos.z * showMul_pos).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_RotX(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_eul.x = StringToFloat(str) * (1f / showMul_rot);
			Move();
			inputRot[0].text = (val_eul.x * showMul_rot).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_RotY(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_eul.y = StringToFloat(str) * (1f / showMul_rot);
			Move();
			inputRot[1].text = (val_eul.y * showMul_rot).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_RotZ(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_eul.z = StringToFloat(str) * (1f / showMul_rot);
			Move();
			inputRot[2].text = (val_eul.z * showMul_rot).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_SclX(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_scl.x = StringToFloat(str) * (1f / showMul_scl);
			Move();
			inputScl[0].text = (val_scl.x * showMul_scl).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_SclY(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_scl.y = StringToFloat(str) * (1f / showMul_scl);
			Move();
			inputScl[1].text = (val_scl.y * showMul_scl).ToString(num_format);
			invoke = true;
		}
	}

	private void OnInputField_SclZ(string str)
	{
		if (invoke)
		{
			invoke = false;
			val_scl.z = StringToFloat(str) * (1f / showMul_scl);
			Move();
			inputScl[2].text = (val_scl.z * showMul_scl).ToString(num_format);
			invoke = true;
		}
	}

	private void Move()
	{
		Limit();
		if (onMoveAct != null)
		{
			onMoveAct(val_pos, val_eul, val_scl);
		}
	}

	private void Limit()
	{
		if (isLimitPos)
		{
			val_pos.x = Mathf.Clamp(val_pos.x, limitPosMin.x, limitPosMax.x);
			val_pos.y = Mathf.Clamp(val_pos.y, limitPosMin.y, limitPosMax.y);
			val_pos.z = Mathf.Clamp(val_pos.z, limitPosMin.z, limitPosMax.z);
		}
		if (isLimitScl)
		{
			val_scl.x = Mathf.Clamp(val_scl.x, limitSclMin.x, limitSclMax.x);
			val_scl.y = Mathf.Clamp(val_scl.y, limitSclMin.y, limitSclMax.y);
			val_scl.z = Mathf.Clamp(val_scl.z, limitSclMin.z, limitSclMax.z);
		}
	}

	public void ResetButton()
	{
		invoke = false;
		val_pos = Vector3.zero;
		val_eul = Vector3.zero;
		val_scl = Vector3.one;
		Move();
		ToText();
		SystemSE.Play(SystemSE.SE.CHOICE);
		invoke = true;
	}

	private static float StringToFloat(string str)
	{

		try
		{
			return float.Parse(str);
		}
		catch
		{
			return 0f;
		}
	}

	private void OnDrag_PosX(Vector2 move)
	{
		DragMove(new Vector3(DragToMoveFloat(move, dragMul_pos), 0f, 0f), Vector3.zero, Vector3.zero);
	}

	private void OnDrag_PosY(Vector2 move)
	{
		DragMove(new Vector3(0f, DragToMoveFloat(move, dragMul_pos), 0f), Vector3.zero, Vector3.zero);
	}

	private void OnDrag_PosZ(Vector2 move)
	{
		DragMove(new Vector3(0f, 0f, DragToMoveFloat(move, dragMul_pos)), Vector3.zero, Vector3.zero);
	}

	private void OnDrag_RotX(Vector2 move)
	{
		DragMove(Vector3.zero, new Vector3(DragToMoveFloat(move, dragMul_rot), 0f, 0f), Vector3.zero);
	}

	private void OnDrag_RotY(Vector2 move)
	{
		DragMove(Vector3.zero, new Vector3(0f, DragToMoveFloat(move, dragMul_rot), 0f), Vector3.zero);
	}

	private void OnDrag_RotZ(Vector2 move)
	{
		DragMove(Vector3.zero, new Vector3(0f, 0f, DragToMoveFloat(move, dragMul_rot)), Vector3.zero);
	}

	private void OnDrag_SclX(Vector2 move)
	{
		DragMove(Vector3.zero, Vector3.zero, new Vector3(DragToMoveFloat(move, dragMul_scl), 0f, 0f));
	}

	private void OnDrag_SclY(Vector2 move)
	{
		DragMove(Vector3.zero, Vector3.zero, new Vector3(0f, DragToMoveFloat(move, dragMul_scl), 0f));
	}

	private void OnDrag_SclZ(Vector2 move)
	{
		DragMove(Vector3.zero, Vector3.zero, new Vector3(0f, 0f, DragToMoveFloat(move, dragMul_scl)));
	}

	private float DragToMoveFloat(Vector2 move, float mul)
	{
		float num = move.x / (float)Screen.width;
		float num2 = move.y / (float)Screen.height;
		return (num + num2) * mul;
	}

	private void DragMove(Vector3 movePos, Vector3 moveEul, Vector3 moveScl)
	{
		invoke = false;
		val_pos += movePos;
		val_eul += moveEul;
		val_scl += moveScl;
		Move();
		ToText();
		invoke = true;
	}
}
