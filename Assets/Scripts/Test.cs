using UnityEngine;
using UnityEngine.UI;
using System;

public class Test : MonoBehaviour {
	[SerializeField] Text datetimeText;

	void Update ( ) {
		if (WorldTimeAPI.Instance.IsTimeLodaed ) {
			DateTime currentDateTime = WorldTimeAPI.Instance.GetCurrentDateTime ( );

			datetimeText.text = currentDateTime.ToString ( );
		}
	}
}
