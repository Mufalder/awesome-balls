using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AwesomeBalls
{
	public class GameStateUnityObserver : GameStateObserver
	{

		[SerializeField]
		protected UnityEvent startEvent = null;
		[SerializeField]
		protected UnityEvent gameEvent = null;
		[SerializeField]
		protected UnityEvent endEvent = null;

		protected override void OnEnd()
		{
			endEvent?.Invoke();
		}

		protected override void OnGame()
		{
			gameEvent?.Invoke();
		}

		protected override void OnStart()
		{
			startEvent?.Invoke();
		}
	}
}