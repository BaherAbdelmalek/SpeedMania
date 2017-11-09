using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovingCar : MonoBehaviour {
	public KeyCode MoveL;
	public KeyCode MoveL1;
	public KeyCode MoveR1;
	public KeyCode MoveR;
	public KeyCode escape;
	private float Zpos = 120;
	public GameObject Plateform;
	public GameObject Runner;
	public GameObject Collider;
	public GameObject Coin;
	public GameObject Laser;
	public GameObject Obtsacle;
	public KeyCode CameraButton;
	private bool fps = false;
	public GameObject FirstCamera;
	public GameObject SecondCamera;
	private bool ground;
	private bool pause;
	public Button Resume;
	public Button Restart;
	public Button Quit;
	private float LaserZl;
	private float ObstacleZl;
	private float CoinZl;
	public Text Score;
	public Text FinalScore;
	public GameObject GameOverScreen;
	public GameObject PauseScreen;
	private int SpeedIncreasing;
	public AudioClip Punch;
	public AudioClip Hit;
	public AudioClip Pause;
	public AudioClip Catch;
	public AudioClip Jump;
	public AudioClip Background;
	public GameObject AndroidCanvas;
	private AudioSource source;
	private float Ystart;


	//public KeyCode Jump;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = new Vector3 (0, 0,5);
		StartCoroutine("GenerateRandom");
		Ystart = transform.position.y;
		source = GetComponent<AudioSource>();
		pause = false;
		ground = true;
		SharedVariables.score = 0;
		SharedVariables.GameOver = false;
		SpeedIncreasing = 1;
		SharedVariables.speed = 5;	
		if (SharedVariables.Mute)
			source.Stop();
		#if UNITY_ANDROID
		AndroidCanvas.active = true;
		#endif
	}


	// Update is called once per frame
	void Update () {
		Score.text = "Score: " + SharedVariables.score;
		if ((SharedVariables.score / 50) == SpeedIncreasing) {
			SharedVariables.speed = 5 + ((SharedVariables.score / 50) * 2);
			Debug.Log (SharedVariables.speed);
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, SharedVariables.speed);
			SpeedIncreasing ++;
		}

		transform.Translate (new Vector2 (Input.acceleration.x, 0) * Time.deltaTime * 7f);
		#if UNITY_EDITOR
		if (transform.position.y == 1.0f)
			ground = true;
		#endif

		if (Plateform.transform.childCount > 3)
			Destroy(Plateform.transform.GetChild(0).gameObject);		
	
		if ((Input.GetKeyDown (MoveL) || Input.GetKeyDown (MoveL1)) && !pause) {
			float y = transform.position.y;
			float z = transform.position.z;
			float x = -8.5f;
			if (transform.position.x > -8.5f)
				x = transform.position.x - 8.5f;
			Vector3 pos = new Vector3 (x, y, z);
			transform.position = pos;

		}	
		if ((Input.GetKeyDown (MoveR) || Input.GetKeyDown (MoveR1)) && !pause) {
			float y = transform.position.y;
			float z = transform.position.z;
			float x = 8.5f;
			if (transform.position.x < 8.5f)
				x = transform.position.x + 8.5f;
			Vector3 pos = new Vector3 (x, y, z);
			transform.position = pos;
		}
	
		#if UNITY_EDITOR
		if (Input.GetKeyDown ("space") && ground && !pause) {
			Vector3 up = transform.TransformDirection (Vector3.up);
			(GetComponent<Rigidbody> ()).AddForce (up * 10, ForceMode.Impulse);
			if (!SharedVariables.Mute)
			source.PlayOneShot(Jump,1.0f);
			ground = false;
		}
		#endif
		if (Input.GetKeyDown (escape)) {
			if (!SharedVariables.GameOver) {
				if (PauseScreen.active == true) {
					pause = false;
					PauseScreen.active = false;
					source.clip = Background; 
					if (!SharedVariables.Mute)
						source.Play ();
					Time.timeScale = 1;	
				} else {
					pause = true;
					source.clip = Pause;
					if (!SharedVariables.Mute)
						source.Play ();
					PauseScreen.active = true;
					Time.timeScale = 0;
				}
			}
		}
		if (Input.GetKeyDown (CameraButton) && !pause) {
			fps = !fps;
			FirstCamera.active = fps;
			if (!fps)
				SecondCamera.transform.position = new Vector3 (0, 9, transform.position.z - 15);
			SecondCamera.active = !fps; 
		}
		if (SecondCamera.transform.position.z > transform.position.z) {
			GetComponent<Rigidbody>().velocity = new Vector3 (0, 0,SharedVariables.speed);
			transform.position = new Vector3 (0, 2.39f, SecondCamera.transform.position.z + 17);
		}
	}
	public void ResumePlaying(){
		Time.timeScale = 1;
		pause = false;
		source.clip = Background; 
		if (!SharedVariables.Mute)
		source.Play ();
		PauseScreen.active = false;
	}
	public void RestartGame(){
		Time.timeScale = 1;
		Application.LoadLevel("scene1");
		//SceneManager.LoadScene( SceneManager.GetActiveScene().name );
	}

	public void QuitGame(){
		//Application.Quit();
		#if UNITY_ANDROID
		Application.Quit();
		#endif
		#if UNITY_EDITOR	
		Application.LoadLevel("GameStart");
		#endif

	}
	IEnumerator OnTriggerEnter(Collider other) {
		if (other.gameObject == Collider) {
			//GameObject.Destroy (Plateform.transform.GetChild (0).gameObject);
			GameObject insta =  Instantiate(Runner, new Vector3(0, 0, Zpos), Quaternion.Euler(0, 0, 0)) as GameObject;
			insta.transform.parent = Plateform.transform;
			Collider.transform.position = new Vector3 (Collider.transform.position.x, Collider.transform.position.y,insta.transform.position.z-45);
			Zpos = insta.transform.position.z+45;
		}
		if (other.CompareTag ("Coin")) {
			SharedVariables.score += 10;
			if (!SharedVariables.Mute)
			source.PlayOneShot(Catch,1.0f);
			Destroy (other.gameObject);
		}
		if (other.CompareTag ("Laser")) {
			if (!SharedVariables.Mute)
			source.PlayOneShot(Hit,1.0f);
			if (SharedVariables.score >= 50)
				SharedVariables.score -= 50;
			else
				SharedVariables.score = 0;
			SharedVariables.speed = 5 + ((SharedVariables.score / 50) * 2);
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, SharedVariables.speed);
			SpeedIncreasing--;
		}
		if (other.CompareTag ("Obstacle")) {
			//Destroy (this.gameObject);
			source.PlayOneShot(Punch,6.0f);
			yield return new WaitForSeconds(0.4f);
			GameOverScreen.SetActive (true);
			Score.gameObject.SetActive (false);
			FinalScore.text = "Final Score: " + SharedVariables.score;
			Time.timeScale = 0;
			pause = true;
			source.clip = Pause;
			if (!SharedVariables.Mute)
			source.Play ();
			SharedVariables.GameOver = true;
		}
		#if UNITY_ANDROID
		if (other.CompareTag ("Ground") || other.CompareTag ("Coin") || other.CompareTag ("Laser")) {
			ground = true;
		}
		#endif
	}
	void OnCollisionEnter(Collision collision)
	{
		#if UNITY_ANDROID
		if(collision.gameObject.CompareTag("Ground") ||collision.gameObject.CompareTag("Coin") || collision.gameObject.CompareTag("Laser"))
			ground = true;
		#endif

	}

	IEnumerator GenerateRandom() {
		for(;;) {
			// execute block of code here
			int action = Random.Range(0, 5);
			int lane = Random.Range(1, 3);

			switch (action)
			{
			case 5:
				GenerateCoin();
				break;
			case 4:
				float ObstacleXpos = 0;
				switch (lane) {
				case 1:
					ObstacleXpos = 0f;
					break;
				case 2:
					ObstacleXpos = 8.5f;
					break;
				case 3:
					ObstacleXpos = -8.5f;
					break; 
				}
			
				float lastObstacle = Mathf.Max(ObstacleZl, transform.position.z + 8);
				ObstacleZl = lastObstacle + 8f;
				Instantiate(Obtsacle, new Vector3(ObstacleXpos, 2.4f, ObstacleZl), Quaternion.identity);
				break;
			case 3:
				float lastlaser = Mathf.Max(LaserZl, transform.position.z + 5f);
				if(LaserZl > transform.position.z + 5f)
				LaserZl = lastlaser + 20f;
				else
					LaserZl = lastlaser + 15f;
				Instantiate(Laser, new Vector3(0, 1.5f, LaserZl),Quaternion.Euler(0, 0, 90));
				break;
			case 2:
				GenerateCoin();
				break;
			case 1:		
				GenerateCoin();
				break;
			case 0:
				GenerateCoin();
				break;
			default:
				break;
			}
			yield return new WaitForSeconds(1);
		}
	}
	public void AndroidJump(){
		if (ground &&  !pause) {
			Vector3 up = transform.TransformDirection (Vector3.up);
			(GetComponent<Rigidbody> ()).AddForce (up * 13, ForceMode.Impulse);
			if (!SharedVariables.Mute)
				source.PlayOneShot (Jump, 1.0f);
			ground = false;
		}
	}

	public void AndroidCamera(){
		Debug.Log ("android");
		if (!pause) {
			fps = !fps;
			FirstCamera.active = fps;
			if (!fps)
				SecondCamera.transform.position = new Vector3 (0, 9, transform.position.z - 15);
			SecondCamera.active = !fps; 
		}
	}
	public void AndroidPause(){
		if (!SharedVariables.GameOver) {
			if (PauseScreen.active == true) {
				pause = false;
				PauseScreen.active = false;
				source.clip = Background; 
				if (!SharedVariables.Mute)
					source.Play ();
				Time.timeScale = 1;	
			} else {
				pause = true;
				source.clip = Pause;
				if (!SharedVariables.Mute)
					source.Play ();
				PauseScreen.active = true;
				Time.timeScale = 0;
			}
		}
	}
	public void GenerateCoin(){
		int lane = Random.Range(1, 3);
		float CoinXpos = 0;
		switch (lane) {
		case 1:
			CoinXpos = -0.1f;
			break;
		case 2:
			CoinXpos = 8.5f;
			break;
		case 3:
			CoinXpos = -8.5f;
			break; 
		}
		float last = Mathf.Max(CoinZl, transform.position.z + 5);
		CoinZl = last + 5f;
		Instantiate(Coin, new Vector3(CoinXpos, 2, CoinZl), Quaternion.Euler(90, 0, 0));
	}
}
