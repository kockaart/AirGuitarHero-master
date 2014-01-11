using UnityEngine;
using System;
using System.IO;

public class GUIManager : MonoBehaviour 
{		
	public GUIText debugLabel;
	public float x = 16;
	
	public GUIText comboLabel;
	public GUIText countdownLabel;
	public GUIText multiplicateurlabel;
	public GUIText pointsLabel;
	public GUIText pourcentslabel;
	public GUIText timeLabel;
	
	protected GameObject screen;
	
    private static GUIManager instance = null;

    void Awake() {
        instance = this;
		instance.comboLabel.text = "Combo: 0";
		instance.multiplicateurlabel.text = "Multiplicateur: 1";
		instance.pointsLabel.text = "Points: 0";
		instance.pourcentslabel.text = "100%";
		instance.debugLabel.enabled = false;
		
		//feedback management (adjust to screen)
		instance.screen = GameObject.Find("Screen");
		instance.screen.transform.position = new Vector3(0, -0.0475f*Screen.height, 0.05f*Screen.height);
		instance.screen.transform.localScale = new Vector3(Screen.width/100f, 1, Screen.height/100f);
		
		//adjust label size to screen
		instance.timeLabel.fontSize = (int) Math.Round(Screen.width / 28d);
		instance.comboLabel.fontSize = (int) Math.Round(Screen.width / 28d);
		instance.multiplicateurlabel.fontSize = (int) Math.Round(Screen.width / 28d);;
		instance.pointsLabel.fontSize = (int) Math.Round(Screen.width / 28d);
		instance.pourcentslabel.fontSize = (int) Math.Round(Screen.width / 28d);
    }
	
	public static void SetTime(float current, float total)
	{
		instance.timeLabel.text = String.Format("{0:00}:{1:00}",Math.Truncate(current/60f), Math.Truncate(current%60f))+" / "+String.Format("{0:00}:{1:00}",Math.Truncate(total/60f),Math.Truncate(total%60f));
	}
	
	public static void SetCountdown(int count)
	{
		instance.countdownLabel.text = count.ToString();
	}
	public static void SetCountdown(bool active)
	{
		instance.countdownLabel.enabled = active;
	}
	 
	public static void SetPoints(int points)
	{
		instance.pointsLabel.text = "Points: "+points;
	}
	 
	public static void SetCombo(int combo)
	{
		instance.comboLabel.text = "Combo: "+combo;
	}
	 
	public static void SetMultiplicateur(int multi)
	{
		instance.multiplicateurlabel.text = "Multiplicateur: "+multi;
	}
	
	public static void SetPourcentage(int pourcents)
	{
		instance.pourcentslabel.text = pourcents+"%";
	}
	
	public static void SetDebug(String text)
	{
		instance.debugLabel.enabled = true;
		instance.debugLabel.text = text;
	}

}
