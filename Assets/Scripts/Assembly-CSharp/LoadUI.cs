using System;
using UnityEngine;

public abstract class LoadUI : MonoBehaviour
{
	public abstract void Setup(Sprite thumbnail, string name, string file, Action<int> loadAct);
}
