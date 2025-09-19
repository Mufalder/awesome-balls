using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	public static class BallFactory
	{

		private static ResourceProperty<Ball> baseBallPrefab = new ResourceProperty<Ball>("Balls/BaseBall");

		public static Ball CreateBall(BallData ballData, Vector3 pos, bool isKinematic = false)
		{
			Ball ball = GameObject.Instantiate(baseBallPrefab.Value, pos, Quaternion.identity);
			ball.Init(ballData, isKinematic);
			return ball;
		}

	}
}