using UnityEngine;
using System.Collections;

public class Gates : MonoBehaviour {
    public Transform Left;
    public Transform Right;

    public Vector3 CloseTarget;

    private Vector3 openTargetLeft;
    private Vector3 openTargetRight;

    private BoxCollider2D leftCollider;
    private BoxCollider2D rightCollider;

    private Vector3 closedTargetLeft;
    private Vector3 closedTargetRight;

    private BoxCollider2D closeTrigger;

    private enum GateState {
        Open,
        Closed,
        Midway
    }
    private GateState state;

    public float Speed;

    // Debugging
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(state == GateState.Open) {
                Close();
            } else if(state == GateState.Closed) {
                Open();
            }            
        }
    }

    void Start () {
        closeTrigger = GetComponent<BoxCollider2D>();
        leftCollider = Left.GetComponent<BoxCollider2D>();
        rightCollider = Right.GetComponent<BoxCollider2D>();
        // Save left and right side starting positions
        openTargetLeft = Left.localPosition;
        openTargetRight = Right.localPosition;
        // Calculate right and left close targets
        closedTargetLeft = (Vector3)(Left.worldToLocalMatrix * CloseTarget) - (leftCollider.bounds.size.x * Vector3.right / 2.0f);
        closedTargetRight = (Vector3)(Right.worldToLocalMatrix * CloseTarget) + (rightCollider.bounds.size.x * Vector3.right / 2.0f);
        state = GateState.Open;
       // Close();
    }    
	
	void OnTriggerEnter2D(Collider2D col) {
		if(col.gameObject.tag == "Player")
        	Close();
    }

    public void Close() {
        if(state == GateState.Open) {
            //Left.localPosition = leftCloseTarget;
            //Right.localPosition = rightCloseTarget;
            //open = false;
            StartCoroutine(CloseAnim());  

        }
    }
    private IEnumerator CloseAnim() {
        state = GateState.Midway;
        float leftDist = Vector3.Distance(Left.localPosition, closedTargetLeft);
        float rightDist = Vector3.Distance(Right.localPosition, closedTargetRight);
        bool leftDone = leftDist < Time.deltaTime * Speed;
        bool rightDone = rightDist < Time.deltaTime * Speed;

        Vector3 leftDir = closedTargetLeft - openTargetLeft;
        leftDir.Normalize();
        Vector3 rightDir = closedTargetRight - openTargetRight;
        rightDir.Normalize();
        while (!leftDone || !rightDone) {
            // Move towards targets
            if(!leftDone) {
                Left.localPosition += (leftDir * Speed * Time.deltaTime);
            }
            if(!rightDone) {
                Right.localPosition += (rightDir * Speed * Time.deltaTime);
            }           
            // Update remaining distances
            leftDist = Vector3.Distance(Left.localPosition, closedTargetLeft);
            rightDist = Vector3.Distance(Right.localPosition, closedTargetRight);
            leftDone = leftDist < Time.deltaTime * Speed;
            rightDone = rightDist < Time.deltaTime * Speed;
            yield return null;
        }
        // Ensure fully closed
        Left.localPosition = closedTargetLeft;
        Right.localPosition = closedTargetRight;
        state = GateState.Closed;
    }

    public void Open() {
        if(state == GateState.Closed) {
            //Left.localPosition = openTargetLeft;
            //Right.localPosition = openTargetRight;
            //open = true;
            StartCoroutine(OpenAnim());

        }

    }

    private IEnumerator OpenAnim() {
        state = GateState.Midway;
        float leftDist = Vector3.Distance(Left.localPosition, openTargetLeft);
        float rightDist = Vector3.Distance(Right.localPosition, openTargetRight);
        bool leftDone = leftDist < Time.deltaTime * Speed;
        bool rightDone = rightDist < Time.deltaTime * Speed;

        Vector3 leftDir = openTargetLeft - closedTargetLeft;
        leftDir.Normalize();
        Vector3 rightDir = openTargetRight - closedTargetRight;
        rightDir.Normalize();
        while (!leftDone || !rightDone) {
            // Move towards targets
            if (!leftDone) {
                Left.localPosition += (leftDir * Speed * Time.deltaTime);
            }
            if (!rightDone) {
                Right.localPosition += (rightDir * Speed * Time.deltaTime);
            }
            // Update remaining distances
            leftDist = Vector3.Distance(Left.localPosition, openTargetLeft);
            rightDist = Vector3.Distance(Right.localPosition, openTargetRight);
            leftDone = leftDist < Time.deltaTime * Speed;
            rightDone = rightDist < Time.deltaTime * Speed;
            yield return null;
        }
        // Ensure fully open
        Left.localPosition = openTargetLeft;
        Right.localPosition = openTargetRight;
        state = GateState.Open;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CloseTarget, 0.2f);
    }
}
