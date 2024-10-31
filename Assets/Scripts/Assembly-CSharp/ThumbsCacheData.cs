using System;
using System.IO;

internal class ThumbsCacheData
{
	public int cardID;

	public byte[] data;

	public long time;

	public ThumbsCacheData()
	{
		cardID = -1;
		data = null;
		time = 0L;
	}

	public ThumbsCacheData(int cardID, byte[] data, long time)
	{
		this.cardID = cardID;
		this.data = data;
		this.time = time;
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write(cardID);
		writer.Write(data.Length);
		writer.Write(data);
		writer.Write(time);
	}

	public void Load(BinaryReader reader)
	{
		cardID = reader.ReadInt32();
		int count = reader.ReadInt32();
		data = reader.ReadBytes(count);
		time = reader.ReadInt64();
	}
}
