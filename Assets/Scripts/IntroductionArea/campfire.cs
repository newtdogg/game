using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class campfire : MonoBehaviour
{
	public float Min;
	public float Max;
	public float Strength;
	public bool increasing;
	private PlayerBasicIntro playerIntro;
	private Light lightSource;
	private float _baseIntensity;
	private HintManager hintManager;
	private GameObject torch;
	private Texture2D cursor;
    //  private bool _flickering;
    // Start is called before the first frame update
    void Start(){
			cursor = new Texture2D(2,2);
			cursor.LoadImage(File.ReadAllBytes("Assets/Sprites/mouseclick.png"));
			Min = 10f;
			Max = 40f;
			increasing = true;
			Strength = 25;
			lightSource = gameObject.transform.GetChild(0).gameObject.GetComponent<Light>();
			playerIntro = GameObject.Find("Player").GetComponent<PlayerBasicIntro>();
			torch = GameObject.Find("Torch");
			hintManager = GameObject.Find("Hint").GetComponent<HintManager>();

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

	void OnMouseEnter(){
		if (hintManager.inHint == false) {
			Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
		}
	}

	void OnMouseExit(){
		if (hintManager.inHint == false) {
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}

	void OnMouseDown() {
		if(playerIntro.maxSlots == 0 && hintManager.inHint == false){
			playerIntro.maxSlots = 1;
			playerIntro.inventorySlot = 1;
			playerIntro.inventoryActive = torch;
			torch.transform.SetParent(playerIntro.transform.GetChild(1).gameObject.transform);
			torch.transform.position = new Vector3(playerIntro.transform.position.x, playerIntro.transform.position.y, -11);
			hintManager.setHint("Use scroll to change active item", "wasd");
			hintManager.timer += Time.deltaTime;
		}
	}
}
