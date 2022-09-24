using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgrageSwitch : MonoBehaviour {

	public GameObject Upgrage1;
	public GameObject Upgrage2;
	public GameObject Upgrage3;
	public int MaxWeapon = 3;
	private int ScrolInt;

	// Update is called once per frame
	private void Update () {
            
            Upgrage1.GetComponent<HookUpgrade>();
            Upgrage2.GetComponent<JetpackUpgrade>();
            Upgrage3.GetComponent<NightvisionUpgrade>();

		if (ScrolInt == 0) {
			Upgrage1.SetActive (true);
			Upgrage2.SetActive (false);
			Upgrage3.SetActive (false);
		}
		if (ScrolInt == 1) {
			Upgrage1.SetActive (false);
			Upgrage2.SetActive (true);
			Upgrage3.SetActive (false);
		}
		if (ScrolInt == 2) {
			Upgrage1.SetActive (false);
			Upgrage2.SetActive (false);
			Upgrage3.SetActive (true);
		}
		if (ScrolInt <= 0) {
			ScrolInt = 0;
		}
		if (ScrolInt >= MaxWeapon) {
			ScrolInt = MaxWeapon;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) { 
			ScrolInt += 1;
		}if (Input.GetAxis ("Mouse ScrollWheel") < 0f) { 
			ScrolInt -= 1;
		}
	}
}