using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameCursor : Singleton<GameCursor>
{
	private struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;
	}

	public struct POINT
	{
		public int X;

		public int Y;

		public POINT(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	private static Vector3 pos = Vector3.zero;

	private static Vector3 rawPos = Vector3.zero;

	private static bool isNextLock;

	private static bool isNowLock;

	private static bool isPrevLock;

	private static POINT lockPos = default(POINT);

	public Texture2D tex;

	public Vector2 size = new Vector2(32f, 32f);

	public Vector2 hotspot = Vector2.zero;

	private static ShortClick shortClick = new ShortClick();

	private const int MOUSEEVENTF_LEFTDOWN = 2;

	private const int MOUSEEVENTF_LEFTUP = 4;

	//private IntPtr windowPtr = GetForegroundWindow();

	private RECT winRect = default(RECT);

	public static bool IsDraw
	{
		get
		{
			return Cursor.visible;
		}
		set
		{
			Cursor.visible = value;
		}
	}

	public static Vector2 move { get; private set; }

	public static Vector2 pos2D { get; private set; }

	public static Vector2 Pos
	{
		get
		{
			return pos;
		}
	}

	public static Vector2 RawPos
	{
		get
		{
			return rawPos;
		}
	}

	public static bool IsShortClick
	{
		get
		{
			return shortClick.IsShortClickNow;
		}
	}

	public static bool OnUI
	{
		get
		{
			return (bool)EventSystem.current && EventSystem.current.IsPointerOverGameObject();
		}
	}

	private void Start()
	{
		UpdatePos();
		Pos3DtoPos2D();
		//GetCursorPos(out lockPos);
		//windowPtr = GetForegroundWindow();
		move = Vector2.zero;
		IsDraw = true;
		isNextLock = false;
		isNowLock = false;
		isPrevLock = false;
		Cursor.SetCursor(tex, hotspot, CursorMode.Auto);
	}

	private void Update()
	{
		UpdatePos();
		Pos3DtoPos2D();
		shortClick.Update();
		//UpdateCursorLock();
	}

	private void OnGUI()
	{
	}

	private void UpdatePos()
	{
		if (isNowLock)
		{
			move = Vector2.zero;
		}
		else
		{
			move = Input.mousePosition - pos;
			pos = Input.mousePosition;
		}
		//POINT lpPoint;
		//if (GetCursorPos(out lpPoint))
		//{
		//	rawPos.x = lpPoint.X;
		//	rawPos.y = lpPoint.Y;
		//}
	}

	//[DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
	//private static extern void SetCursorPos(int X, int Y);

	//[DllImport("USER32.dll")]
	//[return: MarshalAs(UnmanagedType.Bool)]
	//private static extern bool GetCursorPos(out POINT lpPoint);

	//[DllImport("USER32.dll")]
	//private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

	//[DllImport("USER32.dll")]
	//private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

	//[DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
	//private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

	//[DllImport("user32.dll")]
	//private static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);

	//[DllImport("user32.dll")]
	//private static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

	//[DllImport("user32.dll")]
	//private static extern IntPtr GetForegroundWindow();

	//[DllImport("user32.dll")]
	//private static extern IntPtr FindWindow(string className, string windowName);

	//public void SetCoursorPosition(Vector3 mousePos)
	//{
	//	POINT lpPoint = new POINT(0, 0);
	//	ClientToScreen(windowPtr, ref lpPoint);
	//	GetWindowRect(windowPtr, ref winRect);
	//	POINT pOINT = new POINT(lpPoint.X - winRect.left, lpPoint.Y - winRect.top);
	//	lpPoint.X = (int)mousePos.x;
	//	lpPoint.Y = Screen.height - (int)mousePos.y;
	//	ClientToScreen(windowPtr, ref lpPoint);
	//	SetCursorPos(lpPoint.X + pOINT.X, lpPoint.Y + pOINT.Y);
	//}

	//private static void UpdateCursorLock()
	//{
	//	if (isNowLock)
	//	{
	//		SetCursorPos(lockPos.X, lockPos.Y);
	//	}
	//	if (isNextLock)
	//	{
	//		if (!isPrevLock)
	//		{
	//			GetCursorPos(out lockPos);
	//			isNowLock = true;
	//			IsDraw = false;
	//			Cursor.lockState = CursorLockMode.Locked;
	//		}
	//	}
	//	else if (isPrevLock)
	//	{
	//		Cursor.lockState = CursorLockMode.None;
	//		SetCursorPos(lockPos.X, lockPos.Y);
	//		isNowLock = false;
	//		IsDraw = true;
	//	}
	//	isNextLock = false;
	//	isPrevLock = isNowLock;
	//}

	public static void Lock()
	{
		isNextLock = true;
	}

	private void Pos3DtoPos2D()
	{
		pos2D = new Vector2(pos.x, (float)Screen.height - pos.y);
	}
}
