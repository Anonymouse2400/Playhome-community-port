using System;
using System.IO;
using Character;
using UnityEngine;

[Serializable]
public class AccessoryCustom : ParameterBase
{
	public const int SLOT_NUM = 10;

	public ACCESSORY_TYPE type = ACCESSORY_TYPE.NONE;

	public int id = -1;

	public ACCESSORY_ATTACH nowAttach = ACCESSORY_ATTACH.NONE;

	public Vector3 addPos = Vector3.zero;

	public Vector3 addRot = Vector3.zero;

	public Vector3 addScl = Vector3.one;

	public ColorParameter_PBR2 color;

	public AccessoryCustom(SEX sex)
		: base(sex)
	{
		Init();
	}

	public AccessoryCustom(AccessoryCustom copy)
		: base(copy.sex)
	{
		Copy(copy);
	}

	public void Init()
	{
		type = ACCESSORY_TYPE.NONE;
		id = -1;
		nowAttach = ACCESSORY_ATTACH.NONE;
		addPos = Vector3.zero;
		addRot = Vector3.zero;
		addScl = Vector3.one;
		color = null;
	}

	public void Copy(AccessoryCustom src)
	{
		type = src.type;
		id = src.id;
		nowAttach = src.nowAttach;
		addPos = src.addPos;
		addRot = src.addRot;
		addScl = src.addScl;
		if (src.color != null)
		{
			color = new ColorParameter_PBR2(src.color);
		}
		else
		{
			color = null;
		}
	}

	public void Set(ACCESSORY_TYPE type, int id, string key)
	{
		this.type = type;
		this.id = id;
		nowAttach = AccessoryData.CheckAttach(key);
		color = new ColorParameter_PBR2();
		if (nowAttach == ACCESSORY_ATTACH.NONE && type != ACCESSORY_TYPE.NONE)
		{
			AccessoryData acceData = CustomDataManager.GetAcceData(type, id);
			if (acceData != null)
			{
				nowAttach = acceData.defAttach;
			}
		}
	}

	public void ResetAttachPosition()
	{
		addPos = Vector3.zero;
		addRot = Vector3.zero;
		addScl = Vector3.one;
	}

	public bool CheckAttachInHead()
	{
		return AccessoryData.CheckAttachInHead(nowAttach);
	}

	public void CheckHasData()
	{
		if (!CustomDataManager.HasAcceData(type, id))
		{
			Init();
		}
	}

	public void Save(BinaryWriter writer, SEX sex)
	{
		Write(writer, type);
		Write(writer, id);
		Write(writer, nowAttach);
		Write(writer, addPos);
		Write(writer, addRot);
		Write(writer, addScl);
		if (color != null)
		{
			color.Save(writer);
		}
		else
		{
			writer.Write(0);
		}
	}

	public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
	{
		Read(reader, ref type);
		Read(reader, ref id);
		Read(reader, ref nowAttach);
		Read(reader, ref addPos);
		Read(reader, ref addRot);
		Read(reader, ref addScl);
		color = new ColorParameter_PBR2();
		color.Load(reader, version);
	}

	protected void Write(BinaryWriter writer, ACCESSORY_TYPE val)
	{
		writer.Write((int)val);
	}

	protected void Read(BinaryReader reader, ref ACCESSORY_TYPE val)
	{
		val = (ACCESSORY_TYPE)reader.ReadInt32();
	}

	protected void Write(BinaryWriter writer, ACCESSORY_ATTACH val)
	{
		writer.Write((int)val);
	}

	protected void Read(BinaryReader reader, ref ACCESSORY_ATTACH val)
	{
		val = (ACCESSORY_ATTACH)reader.ReadInt32();
	}
}
