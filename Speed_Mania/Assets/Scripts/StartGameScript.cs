using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameScript : MonoBehaviour {
	public Button Mute;
	public Button Help;
	public Button Credits;
	public Button StartGame;
	public GameObject HelpText;
	public GameObject CreditsText;
	private AudioSource source;


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		source.Play ();
		SharedVariables.Mute = false;
		StartGame.onClick.AddListener(() =>
			{
				Time.timeScale = 1;
				Application.LoadLevel("scene1");

			});
		Help.onClick.AddListener(() =>
			{
				if(HelpText.active == true)
					HelpText.SetActive(false);
				else
					HelpText.SetActive(true);

			});
		Credits.onClick.AddListener(() =>
			{
				if(CreditsText.active)
					CreditsText.SetActive(false);
				else
					CreditsText.SetActive(true);
			});
		Mute.onClick.AddListener(() =>
			{
				
				SharedVariables.Mute = !SharedVariables.Mute;
				if (SharedVariables.Mute)
					source.Stop ();
				else
					source.Play ();
				Debug.Log(SharedVariables.Mute);
			});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
