using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	public static class GameManager
	{
		
		public enum GameState { Start, Game, End };

		public delegate void GameStateChange(GameState newState);
		public static GameStateChange OnGameStateChanged { get; set; }

		private static GameState currentState = GameState.Start;
		public static GameState CurrentState
		{
			get => currentState;
			set
			{
				if (currentState == value)
					return;

				currentState = value;
				if (currentState == GameState.Start)
					score = 0;

				OnGameStateChanged?.Invoke(currentState);
			}
		}

		private static int score = 0;
		public static int Score => score;
		public delegate void ScoreChanged(int newScore);
		public static ScoreChanged OnScoreChanged { get; set; }
		public static void AddScore(int toAdd)
		{
			if (CurrentState != GameState.Game || toAdd <= 0)
				return;

			score += toAdd;
			OnScoreChanged?.Invoke(score);
		}

	}
}