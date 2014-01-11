using UnityEngine;
using System.Collections;

public static class SceneManager {

	private static Hashtable sceneArguments;
	
	public static void LoadScene(string sceneName, Hashtable arguments)
    {
        sceneArguments = arguments;
        Application.LoadLevel(sceneName);
    }    

    public static Hashtable GetSceneArguments()
    {        
		return sceneArguments;
    }

}
