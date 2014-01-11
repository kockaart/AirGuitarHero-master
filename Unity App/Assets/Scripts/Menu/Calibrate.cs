using UnityEngine;
using System.Collections;


public class Calibrate : MenuBase {
	
	
	protected Rect upRect;
	protected Rect downRect;
	protected Rect backRect;
	
	// Use this for initialization
	new void Start () {
		base.Start();
		
		Vector2 middle = new Vector2(this.screenWidth / 2f, this.screenHeight / 2f);
		backRect = new Rect((middle.x -  this.screenWidth /12f), (this.screenHeight/1.2f), this.screenWidth / 6f, this.screenHeight / 8f);
		
		Vector2 buttonSize = new Vector2(this.screenWidth / 3f, this.screenHeight / 5f);
		upRect = new Rect((middle.x -  buttonSize.x/2), (middle.y - buttonSize.y - 20), buttonSize.x, buttonSize.y);
		downRect = new Rect((middle.x - buttonSize.x/2), (middle.y + 20), buttonSize.x, buttonSize.y);
	}
	
	// Update is called once per frame
	void Update () {
		//kinect management
		if(this.clickEnabled && (this.kinectController.getRightHand().z > KinectMenuController.CLICK_Z || (this.kinectController.getLeftHand().z > KinectMenuController.CLICK_Z)))
		{
			if(checkClick(this.upRect))
			{
				//to prevent multiple clicks in a row
				this.clickEnabled = false;
				this.kinectController.moveKinect(0.2f);	
				StartCoroutine("enableClick", 2f);
			}
			else
				if(checkClick(this.downRect))
				{
					//to prevent multiple clicks in a row
					this.clickEnabled = false;
					this.kinectController.moveKinect(-0.2f);
					StartCoroutine("enableClick", 2f);
				}
				else
					if(checkClick(this.backRect))
					{		
						Application.LoadLevel("SettingsMenu");
					}
		}
	}
	
	void OnGUI() {		
		GUI.skin = menuSkin;
		
		//Buttons
		
		//Back to settings
		if(GUI.Button(backRect,"Back")){
			Application.LoadLevel("SettingsMenu");
		}
		else if(GUI.Button(upRect,"UP")){
			this.kinectController.moveKinect(0.2f);
		}
		else if(GUI.Button(downRect,"DOWN")){
			this.kinectController.moveKinect(-0.2f);
		}
		
		//GUI.Label(new Rect(this.screenWidth / 7, (float)(this.screenHeight / 1.2),(float)(this.screenWidth / 1.5), this.screenHeight / 6), "Play and play it again !");
		GUI.Box (new Rect(0, 0, (float)(this.screenWidth), (float)(this.screenHeight)), "Calibrate");
			
	}
}
