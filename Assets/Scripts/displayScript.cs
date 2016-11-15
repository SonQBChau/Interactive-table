using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class displayScript : MonoBehaviour {
	private MovieTexture movie;
	private AudioSource audio;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showQualification()
	{
		Debug.Log ("showQualification");
		hideAll ();
		GameObject qualification = gameObject.transform.FindChild ("qualificationGroup").gameObject;
		qualification.transform.localScale = new Vector3(1, 1, 1);//show
	}

	public void showCareer()
	{
		Debug.Log ("showCareer");
		hideAll ();
		GameObject career = gameObject.transform.FindChild ("careerGroup").gameObject;
		career.transform.localScale = new Vector3 (1, 1, 1);//show
	}

//	[RequireComponent(typeof(AudioSource))]
	public void showMovie()
	{
		Debug.Log ("showMovie");
		if (movie != null && movie.isPlaying) {
			movie.Pause ();
		} else {
			hideAll ();
			GameObject movieGroup = gameObject.transform.FindChild ("movieGroup").gameObject;
			movieGroup.transform.localScale = new Vector3 (1, 1, 1);//show
			GameObject movieFrame = movieGroup.transform.FindChild ("movieFrame").gameObject;
			movie = (MovieTexture)movieFrame.transform.GetComponent<RawImage> ().texture;
			audio = movieFrame.transform.GetComponent<AudioSource> ();
			audio.clip = movie.audioClip;
			movie.Play ();
			audio.Play ();
		}
	}

	public void hideAll(){
		GameObject qualification = gameObject.transform.FindChild ("qualificationGroup").gameObject;
		qualification.transform.localScale = new Vector3 (0, 0, 0);
		GameObject career = gameObject.transform.FindChild ("careerGroup").gameObject;
		career.transform.localScale = new Vector3 (0, 0, 0);
		GameObject movieGroup = gameObject.transform.FindChild ("movieGroup").gameObject;
		movieGroup.transform.localScale = new Vector3 (0, 0, 0);
		if (movie != null && movie.isPlaying) {
			movie.Stop();
		}
	}
}
