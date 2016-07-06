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

    public UnityEngine.UI.Image ProgressBar;

    public float ProgressBarSpeed;

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
        RectTransform rect = ProgressBar.GetComponent<RectTransform>();
        float scale = 0;
        while (scale < Screen.width) {
            ++count;
            if(asyncOp.progress < 0.9f) {
                scale = Mathf.Min(Screen.width * asyncOp.progress, scale + ProgressBarSpeed * Time.deltaTime);
            } else {
                scale = scale + ProgressBarSpeed * Time.deltaTime;
            }
            
            rect.sizeDelta = new Vector2(scale, 0);
            
            yield return null;
        }
        // Load end animation
        //Debug.Log("Progress " + asyncOp.progress + " * " + Screen.width + " = " + Screen.width * asyncOp.progress);
        //Debug.Log("Loading took " + count + " frames");
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
        if (myScript.UseButton) {
            myScript.ButtonName = EditorGUILayout.TextField("Button Name", myScript.ButtonName);
        }
        myScript.ProgressBar = (UnityEngine.UI.Image)EditorGUILayout.ObjectField("Progress Bar", myScript.ProgressBar, typeof(UnityEngine.UI.Image), true);
        myScript.ProgressBarSpeed = EditorGUILayout.FloatField("Progress Bar Speed", myScript.ProgressBarSpeed);
    }
}
#endif
