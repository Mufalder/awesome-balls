using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	public class BallsSounds : MonoBehaviour
	{

		[SerializeField]
		private SoundContainer soundContainer = null;

		public void Hit()
		{
			soundContainer.PlaySound("hit");
		}

		public void Kill()
		{
			soundContainer.PlaySound("kill");
		}

	}
}