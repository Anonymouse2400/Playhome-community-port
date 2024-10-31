using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class Mapping3D : MonoBehaviour
	{
		public GameObject _cubePrefab;

		private const int MaxCubes = 48;

		private const float SpawnTime = 0.25f;

		private float _timer = 0.25f;

		private List<GameObject> _cubes = new List<GameObject>(32);

		private void Update()
		{
			_timer -= Time.deltaTime;
			if (_timer <= 0f)
			{
				_timer = 0.25f;
				SpawnCube();
				if (_cubes.Count > 48)
				{
					RemoveCube();
				}
			}
		}

		private void SpawnCube()
		{
			Quaternion rotation = Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
			float num = Random.Range(0.1f, 0.6f);
			GameObject gameObject = Object.Instantiate(_cubePrefab, base.transform.position, rotation);
			Transform component = gameObject.GetComponent<Transform>();
			component.localScale = new Vector3(num, num, num);
			_cubes.Add(gameObject);
		}

		private void RemoveCube()
		{
			GameObject obj = _cubes[0];
			_cubes.RemoveAt(0);
			Object.Destroy(obj);
		}
	}
}
