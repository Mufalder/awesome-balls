using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AwesomeBalls
{
	public class Ball : GameStateObserver
	{

		[SerializeField]
		protected SpriteRenderer spriteRenderer = null;
		[SerializeField]
		protected Rigidbody2D rigid = null;
		[SerializeField]
		protected BallExplosion ballExplosion = null;

		public BallData Data { get; protected set; }

		private const float EmergeTime = 0.4f;

		private static BallsSounds ballsSounds;

		public virtual void Init(BallData data, bool kinematic = false)
		{
			Data = data;

			spriteRenderer.sprite = data.Sprite;

			rigid.isKinematic = kinematic;
		}

		protected virtual void Awake()
		{
			transform.DOPunchScale(Vector3.one * 0.2f, EmergeTime, 4, 1);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (ballsSounds == null)
				ballsSounds = FindObjectOfType<BallsSounds>();
		}

		protected virtual void LateUpdate()
		{
			spriteRenderer.transform.eulerAngles = Vector3.zero;
		}

		public void SetKinematic(bool kinematic)
		{
			rigid.isKinematic = kinematic;
		}

		public void SetVelocity(Vector2 velocity)
		{
			rigid.velocity = velocity;
		}

		public Vector2 GetVelocity() => rigid.velocity;

		public bool IsSleeping() => rigid.IsSleeping();

		public void Kill()
		{
			ballsSounds.Kill();
			GameManager.AddScore(Data.Score);
			BallExplosion explo = Instantiate(ballExplosion, transform.position, Quaternion.identity);
			explo.Init(Data);
			Destroy(gameObject);
			Destroy(explo.gameObject, 2);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			ballsSounds.Hit();
		}

		protected override void OnStart()
		{
			try
			{
				Destroy(gameObject);
			}
			catch
			{

			}
		}

		protected override void OnGame()
		{
		}

		protected override void OnEnd()
		{
		}
	}
}