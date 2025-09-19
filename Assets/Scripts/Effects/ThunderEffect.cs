using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	public class ThunderEffect : MonoBehaviour
	{

		[SerializeField]
		private LineRenderer lineRenderer = null;
		[SerializeField]
		private int divisions = 6;
		[SerializeField]
		private float amplitude = 0.2f;
		[SerializeField]
		private float lifeTime = 0.04f;

		public void Init(Vector2 start, Vector2 end)
		{
			lineRenderer.positionCount = divisions + 2;
			Vector2 dir = (end - start).normalized;
			float dist = Vector2.Distance(start, end) / lineRenderer.positionCount;
			for (int i = 0; i < lineRenderer.positionCount; i++)
			{
				Vector2 pos = start + dir * dist * i;
				if (i > 0 && i < lineRenderer.positionCount - 1)
					pos += Random.insideUnitCircle * amplitude;
				lineRenderer.SetPosition(i, pos);
			}
			Destroy(gameObject, lifeTime);
		}

		private void Update()
		{
			for (int i = 1; i < lineRenderer.positionCount - 1; i++)
			{
				Vector2 pos = lineRenderer.GetPosition(i);
				if (i > 0 && i < lineRenderer.positionCount - 1)
					pos += Random.insideUnitCircle * amplitude;
				lineRenderer.SetPosition(i, pos);
			}
		}

	}
}