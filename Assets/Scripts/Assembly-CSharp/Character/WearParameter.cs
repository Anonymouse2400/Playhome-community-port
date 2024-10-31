using System;
using System.IO;

namespace Character
{
	public class WearParameter : ParameterBase
	{
		public WearCustom[] wears = new WearCustom[11];

		public bool isSwimwear;

		public bool swimOptTop = true;

		public bool swimOptBtm = true;

		public WearParameter(SEX sex)
			: base(sex)
		{
			for (int i = 0; i < wears.Length; i++)
			{
				wears[i] = new WearCustom(sex, (WEAR_TYPE)i, 0);
			}
		}

		public WearParameter(WearParameter copy)
			: base(copy.sex)
		{
			for (int i = 0; i < copy.wears.Length; i++)
			{
				wears[i] = new WearCustom(copy.wears[i]);
			}
			isSwimwear = copy.isSwimwear;
			swimOptTop = copy.swimOptTop;
			swimOptBtm = copy.swimOptBtm;
		}

		public void Init()
		{
			for (int i = 0; i < wears.Length; i++)
			{
				wears[i].Init();
			}
			isSwimwear = false;
			swimOptTop = true;
			swimOptBtm = true;
		}

		public int GetWearID(WEAR_TYPE type)
		{
			if (wears[(int)type] != null)
			{
				return wears[(int)type].id;
			}
			return -1;
		}

		public void Save(BinaryWriter writer, SEX sex)
		{
			for (int i = 0; i < wears.Length; i++)
			{
				wears[i].Save(writer);
			}
			if (sex == SEX.FEMALE)
			{
				Write(writer, isSwimwear);
				Write(writer, swimOptTop);
				Write(writer, swimOptBtm);
			}
		}

		public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
		{
			for (int i = 0; i < wears.Length; i++)
			{
				wears[i].Load(reader, sex, version);
			}
			if (sex == SEX.FEMALE)
			{
				Read(reader, ref isSwimwear);
				Read(reader, ref swimOptTop);
				Read(reader, ref swimOptBtm);
			}
		}

		public void Copy(WearParameter source)
		{
			isSwimwear = source.isSwimwear;
			swimOptTop = source.swimOptTop;
			swimOptBtm = source.swimOptBtm;
			for (int i = 0; i < wears.Length; i++)
			{
				wears[i].Copy(source.wears[i]);
			}
		}

		public void DisposeCoordinate(SEX sex)
		{
			WearData wearData = CustomDataManager.GetWearData(sex, WEAR_TYPE.TOP, GetWearID(WEAR_TYPE.TOP));
			WearData wearData2 = CustomDataManager.GetWearData(sex, WEAR_TYPE.BOTTOM, GetWearID(WEAR_TYPE.BOTTOM));
			if (isSwimwear)
			{
				int num = 0;
				int num2 = 1;
				int num3 = 2;
				int num4 = 3;
				wears[num].id = ((sex != 0) ? CustomDataManager.noWears_Male[num] : CustomDataManager.noWears_Female[num]);
				wears[num2].id = ((sex != 0) ? CustomDataManager.noWears_Male[num2] : CustomDataManager.noWears_Female[num2]);
				wears[num3].id = ((sex != 0) ? CustomDataManager.noWears_Male[num3] : CustomDataManager.noWears_Female[num3]);
				wears[num4].id = ((sex != 0) ? CustomDataManager.noWears_Male[num4] : CustomDataManager.noWears_Female[num4]);
				return;
			}
			int num5 = 4;
			int num6 = 5;
			int num7 = 6;
			wears[num5].id = ((sex != 0) ? CustomDataManager.noWears_Male[num5] : CustomDataManager.noWears_Female[num5]);
			wears[num6].id = ((sex != 0) ? CustomDataManager.noWears_Male[num6] : CustomDataManager.noWears_Female[num6]);
			wears[num7].id = ((sex != 0) ? CustomDataManager.noWears_Male[num7] : CustomDataManager.noWears_Female[num7]);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (wearData != null)
			{
				if (wearData.braDisable)
				{
					flag = true;
				}
				if (wearData.shortsDisable)
				{
					flag2 = true;
				}
				if (wearData.coordinates == 2)
				{
					flag3 = true;
				}
			}
			if (wearData2 != null)
			{
				if (wearData2.braDisable)
				{
					flag = true;
				}
				if (wearData2.shortsDisable)
				{
					flag2 = true;
				}
			}
			if (flag)
			{
				int num8 = 2;
				wears[num8].id = ((sex != 0) ? CustomDataManager.noWears_Male[num8] : CustomDataManager.noWears_Female[num8]);
			}
			if (flag2)
			{
				int num9 = 3;
				wears[num9].id = ((sex != 0) ? CustomDataManager.noWears_Male[num9] : CustomDataManager.noWears_Female[num9]);
			}
			if (flag3)
			{
				int num10 = 1;
				wears[num10].id = ((sex != 0) ? CustomDataManager.noWears_Male[num10] : CustomDataManager.noWears_Female[num10]);
			}
		}

		public void CheckHasData()
		{
			for (int i = 0; i < wears.Length; i++)
			{
				wears[i].CheckHasData();
			}
		}
	}
}
