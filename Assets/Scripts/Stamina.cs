using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour {

	private GameObject player;
	private PlayerController playerController;

	private SpriteRenderer staminaBackgroundSprite;
	private SpriteRenderer spriteRenderer;
	private GameObject staminaBackground;
	private float staminaPercent;
	private Color backgroundColor;
	private Color staminaColor;

	// Use this for initialization
	void Start () {
		player = this.gameObject.transform.parent.gameObject;
		staminaBackground = GameObject.Find("StaminaBackground");
		staminaBackgroundSprite = staminaBackground.GetComponent<SpriteRenderer>();
		playerController = player.GetComponent<PlayerController>();
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		staminaColor = spriteRenderer.color;
		backgroundColor = staminaBackgroundSprite.color;
	}
	// Update is called once per frame
	void Update () {
		var localStamina = playerController.stamina;
		if(playerController.stamina < -1){
			spriteRenderer.color = new Color(1f,0f,0f);
			staminaPercent = (localStamina + 6)/2;
		} else {
			staminaPercent = localStamina /2;
		}
		gameObject.transform.localScale = new Vector3(1, staminaPercent, 0);
		gameObject.transform.localPosition = new Vector3(0.25f, 0.17f + staminaPercent/8, 0);
		if (playerController.stamina >= 5){
			spriteRenderer.color = new Color(0f,0f,0f, 0f);
			staminaBackgroundSprite.color = new Color(0f,0f,0f, 0f);
		} else {
			spriteRenderer.color = staminaColor;
			staminaBackgroundSprite.color = backgroundColor;
		}

	}
}
