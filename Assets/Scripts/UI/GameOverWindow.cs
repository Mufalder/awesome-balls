using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AwesomeBalls
{
	public class GameOverWindow : MonoBehaviour
	{

		[SerializeField]
		private Transform window = null;
		[SerializeField]
		private TextMeshProUGUI score = null;
		[SerializeField]
		private Button againButton = null;
		[SerializeField]
		private Button toMenuButton = null;
		[SerializeField]
		private Image fade = null;
		[SerializeField, Header("Animation Params")]
		private float scaleTime = 0.2f;
		[SerializeField]
		private AudioSource counterSound = null;
		[SerializeField]
		private float counterDelay = 0.6f;
		[SerializeField]
		private float counterTime = 1f;
		[SerializeField]
		private float buttonsActivateDelay = 0.5f;

		private void OnEnable()
		{
			Haptic.VibratePredefined(Haptic.PredefinedEffect.Effects.HeavyClick, () => { Haptic.Vibrate(50, 20, TapticPlugin.ImpactFeedback.Medium); });
			fade.fillAmount = 0;
			StartCoroutine(Animation());
		}

		private IEnumerator Animation()
		{
			againButton.interactable = toMenuButton.interactable = false;
			score.text = "0";
			window.localScale = Vector3.zero;
			window.DOScale(1, scaleTime);
			if (GameManager.Score > 0)
			{
				yield return new WaitForSeconds(counterDelay);
				counterSound.Play();
				float t = 0;
				while (t < counterTime)
				{
					float val = Mathf.Lerp(0, GameManager.Score, t / counterTime);
					t += Time.deltaTime;
					score.text = val.ToString("F0");
					yield return null;
				}
				score.text = GameManager.Score.ToString();
				counterSound.Stop();
			}
			yield return new WaitForSeconds(buttonsActivateDelay);
			againButton.interactable = toMenuButton.interactable = true;
		}

	}
}