using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

public class SceneLoader : MonoBehaviour {
    public int CurrentSceneIndex;
    public int NextSceneIndex;

    public bool UseButton = false;
    public string ButtonName = "Submit";

    public GameObject SceneToHide;
    public Canvas Canvas;
    public UnityEngine.UI.Image ProgressBarLeft;
    public UnityEngine.UI.Image ProgressBarRight;

    public float ProgressBarSpeed;

    public delegate void LoadEventHandler();
    public event LoadEventHandler LoadStarted;
    public event LoadEventHandler LoadEnded;

    void Awake() {
        gameObject.tag = "SceneLoader";
        //progressBar = GetComponent<SpriteRenderer>();
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

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            LoadScene();
        }
    }

    public void LoadScene() {        
        StartCoroutine(LoadSceneAsync());
    }
    private IEnumerator LoadSceneAsync() {
        // Trigger load end event
        if (LoadStarted != null) {
            LoadStarted();
        }
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(NextSceneIndex, LoadSceneMode.Additive);
        // Wait to activate scene until transition animations are done
        asyncOp.allowSceneActivation = false;
        // Save reference to source scene main camera
        Camera originCam = Camera.main;
        originCam.tag = "Untagged";
        // Progress animation
        RectTransform rectLeft = ProgressBarLeft.GetComponent<RectTransform>();
        RectTransform rectRight = ProgressBarRight.GetComponent<RectTransform>();
        float scale = 0;
        float halfWidth = Screen.width / 2.0f;
        while (scale < halfWidth) {
            scale = scale + ProgressBarSpeed * Time.deltaTime;
            rectLeft.sizeDelta = new Vector2(scale, Screen.height);
            rectRight.sizeDelta = new Vector2(scale, Screen.height);
            yield return null;
        }
        // Allow scene activation
        asyncOp.allowSceneActivation = true;
        SceneToHide.SetActive(false);
        yield return null;
        // Trigger load end event
        if (LoadEnded != null) {
            LoadEnded();
        }
        // Load end animation
        Camera destCam = Camera.main;
        // Disable origin camera
        originCam.gameObject.SetActive(false);
        // Change pvot points
        float offset = 0;
        while (offset < halfWidth) {
            offset = offset + ProgressBarSpeed * Time.deltaTime;
            rectLeft.localPosition = new Vector3(-offset, 0, 0);
            rectRight.localPosition = new Vector3(offset, 0, 0);
            yield return null;
        }
        SceneManager.UnloadScene(CurrentSceneIndex);
    }
    //private IEnumerator LoadSceneAsync() {
    //    // Trigger load end event
    //    if(LoadStarted != null) {
    //        LoadStarted();
    //    }        
    //    AsyncOperation asyncOp = SceneManager.LoadSceneAsync(NextSceneIndex, LoadSceneMode.Additive);
    //    // Wait to activate scene until transition animations are done
    //    asyncOp.allowSceneActivation = false;

    //    Camera originCam = Camera.main;
    //    originCam.tag = "Untagged";

    //    // Progress animation        
    //    float screenHeight = originCam.orthographicSize * 2;
    //    float screenWidth = originCam.orthographicSize * 2 * originCam.aspect;
    //    progressBar.transform.position = new Vector3(originCam.transform.position.x - screenWidth / 2.0f, originCam.transform.position.y, 0);
    //    progressBar.transform.localScale = new Vector3(progressBar.transform.localScale.x, screenHeight, 1);
    //    float scale = 0;
    //    while (scale < screenWidth) {
    //        if(asyncOp.progress < 0.9f) {
    //            scale = Mathf.Min(screenWidth * asyncOp.progress, scale + ProgressBarSpeed * Time.deltaTime);
    //        } else {
    //            scale = scale + ProgressBarSpeed * Time.deltaTime;
    //        }
    //        // Change size of progress bar
    //        progressBar.transform.localScale = new Vector3(scale, progressBar.transform.localScale.y, 1);
    //        yield return null;
    //    }
    //    // Allow scene activation
    //    asyncOp.allowSceneActivation = true;
    //    yield return null;
    //    // Disable camera
    //    originCam.gameObject.SetActive(false);
    //    // Adjust progress bar to new camera
    //    Camera destCam = Camera.main;
    //    screenHeight = destCam.orthographicSize * 2;
    //    screenWidth = destCam.orthographicSize * 2 * destCam.aspect;
    //    progressBar.transform.position = new Vector3(destCam.transform.position.x - screenWidth / 2.0f, destCam.transform.position.y, 0);
    //    progressBar.transform.localScale = new Vector3(screenWidth, screenHeight, 1);



    //    // Trigger load end event
    //    if (LoadEnded != null) {
    //        LoadEnded();
    //    }
    //    // Load end animation
    //    float offset = ProgressBarSpeed * Time.deltaTime;
    //    float accumulated = 0;
    //    while (accumulated < screenWidth) {
    //        accumulated += offset;
    //        screenHeight = destCam.orthographicSize * 2;
    //        screenWidth = destCam.orthographicSize * 2 * destCam.aspect;
    //        progressBar.transform.position = new Vector3(destCam.transform.position.x - screenWidth / 2.0f, destCam.transform.position.y, 0);
    //        progressBar.transform.localScale = new Vector3(screenWidth, screenHeight, 1);
    //        progressBar.transform.position += (Vector3.right * accumulated);
    //        yield return null;
    //    }
    //    SceneManager.UnloadScene(CurrentSceneIndex);       
    //}
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
