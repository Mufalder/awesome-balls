using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeBalls
{
	public class GameUI : GameStateObserver
	{

		[SerializeField]
		private Image fade = null;
		[SerializeField]
		private SoundContainer soundContainer = null;
		[SerializeField, Header("Start screen")]
		private RectTransform title = null;
		[SerializeField]
		private GameObject startScreen = null;
		[SerializeField]
		private RawImage bg = null;
		[SerializeField]
		private Vector2 bgScrollSpeed = Vector2.one;
		[SerializeField]
		private Image toGameFade = null;
		[SerializeField, Header("Game screen")]
		private GameObject gameScreen = null;
		[SerializeField]
		private Image fadeOut = null;
		[SerializeField, Header("End screen")]
		private GameObject endScreen = null;
		[SerializeField]
		private Image toMenuFade = null;

		private Sequence titleSequence;
		private bool ignore;

		private void Start()
		{
			fade.DOFade(0, 1);

			titleSequence = DOTween.Sequence();
			titleSequence.Append(title.DOScale(0.95f, 2).SetEase(Ease.InOutSine));
			titleSequence.Append(title.DOScale(1, 2).SetEase(Ease.InOutSine));
			titleSequence.SetLoops(-1);
		}

		private void Update()
		{
			if (bg.isActiveAndEnabled)
			{
				Rect rect = bg.uvRect;
				rect.x += bgScrollSpeed.x * Time.deltaTime;
				rect.y += bgScrollSpeed.y * Time.deltaTime;
				bg.uvRect = rect;
			}
		}

		public void ClickSound()
		{
			Haptic.VibratePredefined(Haptic.PredefinedEffect.Effects.Click, () => { Haptic.Vibrate(30, 10, TapticPlugin.ImpactFeedback.Light); });
			soundContainer.PlaySound("click");
		}

		public void Play()
		{
			if (ignore || GameManager.CurrentState != GameManager.GameState.Start)
				return;

			ClickSound();
			ignore = true;
			StartCoroutine(ToGameSequence());
		}

		public void Back()
		{
			if (ignore || GameManager.CurrentState == GameManager.GameState.Start)
				return;

			ClickSound();
			ignore = true;
			StartCoroutine(ToMenuSequence());
		}

		public void Again()
		{
			if (ignore)
				return;

			ClickSound();
			ignore = true;
			StartCoroutine(AgainSequence());
		}

		private IEnumerator ToGameSequence()
		{
			toGameFade.DOFillAmount(1, 1);
			yield return new WaitForSeconds(1.01f);
			GameManager.CurrentState = GameManager.GameState.Game;
			fadeOut.DOFillAmount(0, 1);
			ignore = false;
		}

		private IEnumerator ToMenuSequence()
		{
			if (GameManager.CurrentState == GameManager.GameState.Game)
				fadeOut.DOFillAmount(1, 1);
			else toMenuFade.DOFillAmount(1, 1);
			yield return new WaitForSeconds(1.01f);
			GameManager.CurrentState = GameManager.GameState.Start;
			toGameFade.DOFillAmount(0, 1);
			fadeOut.fillAmount = 1;
			ignore = false;
		}

		private IEnumerator AgainSequence()
		{
			toMenuFade.DOFillAmount(1, 1);
			yield return new WaitForSeconds(1.01f);
			GameManager.CurrentState = GameManager.GameState.Start;
			GameManager.CurrentState = GameManager.GameState.Game;
			fadeOut.fillAmount = 1;
			fadeOut.DOFillAmount(0, 1);
			ignore = false;
		}

		protected override void OnStart()
		{
		}

		protected override void OnGame()
		{
		}

		protected override void OnEnd()
		{
		}
	}
}