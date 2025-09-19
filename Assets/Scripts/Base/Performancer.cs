using UnityEngine;

namespace AwesomeBalls
{
    public class Performancer : MonoBehaviour
    {

        private void Awake()
        {
			Application.targetFrameRate = Mathf.Max(60, (int)Screen.currentResolution.refreshRateRatio.value);
		}
	}
}