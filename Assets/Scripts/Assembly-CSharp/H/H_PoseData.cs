using System;
using UnityEngine;

namespace H
{
	public class H_PoseData
	{
		public enum POSITION
		{
			FLOOR = 0,
			CHAIR = 1,
			WALL = 2
		}

		public enum MEMBER
		{
			PAIR = 0,
			M1F2 = 1,
			NUM = 2
		}

		public string id;

		public string name;

		public bool showMale;

		public IllusionCameraResetData camera;

		public bool hasLight;

		public Vector3 lightDir;

		public IK_DataList ikDatas = new IK_DataList();

		public POSITION position;

		public H_StyleData.MEMBER members;

		public H_PoseData(TagText.Element element)
		{
			element.GetVal(ref id, "id", 0);
			element.GetVal(ref name, "name", 0);
			element.GetVal(ref showMale, "showMale", 0);
			TagText.Attribute attribute = element.GetAttribute("ik");
			if (attribute != null && attribute.vals.Count % 4 == 0)
			{
				for (int i = 0; i < attribute.vals.Count; i += 4)
				{
					string ikChara = attribute.vals[i];
					string ikPart = attribute.vals[i + 1];
					string targetChara = attribute.vals[i + 2];
					string targetPart = attribute.vals[i + 3];
					ikDatas.Add(new IK_Data(ikChara, ikPart, targetChara, targetPart));
				}
			}
			camera = new IllusionCameraResetData();
			element.GetVal(ref camera.pos.x, "cameraPos", 0);
			element.GetVal(ref camera.pos.y, "cameraPos", 1);
			element.GetVal(ref camera.pos.z, "cameraPos", 2);
			element.GetVal(ref camera.eul.x, "cameraEul", 0);
			element.GetVal(ref camera.eul.y, "cameraEul", 1);
			element.GetVal(ref camera.eul.z, "cameraEul", 2);
			element.GetVal(ref camera.dis, "cameraDis", 0);
			TagText.Attribute attribute2 = element.GetAttribute("light");
			if (attribute2 != null)
			{
				hasLight = true;
				attribute2.GetVal(ref lightDir.x, 0);
				attribute2.GetVal(ref lightDir.y, 1);
			}
			string val = string.Empty;
			element.GetVal(ref val, "position", 0);
			if (val == "壁")
			{
				position = POSITION.WALL;
			}
			else if (val == "椅子")
			{
				position = POSITION.CHAIR;
			}
			else
			{
				position = POSITION.FLOOR;
			}
			string val2 = string.Empty;
			element.GetVal(ref val2, "members", 0);
			if (val2 == "M1F2" || val2 == "F2M1")
			{
				members = H_StyleData.MEMBER.M1F2;
			}
			else
			{
				members = H_StyleData.MEMBER.PAIR;
			}
		}
	}
}
