using System;
using System.Collections.Generic;
using UnityEngine;

public class MapMob : MonoBehaviour
{
	[SerializeField]
	private MobVoiceSet[] voiceSets;

	[SerializeField]
	private MobPosSet[] mouthPos;

	private List<ClipAndVoiceType> voiceList = new List<ClipAndVoiceType>();

	private Dictionary<int, List<MobPosSet>> posDic = new Dictionary<int, List<MobPosSet>>();

	private float timer;

	private bool isFirst = true;

	[SerializeField]
	private float firstTime = 10f;

	[SerializeField]
	private float nextTimeMin = 5f;

	[SerializeField]
	private float nextTimeMax = 15f;

	private void Start()
	{
		for (int i = 0; i < voiceSets.Length; i++)
		{
			for (int j = 0; j < voiceSets[i].clips.Length; j++)
			{
				voiceList.Add(new ClipAndVoiceType(i, voiceSets[i].clips[j]));
			}
		}
		for (int k = 0; k < mouthPos.Length; k++)
		{
			int id = mouthPos[k].id;
			if (!posDic.ContainsKey(id))
			{
				posDic.Add(id, new List<MobPosSet>());
			}
			posDic[id].Add(mouthPos[k]);
		}
		timer = 0f;
		isFirst = true;
		NextTime();
	}

	private void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			Voice();
			NextTime();
		}
		for (int i = 0; i < mouthPos.Length; i++)
		{
			mouthPos[i].mouth.volume = ConfigData.VolumeVoice_Mob();
		}
	}

	private void Voice()
	{
		if (voiceList.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, voiceList.Count);
			ClipAndVoiceType clipAndVoiceType = voiceList[index];
			voiceList.RemoveAt(index);
			int id = clipAndVoiceType.id;
			if (posDic.ContainsKey(id))
			{
				List<MobPosSet> list = posDic[id];
				int index2 = UnityEngine.Random.Range(0, list.Count);
				MobPosSet mobPosSet = list[index2];
				mobPosSet.mouth.PlayOneShot(clipAndVoiceType.clip);
			}
		}
	}

	private void NextTime()
	{
		if (isFirst)
		{
			timer = firstTime;
			isFirst = false;
		}
		else
		{
			timer = UnityEngine.Random.Range(nextTimeMin, nextTimeMax);
		}
	}
}
