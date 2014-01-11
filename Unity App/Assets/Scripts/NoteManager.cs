using UnityEngine;
using System.Collections;

public class NoteManager : MonoBehaviour 
{
	public GameObject startPoint;
	public GameObject notePrefab;
	public GameObject toucheObject;
	
	//Hauteur de la touche
	public float toucheHeight = 0.7f;
	
	//Particule des notes jouées
	public GameObject particle;
	protected GameObject newParticle = null;
	
	//déterminant de la piste (A, B ou C)
	protected Note.Which piste;	
	//liste des notes à jouer
	protected ArrayList notesListe;	
	//référence du Main manager
	protected MainManager mainManager;	
	//référence du Kinect controller
	protected KinectGameController kinectController;	
	//temps du lancement de la piste
	protected float beginning;	
	//si la piste à commencer
	protected bool initialized;	
	
	//temps de la prochaine note
	protected float nextNoteTime;
	//temps de la dernière note
	protected float previousNoteEnd;
	//si une note peut etre jouée (bras baissé)
	protected bool valide;
	//si une note est actellement jouée
	protected bool isActive;
	//temps de début de la note
	protected float noteStart;
	//si une note a été jouée par le joueur
	protected bool played;
	//si une note a été ratée par le joueur
	protected bool missed;
	//si le son d'erreur a été joué
	protected bool soundPlayed;
	
	protected ArrayList noteObjects;
	
	protected Color originalColor;
	
	
	void Start () 
	{
		this.guiTexture.enabled = false;
		this.kinectController = (KinectGameController) FindObjectOfType(typeof(KinectGameController));		
	}
		
	public void init(ArrayList liste, Note.Which piste, MainManager m)
	{	
		this.newParticle = null;
		this.beginning = 0;
		this.initialized = false;
		this.noteStart = 0;
		this.noteObjects = new ArrayList();
		this.isActive = false;
		this.nextNoteTime = 0;
		this.previousNoteEnd = 0;
		this.soundPlayed = false;
		this.mainManager = m;
		this.piste = piste;
		this.notesListe = liste;
		this.beginning = Time.timeSinceLevelLoad;
		this.initialized = true;
		StartCoroutine(playNotes());
		StartCoroutine(renderNotes());
		this.toucheObject.renderer.enabled = true;
	}
	
	public void disableTouch()
	{	
		this.toucheObject.renderer.enabled = false;
	}
	
	void Update () 
	{
		//si la musique a commencée
		if(this.initialized)
		{
			//gestion de la note jouée par le joueur
			if(isNotePlayed())
			{
				toucheObject.transform.position = new Vector3(toucheObject.transform.position.x, toucheHeight - 0.2f, toucheObject.transform.position.z);	
				toucheObject.transform.renderer.material.color = new Color(76f/255,111f/255,240f/255,255f/255);
				if(this.played && newParticle == null)
					newParticle = (GameObject) Instantiate(particle, toucheObject.transform.position, toucheObject.transform.rotation);
			}
			else
			{
				toucheObject.transform.position = new Vector3(toucheObject.transform.position.x, toucheHeight, toucheObject.transform.position.z);
				toucheObject.transform.renderer.material.color = new Color(39f/255,45f/255,255f/255,255f/255);	
				if(newParticle != null)
					Destroy(newParticle);
			}
			
			
			if(!this.isActive)
			{			
				if(isNotePlayed())
				{
					// si la note est jouée trop tot
					if(getTime()< this.nextNoteTime - this.mainManager.timeBeforeNote && getTime() > this.previousNoteEnd + this.mainManager.timeAfterNote)
					{			
						if(!this.soundPlayed)
						{
							if(this.mainManager.fail_sound)
								MusicManager.PlayFail();
							this.soundPlayed = true;
						}
						
						this.valide = false;
						this.mainManager.resetCombo();
						this.played = false;
						if(newParticle != null)
							Destroy(newParticle);
					}
				}
				else
				{					
					this.valide = true;
					this.soundPlayed = false;
				}
			}//si une note est actuellement jouée
			else
			{
				// si la note est jouée à temps par le joueur	
				if(getTime()-this.noteStart < this.mainManager.reflexTime)
				{
					if(isNotePlayed())
					{						
						if(this.valide && !this.played)
						{
							this.played = true;	
							setCurrentNoteColor(Color.green);
							this.mainManager.addNote(true);
						}
						else
						{						
							this.mainManager.addPoints(1); 
						}
					}
				}
				else
				{
					if(!played && !missed)
					{
						this.missed = true;
						setCurrentNoteColor(Color.red);
						this.mainManager.addNote(false);
						if(this.mainManager.fail_sound)
							MusicManager.PlayFail();	
					}
					else
						if(played && !missed)
						{	
							if(isNotePlayed())
							{
								this.mainManager.addPoints(1);
							}
							else
							{								
								setCurrentNoteColor(this.originalColor);
								this.missed = true;
							}
						}
				}
			}
		}
	}
	
