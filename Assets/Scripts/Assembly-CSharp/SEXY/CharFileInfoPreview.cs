using System;
using System.IO;

namespace SEXY
{
	[Serializable]
	public class CharFileInfoPreview : BlockControlBase
	{
		public int previewLoadVersion;

		public int productNo = 11;

		public int sex;

		public int personality;

		public string name = string.Empty;

		public int height;

		public int bustSize;

		public int hairType;

		public int state;

		public int resistH;

		public int resistPain;

		public int resistAnal;

		public int isConcierge;

		public CharFileInfoPreview()
			: base("プレビュー情報", 4)
		{
		}

		public override bool LoadBytes(byte[] data, int previewVer)
		{
			using (MemoryStream input = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					if (4 <= previewVer)
					{
						productNo = binaryReader.ReadInt32();
					}
					sex = binaryReader.ReadInt32();
					if (2 <= previewVer)
					{
						personality = binaryReader.ReadInt32();
						int num = binaryReader.ReadInt32();
						name = binaryReader.ReadString();
						height = binaryReader.ReadInt32();
						bustSize = binaryReader.ReadInt32();
						hairType = binaryReader.ReadInt32();
					}
					if (4 <= previewVer)
					{
						state = binaryReader.ReadInt32();
						resistH = binaryReader.ReadInt32();
						resistPain = binaryReader.ReadInt32();
						resistAnal = binaryReader.ReadInt32();
					}
					if (3 <= previewVer)
					{
						isConcierge = binaryReader.ReadInt32();
					}
					return true;
				}
			}
		}

		public void Update(CharFile chaFile)
		{
			sex = chaFile.customInfo.sex;
			name = chaFile.customInfo.name;
			isConcierge = (chaFile.customInfo.isConcierge ? 1 : 0);
			if (sex == 0)
			{
				personality = 255;
				height = 255;
				bustSize = 255;
				hairType = 255;
				state = 255;
				resistH = 255;
				resistPain = 255;
				resistAnal = 255;
				return;
			}
			personality = chaFile.customInfo.personality;
			float num = chaFile.customInfo.shapeValueBody[0];
			if (num < 0.33f)
			{
				height = 0;
			}
			else if (num > 0.66f)
			{
				height = 2;
			}
			else
			{
				height = 1;
			}
			float num2 = chaFile.customInfo.shapeValueBody[1];
			if (num2 < 0.33f)
			{
				bustSize = 0;
			}
			else if (num2 > 0.66f)
			{
				bustSize = 2;
			}
			else
			{
				bustSize = 1;
			}
			hairType = chaFile.customInfo.hairType;
		}
	}
}
