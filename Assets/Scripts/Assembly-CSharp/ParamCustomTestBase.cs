using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

public abstract class ParamCustomTestBase : MonoBehaviour
{
	public class NameAndID
	{
		public string name;

		public int id;

		public NameAndID()
		{
		}

		public NameAndID(string name, int id)
		{
			this.name = name;
			this.id = id;
		}
	}

	public class ItemSelectionSet
	{
		private List<NameAndID> list;

		private Dictionary<int, int> id_map;

		public string[] names { get; protected set; }

		public void Setup<T_Data>(Dictionary<int, T_Data> datas) where T_Data : ItemDataBase
		{
			list = new List<NameAndID>();
			id_map = new Dictionary<int, int>();
			names = new string[datas.Count];
			if (datas == null)
			{
				return;
			}
			foreach (KeyValuePair<int, T_Data> data in datas)
			{
				int count = list.Count;
				id_map.Add(data.Key, count);
				names[count] = data.Value.name;
				list.Add(new NameAndID(data.Value.name, data.Key));
			}
		}

		public void Setup(NameAndID[] setList)
		{
			list = new List<NameAndID>();
			id_map = new Dictionary<int, int>();
			names = new string[setList.Length];
			if (setList != null)
			{
				foreach (NameAndID nameAndID in setList)
				{
					int count = list.Count;
					id_map.Add(nameAndID.id, count);
					names[count] = nameAndID.name;
					list.Add(new NameAndID(nameAndID.name, nameAndID.id));
				}
			}
		}

		public int SelectNoToID(int select)
		{
			if (select == -1)
			{
				return -1;
			}
			return list[select].id;
		}

		public int IDToSelectNo(int id)
		{
			int value = -1;
			if (id_map.TryGetValue(id, out value))
			{
				return value;
			}
			return -1;
		}
	}

	public ItemSelectionSet sel_acceType = new ItemSelectionSet();

	public ItemSelectionSet sel_acceAttach = new ItemSelectionSet();

	public ItemSelectionSet[] sel_acceItems = new ItemSelectionSet[12];

	public abstract Human GetHuman();

	protected void SetupAcce()
	{
		List<NameAndID> list = new List<NameAndID>();
		list.Add(new NameAndID("なし", -1));
		list.Add(new NameAndID("頭", 0));
		list.Add(new NameAndID("耳", 1));
		list.Add(new NameAndID("眼", 2));
		list.Add(new NameAndID("顔", 3));
		list.Add(new NameAndID("首", 4));
		list.Add(new NameAndID("肩", 5));
		list.Add(new NameAndID("胸", 6));
		list.Add(new NameAndID("腰", 7));
		list.Add(new NameAndID("背", 8));
		list.Add(new NameAndID("腕", 9));
		list.Add(new NameAndID("手", 10));
		list.Add(new NameAndID("脚", 11));
		sel_acceType.Setup(list.ToArray());
		List<NameAndID> list2 = new List<NameAndID>();
		list2.Add(new NameAndID("頭", 0));
		list2.Add(new NameAndID("左耳", 2));
		list2.Add(new NameAndID("右耳", 3));
		list2.Add(new NameAndID("眼鏡", 1));
		list2.Add(new NameAndID("鼻", 5));
		list2.Add(new NameAndID("口", 4));
		list2.Add(new NameAndID("首", 6));
		list2.Add(new NameAndID("胸", 7));
		list2.Add(new NameAndID("左乳首", 22));
		list2.Add(new NameAndID("右乳首", 23));
		list2.Add(new NameAndID("左肩", 25));
		list2.Add(new NameAndID("右肩", 26));
		list2.Add(new NameAndID("左腕", 10));
		list2.Add(new NameAndID("右腕", 11));
		list2.Add(new NameAndID("左手首", 8));
		list2.Add(new NameAndID("右手首", 9));
		list2.Add(new NameAndID("左手", 27));
		list2.Add(new NameAndID("右手", 28));
		list2.Add(new NameAndID("左人差指", 12));
		list2.Add(new NameAndID("右人差指", 13));
		list2.Add(new NameAndID("左中指", 14));
		list2.Add(new NameAndID("右中指", 15));
		list2.Add(new NameAndID("左薬指", 16));
		list2.Add(new NameAndID("右薬指", 17));
		list2.Add(new NameAndID("腰", 24));
		list2.Add(new NameAndID("左脚", 18));
		list2.Add(new NameAndID("右脚", 19));
		list2.Add(new NameAndID("左足首", 20));
		list2.Add(new NameAndID("右足首", 21));
		sel_acceAttach.Setup(list2.ToArray());
		for (ACCESSORY_TYPE aCCESSORY_TYPE = ACCESSORY_TYPE.HEAD; aCCESSORY_TYPE < ACCESSORY_TYPE.NUM; aCCESSORY_TYPE++)
		{
			int num = (int)aCCESSORY_TYPE;
			sel_acceItems[num] = new ItemSelectionSet();
			sel_acceItems[num].Setup(CustomDataManager.GetAccessoryDictionary(aCCESSORY_TYPE));
		}
	}
}
