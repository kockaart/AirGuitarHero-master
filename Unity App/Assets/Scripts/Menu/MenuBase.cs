using UnityEngine;
using System.Collections;

public class MenuBase : MonoBehaviour {
	
	//Use the skin defined in the inspector
	public GUISkin menuSkin;
	protected GUIText debug;
	
	protected KinectMenuController kinectController;
	
	protected float screenWidth, screenHeight;
	
	protected bool clickEnabled;

	// Use this for initialization
	public void Start () {
		this.clickEnabled = false;
		StartCoroutine("enableClick", 1f);
		
		this.kinectController = (KinectMenuController) FindObjectOfType(typeof(KinectMenuController));		
		this.debug = (GUIText) GameObject.Find("DebugLabel").GetComponent<GUIText>();
		this.screenWidth = Screen.width;
		this.screenHeight = Screen.height;
	}
	
	protected IEnumerator enableClick(float wait)
	{
		yield return new WaitForSeconds(wait);
		this.clickEnabled = true;
	}
	
	protected bool checkClick(Rect rect)
	{
		Vector2 rightHand = kinectController.getRightHand();
		rightHand.x *= screenWidth;
		rightHand.y = 1 - rightHand.y;
		rightHand.y *= screenHeight;
		
		Vector2 leftHand = kinectController.getLeftHand();
		leftHand.x *= screenWidth;
		leftHand.y = 1 - leftHand.y;
		leftHand.y *= screenHeight;
		
		return rect.Contains(rightHand)||rect.Contains(leftHand);				
	}
}
