using UnityEngine;
using System;
using System.Collections;
using System.IO;
using MiniJSON;

public class KinectMenuController : MonoBehaviour
{	
	public GameObject KinectPrefab;
	
	protected SkeletonWrapper sw;
	protected GUITexture leftCursor;
	protected GUITexture rightCursor;
	protected GUIText debug;
	
	private GameObject[] _bones;
	
	private Vector3 leftHand;
	private Vector3 rightHand;
	
	public const float CLICK_Z = 0.5f;
	protected Texture2D leftText, rightText, leftPushedText, rightPushedText;
	
	// Use this for initialization
	void Start ()
	{
		sw = (SkeletonWrapper) FindObjectOfType(typeof(SkeletonWrapper));
		if(sw == null)
		{
			Instantiate(KinectPrefab);
			sw = (SkeletonWrapper) FindObjectOfType(typeof(SkeletonWrapper));
		}
		leftCursor = (GUITexture) GameObject.Find("LeftCursor").GetComponent<GUITexture>();
		rightCursor = (GUITexture) GameObject.Find("RightCursor").GetComponent<GUITexture>();
		debug = (GUIText) GameObject.Find("DebugLabel").GetComponent<GUIText>();
		
		leftText = (Texture2D) Resources.Load("Images/left_cursor", typeof(Texture2D));
		rightText = (Texture2D) Resources.Load("Images/right_cursor", typeof(Texture2D));
		leftPushedText = (Texture2D) Resources.Load("Images/left_cursor_grip", typeof(Texture2D));
		rightPushedText = (Texture2D) Resources.Load("Images/right_cursor_grip", typeof(Texture2D));
		
	}
	
	// Update is called once per frame
	void Update ()
	{		
		//update all of the bones positions
		if (sw.pollSkeleton ()) 
		{			
			//Head management index=3
			Vector3 headPos = new Vector3 (sw.bonePos [0, 3].x, sw.bonePos [0, 3].y, sw.bonePos [0, 3].z);			
			
			//LeftHand management index=7
			Vector3 leftPos = new Vector3 (sw.bonePos [0, 7].x,	sw.bonePos [0, 7].y,	sw.bonePos [0, 7].z);			
			
			//RightHand management index=11
			Vector3 rightPos = new Vector3 (sw.bonePos [0, 11].x, sw.bonePos [0, 11].y, sw.bonePos [0, 11].z); 
			
			
			float x = (rightPos.x - headPos.x)+ 0.35f;
			float y =  0.75f - (headPos.y - rightPos.y);
			rightCursor.transform.position = new Vector3(x, y, rightCursor.transform.position.z);
			rightHand = new Vector3(x, y, rightPos.z - headPos.z);
			
			x = (leftPos.x - headPos.x)+ 0.65f;
			y =  0.75f - (headPos.y - leftPos.y);
			leftCursor.transform.position = new Vector3(x, y, leftCursor.transform.position.z);
			leftHand = new Vector3(x, y, leftPos.z - headPos.z);
			
			if(leftHand.z > CLICK_Z && leftCursor.texture.Equals(leftText))
				leftCursor.texture = leftPushedText;
			else 
				if(leftHand.z < CLICK_Z && leftCursor.texture.Equals(leftPushedText))
					leftCursor.texture = leftText;
			
			if(rightHand.z > CLICK_Z && rightCursor.texture.Equals(rightText))
				rightCursor.texture = rightPushedText;
			else 
				if(rightHand.z < CLICK_Z && rightCursor.texture.Equals(rightPushedText))
					rightCursor.texture = rightText;
			
			
			
		}
		
	}
	
	public Vector3 getRightHand()
	{
		return rightHand;	
	}
	
	public Vector3 getLeftHand()
	{
		return leftHand;	
	}
	
	public void moveKinect(float yDiff)
	{
		KinectSensor ks = (KinectSensor) FindObjectOfType(typeof(KinectSensor));
		float y = ks.lookAt.y;
		y += yDiff;
		if(y>0 && y <2)
			ks.setKinectAngle(new Vector3(0f, y, 0f));
	}
}

