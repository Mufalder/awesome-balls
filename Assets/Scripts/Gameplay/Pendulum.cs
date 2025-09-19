using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AwesomeBalls
{
	public class Pendulum : MonoBehaviour
	{

		[SerializeField, Header("Components")]
		private LineRenderer lineRenderer = null;
		[SerializeField]
		private Transform attachPoint = null;
		[SerializeField, Header("Pendulum properties")]
		private float length = 2;
		[SerializeField, Range(-90, 90)]
		private float amplitude = 90;
		[SerializeField, Range(-45, 45)]
		private float startPhase = 0;
		[SerializeField]
		private float speedMultiplier = 1;
		[SerializeField, Header("Balls")]
		private float ballSpawnDelay = 2;

		private float w0;
		private const float G = 9.81f;

		private float dropTime;
		private Ball currentBall;

		private Vector2 oldPos;
		private Vector2 velocity;

		public Vector2 Pos { get; private set; }

		private bool canDrop => GameManager.CurrentState == GameManager.GameState.Game && Time.time > dropTime;

		private void Start()
		{
			w0 = Mathf.Sqrt(G / length);
		}

		private void OnEnable()
		{
			if (currentBall == null)
				SpawnBall();
		}

		private void Update()
		{
			if (currentBall == null && canDrop)
				SpawnBall();

			float angle = amplitude * Mathf.Deg2Rad * Mathf.Sin(speedMultiplier * w0 * Time.time + startPhase * Mathf.Deg2Rad);
			float x = length * Mathf.Sin(angle);
			float y = -length * Mathf.Cos(angle) + transform.position.y;

			Pos = new Vector2(x, y);
			velocity = Pos - oldPos;

			lineRenderer.SetPosition(1, new Vector3(x, y, transform.position.z));
			attachPoint.position = Pos;

			if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
			{
				Drop();
			}

			oldPos = Pos;
		}

		private void SpawnBall()
		{
			if (GameManager.CurrentState != GameManager.GameState.Game)
				return;

			currentBall = BallFactory.CreateBall(BallsDataset.GetRandomBallData(), Pos, true);
			currentBall.transform.SetParent(attachPoint);
			currentBall.transform.localPosition = Vector3.zero;
		}

		private IEnumerator DropSequence()
		{
			dropTime = Time.time + ballSpawnDelay + 0.4f;

			currentBall.transform.SetParent(null);
			currentBall.SetKinematic(false);
			currentBall.SetVelocity(velocity / Time.smoothDeltaTime);
			currentBall = null;

			yield return new WaitForSeconds(ballSpawnDelay);
			SpawnBall();
		}

		private void Drop()
		{
			if (!canDrop || currentBall == null)
				return;

			Haptic.VibratePredefined(Haptic.PredefinedEffect.Effects.Click, () => { Haptic.Vibrate(30, 10, TapticPlugin.ImpactFeedback.Light); });
			StartCoroutine(DropSequence());
		}

	}

}