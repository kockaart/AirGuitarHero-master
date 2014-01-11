using UnityEngine;
using System.Collections;


public class Credits : MenuBase {
	
	private Rect backRect;
	
	// Use this for initialization
	new void Start () {
		base.Start();
		
		backRect = new Rect(this.screenWidth / 7, (float)(this.screenHeight / 1.44),(float)(this.screenWidth / 3.5), this.screenHeight / 8);
	}
	
	// Update is called once per frame
	void Update () {
		//kinect management
		if(this.clickEnabled && (this.kinectController.getRightHand().z > KinectMenuController.CLICK_Z || (this.kinectController.getLeftHand().z > KinectMenuController.CLICK_Z)))
		{
			if(checkClick(this.backRect))
			{		
				Application.LoadLevel("SettingsMenu");
			}
		}
	}
	
	void OnGUI() {
		
		GUI.skin = menuSkin;
		
		//Volume
		GUI.Label (new Rect(this.screenWidth / 3.5f, this.screenHeight / 3.5f, this.screenWidth / 3.5f, this.screenHeight / 8), "UTC");
	
		GUI.TextArea(new Rect(this.screenWidth / 7, this.screenHeight / 3.77f, this.screenWidth / 3.5f, this.screenHeight / 2.48f), "Developed by Valentin Hervieu and Vincent Meyer for the University of Technologie of Compiègne");
		
		//Back to settings
		if(GUI.Button(backRect,"Back")){
			Application.LoadLevel("SettingsMenu");
		}
		GUI.Label(new Rect(this.screenWidth / 7, (float)(this.screenHeight / 1.2),(float)(this.screenWidth / 1.5), this.screenHeight / 6), "Play and play it again !");
		GUI.Box (new Rect(this.screenWidth / 10,this.screenHeight / 7, (float)(this.screenWidth / 2.6), (float)(this.screenHeight / 1.36)), "Credits");
			
	}
}
