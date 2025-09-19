using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AwesomeBalls
{
	public class ScoreText : MonoBehaviour
	{

		[SerializeField]
		private TextMeshProUGUI text = null;

		private void OnEnable()
		{
			text.text = GameManager.Score.ToString();
			GameManager.OnScoreChanged += OnScoreChanged;
		}

		private void OnDisable()
		{
			GameManager.OnScoreChanged -= OnScoreChanged;
		}

		private void OnScoreChanged(int newScore)
		{
			text.text = newScore.ToString();

			transform.DOPunchScale(Vector3.one * 0.3f, 0.8f, 3, 1);
		}

	}
}