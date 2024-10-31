using System;

[Serializable]
public class DLDataHeader
{
	public int UID;

	public int __PersonalType;

	public int BustSize;

	public int BHType;

	public int Sex;

	public int Height;

	public int __State;

	public bool __RegistH;

	public bool __RegistPain;

	public bool __RegistAnal;

	public string HandleName = string.Empty;

	public string Comment = string.Empty;

	public string __CharaName = string.Empty;

	public string __PersonalTypeName = string.Empty;

	public string UserID = string.Empty;

	public int DLCount;

	public int WeekCount;
}
