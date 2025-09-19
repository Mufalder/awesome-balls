using UnityEngine;

namespace AwesomeBalls
{
	[System.Serializable]
	public class BallData
	{

		[SerializeField]
		private Sprite sprite = null;
		public Sprite Sprite => sprite;
		[SerializeField]
		private Color color = Color.red;
		public Color Color => color;
		[SerializeField]
		private int score = 1;
		public int Score => score;

	}

}