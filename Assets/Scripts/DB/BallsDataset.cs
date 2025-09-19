using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	[System.Serializable, CreateAssetMenu(fileName = "New BallData", menuName = "AwesomeBalls/Ball Data")]
	public class BallsDataset : ScriptableObject
	{

		[SerializeField]
		private BallData[] entries = null;

		private static ResourceProperty<BallsDataset> data = new ResourceProperty<BallsDataset>("Balls/BallsDataset");

		public static BallData GetBallData(int index)
		{
			return data.Value.entries[index];
		}

		public static BallData GetRandomBallData()
		{
			return data.Value.entries.RandomElement();
		}

	}
}