	protected bool isNotePlayed()
	{
		return (this.kinectController.noteGauche == this.piste || this.kinectController.noteDroite == this.piste || this.kinectController.noteGenous == this.piste);
	}
	
	protected void setCurrentNoteColor(Color c)
	{
		if(this.noteObjects.Count > 0)
		{
			GameObject noteObject = (GameObject) this.noteObjects[0];			
			this.originalColor = noteObject.transform.renderer.material.color;
			noteObject.transform.renderer.material.color = c;
		}	
	}
	
	IEnumerator playNotes() 
	{		
		foreach(Note note in this.notesListe)
		{	// joue puis attend la fin de la note					
			yield return StartCoroutine(playNote(note.start/1000f, note.length/1000f));
		}
        yield return null;
        
    }
	
	IEnumerator playNote(float start, float length) 
	{		
		//gestion du temps de validité de la note (avant de pouvoir la jouer)
		this.previousNoteEnd = getTime();
		this.nextNoteTime = this.mainManager.timeToNote + start;
		this.valide = true;
		//attente avant de jouer la prochaine note
		yield return new WaitForSeconds(this.mainManager.timeToNote + start-getTime());
		
		//la note est jouée
		this.isActive = true;
		this.noteStart = getTime();
		this.played = false;
		this.missed = false;
		
		if(this.mainManager.debug)
			transform.guiTexture.enabled = true;
		
		//attente avant d'arreter la note
		yield return new WaitForSeconds(length);
		
		//la note est finie
		if(this.mainManager.debug)
			transform.guiTexture.enabled = false;
		this.isActive = false;
		if(this.noteObjects.Count > 0)
			this.noteObjects.RemoveAt(0);
		
        yield return null;        
    }	
	IEnumerator renderNotes() 
	{		
		foreach(Note note in this.notesListe)
		{	// joue puis attend la fin de la note					
			yield return StartCoroutine(renderNote(note.start/1000f, note.length/1000f));
		}
        yield return null;
        
    }
	
	IEnumerator renderNote(float start, float length) 
	{		
		//attente avant de jouer la prochaine note
		yield return new WaitForSeconds(start-getTime());
		
		//on calcule la longueur de la note et son decalage de position pour que le bord inferieur soit au début de la piste
		float z =  1f * (length/0.2f);
		notePrefab.transform.localScale = new Vector3(notePrefab.transform.localScale.x, notePrefab.transform.localScale.y, 1f * (length/0.2f));
		Vector3 startPosition = new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, startPoint.transform.position.z +(z-1)/2);
		GameObject obj = (GameObject) Instantiate(notePrefab, startPosition, startPoint.transform.rotation);
		
		this.noteObjects.Add(obj);
		//on prevoit la destruction de l'objet 3 secondes après la fin de sa note
		Destroy(obj, length + 3f);
		
		//attente avant d'arreter la note
		yield return new WaitForSeconds(length);
		
        yield return null;        
    }
	
	protected float getTime()
	{
		return (Time.timeSinceLevelLoad-this.beginning);
	}
	
}
