using System.Collections;
using UnityEngine;

/*
 * This script handles the movement of the camera when the blocks get too high
 */
public class HeightCheck : MonoBehaviour
{
    [SerializeField] private float timeToMoveCamera = 2f;
    [SerializeField] private float moveCameraDelay = 0.6f;
    
    private Camera _cam;
    private Collider2D _triggerbox;
    private float _timer;
    void Start()
    {
        _cam = GetComponentInParent<Camera>();
        _triggerbox = GetComponent<Collider2D>();
        _triggerbox.isTrigger = true;
    }
    
    void Update()
    {
        Debug.Log("timer: " + _timer);
        // used chatgpt to help me figure out how to get the center of the camera and move the camera up.
        // https://chat.openai.com/share/827b1601-9ce0-42bb-a25d-6516f945b19f
        
        // get center of the camera
        Vector3 center = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _cam.nearClipPlane));
        
        // if this is true the blocks are too high and the camera should move up.
        // has a value of 0.6f because blocks take a shorter time than that to move through the trigger.
        if (_timer > moveCameraDelay)
        {
            _timer = 0f;
            // center of new camera position
            Vector3 newCenter = new Vector3(center.x, center.y + 2f, center.z);
            
            // smoothly move camera up.
            StartCoroutine(MoveCameraSmoothly(_cam.transform.position, newCenter, timeToMoveCamera));

        }
    }
    
    IEnumerator MoveCameraSmoothly(Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            // Incrementally move towards the target position using slerp
            _cam.transform.position = Vector3.Slerp(startPos, endPos, elapsedTime / time);
            
            // Update elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the camera reaches the exact target position
        _cam.transform.position = endPos;
    }
    
    // trigger callbacks
    void OnTriggerEnter2D(Collider2D col)
    {
        _timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _timer += Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _timer = 0f;
    }
}
