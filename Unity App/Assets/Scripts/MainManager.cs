using UnityEngine;
using System;
using System.IO;
using System.Collections;
using MiniJSON;

public class MainManager : MonoBehaviour 
{	
	//PARAMETRES
	public bool clavier = false;
	public float timeToNote = 2f;
	public float reflexTime = 0.1f;
	public float timeAfterNote = 0.3f;
	public float timeBeforeNote = 0.1f;
	public int countdown = 3;
	public bool debug = false;
	public float decalageMusique = 0.4f;
	public bool fail_sound = true;
	
	public GUITexture AreaA;
	public GUITexture AreaB;
	public GUITexture AreaC;
	public GUITexture AreaD;
	public GUITexture AreaE;
	public GUITexture AreaF;
	
	protected ArrayList notesA, notesB, notesC, notesD, notesE, notesF;
	protected bool started;
	protected bool countdownOver;
	protected float beginning;	
	protected float beginning_song;	
	protected float song_length;	
	
	protected int points;
	
	protected int combo;
	
	protected int multiplicateur;
	protected bool bonusOn;
	
	protected float notesReussies;
	protected float notesTotales;
	
	protected bool direct_start;
	
	public TextAsset jsonFile1;
	public AudioClip music1;
	public TextAsset jsonFile2;
	public AudioClip music2;
	public TextAsset jsonFile3;
	public AudioClip music3;
	
	protected TextAsset jsonFile;
	protected AudioClip music;
	
	protected bool hasLaterals;
	protected bool hasKnees;
	
	protected KinectGameController kinectController;
	
	
	// Use this for initialization
	void Start () 
	{	
		MusicManager.StopMusic();
		this.direct_start = true;
		this.started = false;
		this.countdownOver = false;
		this.beginning = Time.time;
		this.beginning_song = 0;
		points = 0;
		combo = 0;
		multiplicateur = 1;
		notesTotales = notesReussies = 0;
		bonusOn = false;
		hasLaterals = false;
		hasKnees = false;
		
		this.kinectController = (KinectGameController) FindObjectOfType(typeof(KinectGameController));
		
		Hashtable h =  SceneManager.GetSceneArguments();
		if( h!=null)
		{
			int whichMusic = (int) h["level"];
			if(whichMusic == 1)
			{				
				jsonFile = jsonFile1;
				music = music1;
			}
			else if(whichMusic == 2)
			{
				jsonFile = jsonFile2;
				music = music2;
			}
			else{
				jsonFile = jsonFile3;
				music = music3;
			}
		}
		else
		{
			jsonFile = jsonFile1;
			music = music1;
		}
				
		initializeFromJSON();
		if(direct_start)
			StartPistes();
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if(!this.started)
		{
			if(this.countdownOver)
			{
				this.started = true;
				if(!direct_start)
					StartPistes();
			}
			else
			{//gestion du countdown
				if((Time.time-this.beginning) > countdown)
				{
					GUIManager.SetCountdown(false);
					this.countdownOver = true;
				}
				else
				{
					GUIManager.SetCountdown(countdown - (int)(Time.time-this.beginning));
				}
			}
			
		}
		if(this.beginning_song != 0)
			GUIManager.SetTime(Time.time - this.beginning_song, this.song_length);
		
		if(this.kinectController.bonusActivated){
			this.bonusOn = true;
			setMultiplieur();
		}
		
		//Fin de la partie
		if(this.beginning_song != 0){
			if(Time.time > (this.beginning_song + this.song_length + 5f) || Input.GetKey(KeyCode.Escape))
				Application.LoadLevel("MainMenu");
		}
	}	
	
	private void StartPistes () 
	{		
		AreaA.GetComponent<NoteManager>().init(notesA, Note.Which.A, this);
		AreaB.GetComponent<NoteManager>().init(notesB, Note.Which.B, this);
		AreaC.GetComponent<NoteManager>().init(notesC, Note.Which.C, this);
		if(hasKnees)
			AreaD.GetComponent<NoteManager>().init(notesD, Note.Which.D, this);
		if(hasLaterals)
		{
			AreaE.GetComponent<NoteManager>().init(notesE, Note.Which.E, this);
			AreaF.GetComponent<NoteManager>().init(notesF, Note.Which.F, this);
		}
		else {			
			AreaE.GetComponent<NoteManager>().disableTouch();
			AreaF.GetComponent<NoteManager>().disableTouch();
		}
		StartCoroutine("StartSong");
	}	
	
