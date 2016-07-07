using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

[RequireComponent(typeof(SpriteRenderer))]
public class SceneLoader : MonoBehaviour {
    //public int CurrentSceneIndex;
    public int NextSceneIndex;

    public bool UseButton = false;
    public string ButtonName = "Submit";

    private SpriteRenderer progressBar;

    public float ProgressBarSpeed;

    public delegate void LoadEventHandler();
    public event LoadEventHandler LoadStarted;
    public event LoadEventHandler LoadEnded;

    void Awake() {
        gameObject.tag = "SceneLoader";
        progressBar = GetComponent<SpriteRenderer>();
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
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        progressBar.transform.position = new Vector3(Camera.main.transform.position.x - screenWidth / 2.0f, Camera.main.transform.position.y, 0);
        progressBar.transform.localScale = new Vector3(progressBar.transform.localScale.x, screenHeight, 1);
        float scale = 0;
        while (scale < screenWidth) {
            if(asyncOp.progress < 0.9f) {
                scale = Mathf.Min(screenWidth * asyncOp.progress, scale + ProgressBarSpeed * Time.deltaTime);
            } else {
                scale = scale + ProgressBarSpeed * Time.deltaTime;
            }
            // Change size of progress bar
            progressBar.transform.localScale = new Vector3(scale, progressBar.transform.localScale.y, 1);
            yield return null;
        }
        // Load end animation
        //float offset = ProgressBarSpeed * Time.deltaTime;
        //float accumulated = 0;
        //while (offset < screenWidth) {
        //    accumulated += offset;
        //    progressBar.transform.position += (Vector3.right * offset);
        //    yield return null;
        //}

        // Trigger load end event
        if (LoadEnded != null) {
            LoadEnded();
        }
        // Allow scene activation
        asyncOp.allowSceneActivation = true;
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(SceneLoader))]
//public class SceneLoaderEditor : Editor {
//    public override void OnInspectorGUI() {
//        SceneLoader myScript = (SceneLoader)target;
//        //myScript.CurrentSceneIndex = EditorGUILayout.IntField("Current Scene Index", myScript.CurrentSceneIndex);
//        myScript.NextSceneIndex = EditorGUILayout.IntField("Next Scene Index", myScript.NextSceneIndex);
//        myScript.UseButton = GUILayout.Toggle(myScript.UseButton, "Use Button");
//        if (myScript.UseButton) {
//            myScript.ButtonName = EditorGUILayout.TextField("Button Name", myScript.ButtonName);
//        }
//        myScript.ProgressBarSpeed = EditorGUILayout.FloatField("Progress Bar Speed", myScript.ProgressBarSpeed);
//    }
//}
//#endif
