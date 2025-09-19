using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	public class BallExplosion : MonoBehaviour
	{

		[SerializeField]
		private ParticleSystem debree = null;

		public void Init(BallData data)
		{
			ParticleSystem.MainModule main = debree.main;
			main.startColor = data.Color;
		}

	}
}