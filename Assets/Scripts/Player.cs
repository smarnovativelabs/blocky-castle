using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{


	public GameObject level1;
	public GameObject level2;

	Rigidbody rb;
	CameraFollow camFollow;
	public Text text;
	public Transform[] doorExitPoints;
	public Transform[] innerPoints1;
	public Transform[] innerPoints2;
	[Space]
	public Transform[] doorExitPointsLevel2;
	public Transform[] innerPoints1Level2;
	public Transform[] innerPoints2Level2;
	[Space]
	public Transform[] doorExitPointsLevel3;
	public Transform[] innerPoints1Level3;
	public Transform[] innerPoints2Level3;
	[Space]
	public Transform futureParent;
	public Transform[] trebuchetPoints;

	AudioSource audioSource;
	public AudioClip deadClip;
	public Animation anim;
	public Animation trebuchetAnim;
	public float minSwipeDistY;
	public float minSwipeDistX;
	bool canInput;
	bool canEnterDoor;
	int level;
	bool canColEnter = true;
	float[] levelRot;
	int[] levelCounts;
	float targetLevelRot;
	// Use this for initialization
	void Start () 
	{
  		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		camFollow = Camera.main.GetComponentInParent<CameraFollow> ();
		level = PlayerPrefs.GetInt ("level");
		if (level == 0) 
		{
			level1.SetActive (true);
		}
		else if (level == 1) 
		{
			level2.SetActive (true);
		}
//		level = 1;
		canInput = true;
		minSwipeDistX = Screen.width / 8;
		minSwipeDistY = minSwipeDistX;
		canEnterDoor = false;
//		minSwipeDistY = Screen.height / 15;
		trebuchetAnim["Take 001"].speed=0.5f;
		trebuchetAnim.transform.position = trebuchetPoints [level].position;
		anim.Play ("Armature.001|wing");
		levelRot = new float[3];
		levelCounts = new int[3];
		levelCounts [0] = 5;
		levelCounts [1] = 6;
//		levelCounts [2] = 5;
		levelRot [0] = 95;
		levelRot [1] = 160;
//		levelRot [2] = 150;

		targetLevelRot = levelRot [level];
	}



	Touch  touch;
	Vector2 startPos;

	// Update is called once per frame
//	void Update () 
//	{
//		if (Input.touchCount > 0)
//		{
//			touch = Input.touches [0];
//			if (touch.phase == TouchPhase.Began) 
//			{
//				startPos = touch.position;
//			}
//			else if (touch.phase == TouchPhase.Moved) 
//			{
//				float swipeDistVertical = (new Vector3 (0, touch.position.y, 0) - new Vector3 (0, startPos.y, 0)).magnitude;
//				if(swipeDistVertical>minSwipeDistY)
//				{
//					float swipeValue = Mathf.Sign (touch.position.y - startPos.y);
//					if (swipeValue > 0)
//					{
////						text.text = "swipe up";
//						Jump ();
//					}
//					else
//					{
//						text.text = "swipe down";
//					}
//				}
//
//
//
//				float swipeDistHorizontal = (new Vector3 (touch.position.x, 0, 0) - new Vector3 (startPos.x, 0, 0)).magnitude;
//				if(swipeDistHorizontal>minSwipeDistX)
//				{
//					float swipeValue = Mathf.Sign (touch.position.x - startPos.x);
//					if (swipeValue > 0)
//					{
////						text.text = "swipe right";
//						Forward();
//					}
//					else
//					{
//						text.text = "swipe left";
//					}
//				}
//			}
//			else if (touch.phase == TouchPhase.Ended) 
//			{
//			}
//
//		}
//	}
//


	bool gameover = false;
	Vector2 touchStartPos;
	bool isCompleted = false;
	void Update()
	{
		if (canInput) 
		{
			if (Input.GetKeyDown (KeyCode.LeftArrow)) 
			{
				Forward ();
			}
			else if (Input.GetKeyDown (KeyCode.RightArrow)) 
			{
				Backward ();
			}
			else if (Input.GetKeyDown (KeyCode.UpArrow)) 
			{
				Jump ();
			}
			else if (Input.GetKeyDown (KeyCode.DownArrow)) 
			{
				canInput = false;
				rb.AddForce (-Vector3.up * upForce);
			}
		}


		if (Input.GetMouseButtonDown (0)) 
		{
			touchStartPos = Input.mousePosition;
		}
		else if (Input.GetMouseButton (0)) 
		{
			if (Input.mousePosition.x > touchStartPos.x + minSwipeDistX)
			{
				if (!isCompleted) 
				{
					isCompleted = true;
					Forward ();
				}
			}
			else if (Input.mousePosition.y > touchStartPos.y + minSwipeDistY) 
			{
				if (!isCompleted) 
				{
					isCompleted = true;
					Jump ();
				}
			}
		}
		else if (Input.GetMouseButtonUp (0)) 
		{
			isCompleted = false;
		}
			
		if (rb.velocity.y < -4)
		{
			gameover = true;
		}
			
	}

	void Forward()
	{
		if (canInput)
		{
			audioSource.Play ();
			anim.Play ("Armature|jump_end");
			canInput = false;
			rb.AddForce (Vector3.up * upForce);
			if (!isFacingRight) 
			{
				isFacingRight = true;
				forwardForce *= -1;
				transform.Rotate (0, 180, 0);
			}
			StartCoroutine (Force ());
		}
	}
	void Backward()
	{
		canInput = false;
		//				rb.isKinematic = false;
		rb.AddForce (Vector3.up * upForce);
		if (isFacingRight) 
		{
			isFacingRight = false;
			forwardForce *= -1;
			transform.Rotate (0, 180, 0);
		}
		StartCoroutine (Force ());
	}
	void Jump()
	{
		if (canInput)
		{
			if (canEnterDoor)
			{
				StartCoroutine (DoorAnim ());
			} else 
			{
				canInput = false;
				anim.Play ("Armature|jump_end");
				audioSource.Play ();
				rb.AddForce (Vector3.up * upForce);
			}
		}

	}
	bool isFacingRight = true;
	float upForce = 130;
	float rightForce = 50;
	float forwardForce = 20;
	WaitForSeconds forceDelay = new WaitForSeconds(0.2f);
	IEnumerator Force()
	{
		yield return forceDelay;
//		if (isFacingRight)
//		{
//			transform.Rotate (0, -15, 0);
//			camFollow.targetRot.y -= 15;
//		}
//		else
//		{
//			transform.Rotate (0, 15, 0);
//			camFollow.targetRot.y += 15;
//		}
		rb.AddForce (transform.right * rightForce);
		rb.AddForce (transform.forward * forwardForce);

	}


	public GameObject prev;

	void OnCollisionEnter(Collision col)
	{
		
//		print (col.gameObject);
		if (canColEnter)
		{
			
			if (gameover && !GameManager.o.isGameOver) 
			{
				GameManager.o.GameOver ();
				audioSource.PlayOneShot (deadClip);
				return;
			}
//			print (prev.transform.eulerAngles + "  ----  " + col.transform.eulerAngles);
			canColEnter = false;
			Invoke ("EnableColEnter", 0.4f);
			if (col.gameObject.tag == "stairs" && !GameManager.o.isGameOver) {// && prev != col.gameObject) 
				canInput = true;
				anim.Play ("Armature.001|wing");
				rb.velocity = Vector3.zero;
				transform.position = new Vector3 (col.transform.position.x, transform.position.y, col.collider.transform.position.z);
//				transform.Rotate (0, col.transform.eulerAngles.y - prev.transform.eulerAngles.y, 0);'
				if (isFacingRight)
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, col.transform.eulerAngles.y + targetLevelRot, transform.eulerAngles.z);
				else 
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, col.transform.eulerAngles.y + 95, transform.eulerAngles.z);

				prev = col.gameObject;
			}
			else if (col.gameObject.tag == "movingStair") {// && transform.position.y > col.transform.position.y) 
//				print (transform.position + "    "  + col.transform.position);
				canInput = true;
				transform.position = new Vector3 (col.transform.position.x, transform.position.y, col.collider.transform.position.z);
				transform.parent = col.transform;
//				transform.Rotate (0, col.transform.eulerAngles.y - prev.transform.eulerAngles.y, 0);
			}
			else if (col.gameObject.tag == "specialStair") 
			{
				canInput = true;
				rb.velocity = Vector3.zero;
				transform.position = new Vector3 (col.transform.position.x, transform.position.y, col.collider.transform.position.z);
				prev = col.gameObject;
			}
		}

		if(col.gameObject.tag=="hurdle" && !GameManager.o.isGameOver)
		{
			rb.AddForce (0, 150, -80);
			rb.AddTorque (-01f, 0, 0);
			GameManager.o.GameOver ();
			audioSource.PlayOneShot (deadClip);
		}
	}

	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "movingStair")
		{
			transform.parent = null;
		}
	}

	void EnableColEnter()
	{
		canColEnter = true;
	}



	void OnTriggerEnter(Collider col)
	{
//		print (col.gameObject);
		if (col.tag == "doorPoint") 
		{
			canEnterDoor = true;
		}
		else if (col.tag == "hurdle" && !GameManager.o.isGameOver)
		{
			rb.AddForce (0, Random.Range (120, 150), Random.Range (-80, -100));
			rb.AddTorque (-1f, 0, 0);
//			gameObject.SetActive (false);
//			transform.GetComponentsInChildren<MeshRenderer>().enabled=false;
			GameManager.o.GameOver ();
			audioSource.PlayOneShot (deadClip);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "doorPoint") 
		{
			canEnterDoor = false;
		}

	}

	public int count = 0;
	IEnumerator DoorAnim()
	{

		rb.isKinematic = true;
		canInput = false;
		camFollow.enabled = false;
		if (level == 0) 
		{
			transform.eulerAngles = innerPoints1 [count].eulerAngles;
		}
		else if (level == 1)
		{
			transform.eulerAngles = innerPoints1Level2 [count].eulerAngles;
		}
		else if (level == 2)
		{
			transform.eulerAngles = innerPoints1Level3 [count].eulerAngles;
		}
		WaitForFixedUpdate delay = new WaitForFixedUpdate ();
		Vector3 targetPos = new Vector3 ();
		if (level == 0) 
		{
			targetPos = innerPoints1 [count].position;
		}
		else if (level == 1)
		{
			targetPos = innerPoints1Level2 [count].position;
		}
		else if (level == 2)
		{
			targetPos = innerPoints1Level3 [count].position;
		}
		while (transform.position != targetPos) 
		{
			transform.position = Vector3.MoveTowards (transform.position, targetPos, 0.02f);
			yield return delay;
		}
		if (level == 0) 
		{
			transform.position = innerPoints2 [count].position;
			transform.eulerAngles = innerPoints2 [count].eulerAngles;
			targetPos = doorExitPoints [count].position;
		}
		else if (level == 1)
		{
			transform.position = innerPoints2Level2 [count].position;
			transform.eulerAngles = innerPoints2Level2 [count].eulerAngles;
			targetPos = doorExitPointsLevel2 [count].position;
		}
		else if (level == 2)
		{
			transform.position = innerPoints2Level3 [count].position;
			transform.eulerAngles = innerPoints2Level3 [count].eulerAngles;
			targetPos = doorExitPointsLevel3 [count].position;
		}

		while (transform.position != targetPos) 
		{
			transform.position = Vector3.MoveTowards (transform.position, targetPos, 0.02f);
			yield return  delay;
		}

		if (level == 0) 
		{
			transform.eulerAngles = doorExitPoints [count].eulerAngles;
		}
		else if (level == 1) 
		{
			transform.eulerAngles = doorExitPointsLevel2 [count].eulerAngles;
		}
		else if (level == 2)
		{
			transform.eulerAngles = doorExitPointsLevel3 [count].eulerAngles;
		}
		count++;
		if (count >= levelCounts[level]) 
		{
			camFollow.enabled = true;
			StartCoroutine (LevelCompleteAnim ());
		}
		else
		{
			camFollow.enabled = true;
			rb.isKinematic = false;
			canInput = true;
		}

	}

	IEnumerator LevelCompleteAnim()
	{
		transform.parent = futureParent;
		transform.eulerAngles = futureParent.GetChild (0).eulerAngles;
		yield return new WaitForSeconds (2);
		trebuchetAnim.Play ("Take 001");
		yield return new WaitForSeconds (0.6f);
//		GameManager.o.PlayLevelCompleteClip ();
		transform.parent = null;
		rb.isKinematic = false;
		rb.AddForce (Vector3.up * 400);
		rb.AddForce (Vector3.right * 500);
		camFollow.enabled = false;
		camFollow.transform.parent = transform;
		transform.GetComponent<TrailRenderer> ().enabled = true;
		yield return new WaitForSeconds (2);
		GameManager.o.LevelComplete ();
		transform.GetComponent<Collider> ().enabled = false;
		camFollow.transform.parent = null;
		yield return new WaitForSeconds (1);
		gameObject.SetActive (false);

	}


}
