using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace AwesomeBalls
{
	public class Grid : MonoBehaviour
	{

		private class Match
		{

			public Ball[] MatchedBalls { get; private set; }

			public Match(Ball[] matchedBalls)
			{
				MatchedBalls = matchedBalls;
			}

			public void DestroyBalls()
			{
				for (int i = 0; i < MatchedBalls.Length; i++)
				{
					MatchedBalls[i].Kill();
				}
			}

		}

		[SerializeField]
		private BoxCollider2D coll = null;
		[SerializeField]
		private int x = 3;
		[SerializeField]
		private int y = 3;
		[SerializeField]
		private float checkRate = 0.1f;
		[SerializeField]
		private float velocityThreshold = 0.1f;
		[SerializeField]
		private ThunderEffect thunderEffect = null;

		private float xStep;
		private float yStep;
		private int prevCount;
		private List<Ball> balls = new List<Ball>();
		private Dictionary<Vector2Int, Ball> grid = new Dictionary<Vector2Int, Ball>();
		private List<Match> matches = new List<Match>();

		private void OnEnable()
		{
			StartCoroutine(Checking());
		}

		private IEnumerator Checking()
		{
			xStep = coll.bounds.size.x / x;
			yStep = coll.bounds.size.y / y;

			while (true)
			{
				for (int i = balls.Count - 1; i >= 0; i--)
				{
					if (balls[i] == null)
						balls.RemoveAt(i);
				}

				//game over check
				if (balls.Count > x * y)
				{
					GameManager.CurrentState = GameManager.GameState.End;
					break;
				}

				if (balls.Count > x)
				{
					for (int i = 0; i < balls.Count; i++)
					{
						if (balls[i].IsSleeping() && balls[i].transform.position.y > coll.bounds.center.y + coll.bounds.extents.y + 0.05f)
						{
							GameManager.CurrentState = GameManager.GameState.End;
							break;
						}
					}
				}

				bool isSleeping = true;
				for (int i = 0; i < balls.Count; i++)
				{
					if (!balls[i].IsSleeping())
					{
						isSleeping = false;
						break;
					}
				}

				if (balls.Count > 2 && isSleeping && prevCount != balls.Count)
				{
					grid.Clear();
					matches.Clear();

					for (int i = 0; i < balls.Count; i++)
					{
						Vector3 locPos = transform.InverseTransformPoint(balls[i].transform.position);
						locPos.x += coll.bounds.extents.x;
						locPos.y += coll.bounds.extents.y;
						int xPos = Mathf.FloorToInt(locPos.x / xStep);
						int yPos = Mathf.FloorToInt(locPos.y / yStep);
						grid.Add(new Vector2Int(xPos, yPos), balls[i]);
					}

					//vertical checks
					for (int i = 0; i < x; i++)
					{
						if (grid.Where(x => x.Key.x == i).Count() < y || !grid.ContainsKey(new Vector2Int(i, 0)))
							continue;

						Color color = grid[new Vector2Int(i, 0)].Data.Color;
						bool success = true;
						for (int j = 1; j < y; j++)
						{
							Vector2Int key = new Vector2Int(i, j);
							if (!grid.ContainsKey(key) || color != grid[key].Data.Color)
							{
								success = false;
								break;
							}
						}

						if (success)
						{
							matches.Add(new Match(grid.Where(x => x.Key.x == i).Select(y => y.Value).ToArray()));
						}
					}

					//horizontal checks
					for (int i = 0; i < y; i++)
					{
						if (grid.Where(x => x.Key.y == i).Count() < x || !grid.ContainsKey(new Vector2Int(0, i)))
							continue;

						Color color = grid[new Vector2Int(0, i)].Data.Color;
						bool success = true;
						for (int j = 1; j < x; j++)
						{
							Vector2Int key = new Vector2Int(j, i);
							if (!grid.ContainsKey(key) || color != grid[key].Data.Color)
							{
								success = false;
								break;
							}
						}

						if (success)
						{
							matches.Add(new Match(grid.Where(x => x.Key.y == i).Select(y => y.Value).ToArray()));
						}
					}

					//diagonal checks
					if (x == y)
					{
						if (grid.ContainsKey(new Vector2Int(0, 0)))
						{
							bool success = true;
							Color color = grid[new Vector2Int(0, 0)].Data.Color;
							List<Ball> possibleMatch = new List<Ball>();
							for (int i = 0; i < x; i++)
							{
								Vector2Int key = new Vector2Int(i, i);
								if (!grid.ContainsKey(key) || color != grid[key].Data.Color)
								{
									success = false;
									break;
								}
								possibleMatch.Add(grid[key]);
							}

							if (success)
							{
								matches.Add(new Match(possibleMatch.ToArray()));
							}
						}

						if (grid.ContainsKey(new Vector2Int(0, x - 1)))
						{
							bool success = true;
							Color color = grid[new Vector2Int(0, x - 1)].Data.Color;
							List<Ball> possibleMatch = new List<Ball>();
							for (int i = 0; i < x; i++)
							{
								Vector2Int key = new Vector2Int(i, x - 1 - i);
								if (!grid.ContainsKey(key) || color != grid[key].Data.Color)
								{
									success = false;
									break;
								}
								possibleMatch.Add(grid[key]);
							}

							if (success)
							{
								matches.Add(new Match(possibleMatch.ToArray()));
							}
						}
					}

					ProcessMatches();
					prevCount = balls.Count;
				}
				yield return new WaitForSeconds(checkRate);
			}
		}

		private void ProcessMatches()
		{
			if (matches.Count > 0)
				Haptic.VibratePredefined(Haptic.PredefinedEffect.Effects.HeavyClick, () => { Haptic.Vibrate(50, 20, TapticPlugin.ImpactFeedback.Medium); });

			foreach (Match match in matches)
			{
				match.DestroyBalls();
				SpawnThunder(match.MatchedBalls[0].transform.position, match.MatchedBalls[match.MatchedBalls.Length - 1].transform.position);
			}
				
		}

		private void SpawnThunder(Vector2 start, Vector2 end)
		{
			ThunderEffect effect = Instantiate(thunderEffect, start, Quaternion.identity);
			effect.Init(start, end);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Ball"))
			{
				balls.Add(collision.GetComponent<Ball>());
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.CompareTag("Ball"))
			{
				balls.Remove(collision.GetComponent<Ball>());
			}
		}

	}
}