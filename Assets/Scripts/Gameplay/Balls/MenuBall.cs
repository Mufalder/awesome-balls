using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	public class MenuBall : MonoBehaviour
	{

		[SerializeField]
		private SpriteRenderer spriteRenderer = null;
		[SerializeField]
		private Rigidbody2D rigid = null;

		private void Start()
		{
			BallData ballData = BallsDataset.GetRandomBallData();
			spriteRenderer.sprite = ballData.Sprite;
		}

		private void FixedUpdate()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0;
				rigid.AddForce((transform.position - pos).normalized * 500);
			}
		}

		private void LateUpdate()
		{
			spriteRenderer.transform.eulerAngles = Vector3.zero;
		}

	}
}