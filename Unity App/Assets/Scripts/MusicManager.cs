using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{	
	protected AudioSource mainAudio;
	protected AudioSource failAudio;
	
    private static MusicManager instance = null;
	
    public static MusicManager Instance {
        get { return instance; }
    }
	
    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);			
			instance.transform.position = Camera.main.transform.position;
            return;
        } else {
            instance = this;
			AudioSource[] sources = (AudioSource[]) GetComponents<AudioSource>();
			if(sources.Length >= 2)
			{
				if(sources[0].priority == 128) {
					instance.mainAudio = sources[0];
					instance.failAudio = sources[1];
				}
				else{					
					instance.mainAudio = sources[1];
					instance.failAudio = sources[0];
				}
			}
			MusicManager.SetVolume(0.5f);			
			instance.transform.position = Camera.main.transform.position;
        }
        DontDestroyOnLoad(this.gameObject);
    }
	
	private static float volume = 0.5f;
	private static AudioClip music;
	
	public static void SetVolume(float v){
		volume = v;
		instance.mainAudio.volume = v;
		instance.failAudio.volume = v;
	}
	
	public static float GetVolume(){
		return volume;	
	}
	
	public static void SetMusic(AudioClip m){
		music = m;
		instance.mainAudio.clip = m;
		instance.mainAudio.Play();
	}
	
	public static void PlayMusic(){
		instance.mainAudio.Play();
	}
	
	public static void StopMusic(){
		instance.mainAudio.Stop();
	}
	public static float GetMusicLength(){
		return instance.mainAudio.clip.length;
	}
	
	public static AudioClip GetMusic(){
		return music;
	}
	
	public static void PlayFail(){
		instance.failAudio.Play();
	}
	
	
 

}
