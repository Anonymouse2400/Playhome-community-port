using System;
using UnityEngine;

public class H_SE : MonoBehaviour
{
	private GameControl gameCtrl;

	public H_SE_Clips clips;

	public H_Scene h_scene;

	[SerializeField]
	private float delay = 0.1f;

	[SerializeField]
	private float hiSpeed = 1f;

	public float Delay
	{
		get
		{
			return delay;
		}
	}

	private void Start()
	{
		gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
	}

	public void Play_Insert(Female female)
	{
		Vector3 zero = Vector3.zero;
		zero = female.CrotchTrans.position;
		gameCtrl.audioCtrl.Play3DSE(clips.insert, zero);
	}

	public void Play_Piston(Female female, float speed)
	{
		Vector3 zero = Vector3.zero;
		zero = female.CrotchTrans.position;
		AudioClip clip = ((!CheckHiSpeed(speed)) ? clips.piston01 : clips.piston00);
		gameCtrl.audioCtrl.Play3DSE(clip, zero);
	}

	public void Play_Crotch(Female female, float speed)
	{
		Vector3 zero = Vector3.zero;
		zero = female.CrotchTrans.position;
		AudioClip clip = ((!CheckHiSpeed(speed)) ? clips.crotch01 : clips.crotch00);
		gameCtrl.audioCtrl.Play3DSE(clip, zero);
	}

	private bool CheckHiSpeed(float speed)
	{
		return speed >= hiSpeed;
	}
}
