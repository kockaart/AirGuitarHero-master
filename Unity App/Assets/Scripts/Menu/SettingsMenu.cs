using UnityEngine;
using System.Collections;


public class SettingsMenu : MenuBase {
	
	protected Rect calibRect;
	protected Rect credRect;
	protected Rect backRect;
	
	// Use this for initialization
	new void Start () {
		base.Start();
		
		calibRect = new Rect(this.screenWidth / 7, (float)(this.screenHeight / 2.38),(float)(this.screenWidth / 3.5), this.screenHeight / 8);
		credRect = new Rect(this.screenWidth / 7, (float)(this.screenHeight / 1.8),(float)(this.screenWidth / 3.5), this.screenHeight / 8);
		backRect = new Rect(this.screenWidth / 7, (float)(this.screenHeight / 1.44),(float)(this.screenWidth / 3.5), this.screenHeight / 8);
	}
	
	// Update is called once per frame
	void Update () {
		//kinect management
		if(this.clickEnabled && (this.kinectController.getRightHand().z > KinectMenuController.CLICK_Z || (this.kinectController.getLeftHand().z > KinectMenuController.CLICK_Z)))
		{
			if(checkClick(this.calibRect))
			{
				Application.LoadLevel("Calibrate");
			}
			else
				if(checkClick(this.credRect))
				{
					Application.LoadLevel("Credits");
				}
				else
					if(checkClick(this.backRect))
					{		
						Application.LoadLevel("MainMenu");
					}
		}
	}
	
	void OnGUI() {		
		GUI.skin = menuSkin;
		
		//Volume
		GUI.Label (new Rect(this.screenWidth / 4.3f, this.screenHeight / 3.77f, this.screenWidth / 3.5f, this.screenHeight / 8), "Volume");
		float volume = GUI.HorizontalSlider(new Rect(this.screenWidth / 7, (float)(this.screenHeight / 3),(float)(this.screenWidth / 3.5), this.screenHeight / 8), MusicManager.GetVolume(), (float)0.1, 1);
		MusicManager.SetVolume (volume);
		
		//Calibrage
		if(GUI.Button(calibRect,"Calibrate")){
			Application.LoadLevel("Calibrate");
		} //Credits
		else if(GUI.Button(credRect,"Credits")){
			Application.LoadLevel("Credits");
		}//Back to main menu
		else if(GUI.Button(backRect,"Menu")){
			Application.LoadLevel("MainMenu");
		}
		GUI.Label(new Rect(this.screenWidth / 7, (float)(this.screenHeight / 1.2),(float)(this.screenWidth / 1.5), this.screenHeight / 6), "Play and play it again !");
		GUI.Box (new Rect(this.screenWidth / 10,this.screenHeight / 7, (float)(this.screenWidth / 2.6), (float)(this.screenHeight / 1.36)), "Settings");
			
	}
}