	private IEnumerator StartSong () 
	{
		//decalage musique		
		yield return new WaitForSeconds(this.timeToNote + this.decalageMusique);
		MusicManager.SetMusic(music);
		beginning_song = Time.time;
		song_length = MusicManager.GetMusicLength();
	}
	
	private void initializeFromJSON()
	{
		//lecture du fichier contenant les notes
		String json = jsonFile.text;
		
		//parsing du JSON
		IDictionary search = (IDictionary)Json.Deserialize (json);
		
		// récupération des notes de la piste A
		IList aa = (IList)search ["A"];
		notesA = new ArrayList();
		foreach (IDictionary note in aa) {
			notesA.Add(new Note(Int32.Parse(note ["start"].ToString()), Int32.Parse(note ["length"].ToString())));
		}		
		
		// récupération des notes de la piste B
		IList bb = (IList)search ["B"];
		notesB = new ArrayList();
		foreach (IDictionary note in bb) {
			notesB.Add(new Note(Int32.Parse(note ["start"].ToString()), Int32.Parse(note ["length"].ToString())));
		}		
		
		// récupération des notes de la piste C
		IList cc = (IList)search ["C"];
		notesC = new ArrayList();
		foreach (IDictionary note in cc) {
			notesC.Add(new Note(Int32.Parse(note ["start"].ToString()), Int32.Parse(note ["length"].ToString())));
		}		
		
		// récupération des notes de la piste D
		IList dd = (IList)search ["D"];
		notesD = new ArrayList();
		if(dd != null)
		{
			foreach (IDictionary note in dd) {
				notesD.Add(new Note(Int32.Parse(note ["start"].ToString()), Int32.Parse(note ["length"].ToString())));
			}
			hasKnees = true;
		}	
		
		// récupération des notes de la piste E
		IList ee = (IList)search ["E"];
		notesE = new ArrayList();
		if(ee != null)
		{
			foreach (IDictionary note in ee) {
				notesE.Add(new Note(Int32.Parse(note ["start"].ToString()), Int32.Parse(note ["length"].ToString())));
			}
			hasLaterals = true;
		}	
		
		// récupération des notes de la piste E
		IList ff = (IList)search ["F"];
		notesF = new ArrayList();
		if(ff != null)
		{
			foreach (IDictionary note in ff) {
				notesF.Add(new Note(Int32.Parse(note ["start"].ToString()), Int32.Parse(note ["length"].ToString())));
			}
			hasLaterals = true;
		}
		
		if(search.Contains("direct_start"))
		{
			direct_start = (bool) search["direct_start"];
		}
		
	}
	
	public void addNote(bool reussie)
	{
		notesTotales++;
		if(reussie)
		{
			notesReussies++;
			addCombo();
			addPoints(100);
		}
		else
			resetCombo();
		GUIManager.SetPourcentage((int)(notesReussies/notesTotales*100f));
	}
	
	public void addPoints(int points)
	{
		this.points += (points*multiplicateur);		
		GUIManager.SetPoints(this.points);
	}
	
	public void addCombo()
	{
		this.combo++;	
		GUIManager.SetCombo(this.combo);
		setMultiplieur();
	}
	
	public void resetCombo()
	{
		this.bonusOn =false;
		this.combo=0;	
		GUIManager.SetCombo(this.combo);
		setMultiplieur();
	}
	
	protected void setMultiplieur()
	{
		if(combo < 10)
			this.multiplicateur = 1;
		else if(combo < 30)
			this.multiplicateur = 2;
		else if(combo < 50)
			this.multiplicateur = 4;
		else
			this.multiplicateur = 8;
		if(this.multiplicateur > 2 && bonusOn)
			this.multiplicateur = 16;
		GUIManager.SetMultiplicateur(this.multiplicateur);
	}
	
	public bool HasLaterals()
	{
		return hasLaterals;	
	}
	
	public bool HasKnees()
	{
		return hasKnees;	
	}
}
