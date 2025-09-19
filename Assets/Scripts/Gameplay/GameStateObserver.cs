using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AwesomeBalls
{
	public abstract class GameStateObserver : MonoBehaviour
	{

		protected virtual void OnEnable()
		{
			GameManager.OnGameStateChanged += OnGameStateChanged;
		}

		protected virtual void OnDisable()
		{
			GameManager.OnGameStateChanged += OnGameStateChanged;
		}

		private void OnGameStateChanged(GameManager.GameState newState)
		{
			switch (newState)
			{
				case GameManager.GameState.Start:
					OnStart();
					break;
				case GameManager.GameState.Game:
					OnGame();
					break;
				case GameManager.GameState.End:
					OnEnd();
					break;
			}
		}

		protected abstract void OnStart();
		protected abstract void OnGame();
		protected abstract void OnEnd();

	}
}