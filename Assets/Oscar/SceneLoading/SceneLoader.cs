using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoader : MonoBehaviour {
    //public int CurrentSceneIndex;
    public int NextSceneIndex;

    public bool UseButton = false;
    public string ButtonName = "Submit";

    public delegate void LoadEventHandler();
    public event LoadEventHandler LoadStarted;
    public event LoadEventHandler LoadEnded;

    void Awake() {
        gameObject.tag = "SceneLoader";
    }

    void Update() {
        try {
            if (UseButton && Input.GetButtonDown(ButtonName)) {
                LoadScene();
            }
        } catch(System.Exception e) {
            Debug.LogWarning(ButtonName + " doesn't exist");
        }        
    }

    public void LoadScene() {        
        StartCoroutine(LoadSceneAsync());
    }
    private IEnumerator LoadSceneAsync() {
        // Trigger load end event
        if(LoadStarted != null) {
            LoadStarted();
        }        
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(NextSceneIndex, LoadSceneMode.Single);
        // Wait to activate scene until transition animations are done
        asyncOp.allowSceneActivation = false;
        // Progress animation
        int count = 0;
        while (asyncOp.progress < 0.9f) {
            ++count;
            yield return null;
        }
        // Load end animation
        Debug.Log("Loading took " + count + " frames");
        // Allow scene activation
        asyncOp.allowSceneActivation = true;
        // Trigger load end event
        if(LoadEnded != null) {
            LoadEnded();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderEditor : Editor {
    public override void OnInspectorGUI() {
        SceneLoader myScript = (SceneLoader)target;
        //myScript.CurrentSceneIndex = EditorGUILayout.IntField("Current Scene Index", myScript.CurrentSceneIndex);
        myScript.NextSceneIndex = EditorGUILayout.IntField("Next Scene Index", myScript.NextSceneIndex);
        myScript.UseButton = GUILayout.Toggle(myScript.UseButton, "Use Button");
        if(myScript.UseButton) {
            myScript.ButtonName = EditorGUILayout.TextField("Button Name", myScript.ButtonName);
        }        
    }
}
#endif
