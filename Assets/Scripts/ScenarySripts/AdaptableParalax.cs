using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptableParalax : MonoBehaviour {

	public Sprite[] bgImages;
	[SerializeField]
	private float _foregroundSpeed;
	public float foregroundSpeed{
		get{ return _foregroundSpeed; }
		set{ _foregroundSpeed = value; AjustSpeed (); }
	}
	[SerializeField]
	private SpriteRenderer[] background;
	[SerializeField]
	private float[] bgSpeed;

	// Use this for initialization
	void Awake () {
		background = new SpriteRenderer[bgImages.Length*3];
		bgSpeed = new float[bgImages.Length];

		for (int i = 0; i < bgImages.Length * 3; i += 3) {
			background [i] = new GameObject ().AddComponent<SpriteRenderer> ();
			background [i].sortingLayerName = "Background";
			background [i].sprite = bgImages [i / 3];
			if (i/3 >= bgImages.Length-1) {
				Vector3 position = background [i].transform.position;
				position.y = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0)).y + background [i].bounds.extents.y;
				background [i].transform.position = position;
			}
			background [i + 1] = CreateClone(background[i], true);
			background [i + 2] = CreateClone(background[i]);
			background [i].name = background [i+1].name = background [i+2].name = bgImages [i/3].name;
			background [i].transform.parent = background [i + 1].transform.parent = background [i + 2].transform.parent = this.transform;
		}
		for (int i = 0; i < bgImages.Length; i++) {
			bgSpeed [i] = foregroundSpeed / Mathf.Pow (2, i);
			background [3 * i].sortingOrder = background [(3 * i) + 1].sortingOrder = background [(3 * i) + 2].sortingOrder = i;
		}
		System.Array.Reverse (bgSpeed);
	}

	void AjustSpeed(){
		for (int i = 0; i < bgSpeed.Length; i++) {
			bgSpeed [i] = foregroundSpeed / Mathf.Pow (2, i);
		}
		System.Array.Reverse (bgSpeed);
	}

	SpriteRenderer CreateClone(SpriteRenderer img, bool left = false){
		Vector3 position = img.transform.position;
		if (left) {
			position.x = img.bounds.min.x - img.bounds.extents.x;
		} else {
			position.x = img.bounds.extents.x + img.bounds.max.x;
		}
		GameObject Clone = (GameObject)Instantiate (img.gameObject, position, Quaternion.identity);
		Clone.transform.parent = transform;
		return Clone.GetComponent<SpriteRenderer>();
	}

	void MovePart(Transform part, float spd){
		part.Translate (Vector3.left * spd * Time.deltaTime);
	}

	void CheckRepeating(){
		for (int i = 0; i < bgImages.Length * 3; i += 3) {
			Repeat (background [i + 1], background [i + 2]);
			Repeat (background [i], background [i + 1]);
			Repeat (background [i + 2], background [i]);
		}
	}

	void Repeat(SpriteRenderer obj, SpriteRenderer lastSprite){
		Transform last = lastSprite.transform;
		if(obj.transform.position.x + obj.bounds.extents.x < Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x-0.1f){
			Vector3 position = last.position;
			position.x += obj.bounds.size.x;
			obj.transform.position = position;	
		}
	}
	
	// Update is called once per frame
	// TODO: Create a better way to don't need to use Mathf.pow every frame
	void Update () {
		for (int i = bgSpeed.Length-1; i >= 0; i--) {
			MovePart (background [3 * i].transform, bgSpeed [i]);
			MovePart (background [(3 * i)+1].transform, bgSpeed [i]);
			MovePart (background [(3 * i)+2].transform, bgSpeed [i]);
		}
		CheckRepeating ();
	}
}
