using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class campfireBasic : MonoBehaviour
{
	public float Min;
	public float Max;
	public float Strength;
	public bool increasing;
	private Light lightSource;
    //  private bool _flickering;
    // Start is called before the first frame update
    void Start(){
			Min = 10f;
			Max = 40f;
			increasing = true;
			Strength = 25;
			lightSource = gameObject.transform.GetChild(0).gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update(){
		Flickering();
    }

	private void Flickering(){
		if (increasing && lightSource.intensity < Max){
			lightSource.intensity += 0.3f;
		}
		if (increasing && lightSource.intensity >= Max){
			increasing = false;
			System.Random r = new System.Random();
			Max = r.Next(25, 45);
		}
		if (!increasing && lightSource.intensity > Min){
			lightSource.intensity -= 0.3f;
		}
		if (!increasing && lightSource.intensity <= Min){
			increasing = true;
			System.Random r = new System.Random();
			Max = r.Next(10, 25);
		}
	}
}
