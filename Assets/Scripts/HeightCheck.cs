using System.Collections;
using UnityEngine;

/*
 * This script handles the movement of the camera when the blocks get too high
 */
public class HeightCheck : MonoBehaviour
{
    [SerializeField] private float timeToMoveCamera = 2f;
    [SerializeField] private float moveCameraDelay = 0.6f;
    [SerializeField] private float cameraMoveRange = 4f;
    
    private Camera _cam;
    private Collider2D _triggerbox;
    private float _timer;
    private void Start()
    {
        _cam = GetComponentInParent<Camera>();
        _triggerbox = GetComponent<Collider2D>();
        _triggerbox.isTrigger = true;
    }
    
    private void Update()
    {
        // get center of the camera
        Vector3 center = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _cam.nearClipPlane));
        
        // if this is true the blocks are too high and the camera should move up.
        // has a value of 0.6f because blocks take a shorter time than that to move through the trigger.
        if (_timer > moveCameraDelay)
        {
            _timer = 0f;
            Vector3 newCenter = new Vector3(center.x, center.y + cameraMoveRange, center.z);
            
            // move camera up.
            StartCoroutine(MoveCameraSmoothly(_cam.transform.position, newCenter, timeToMoveCamera));

        }
    }
    
    // move camera up smoothly in timeToMoveCamera (2) seconds.
    private IEnumerator MoveCameraSmoothly(Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            _cam.transform.position = Vector3.Slerp(startPos, endPos, elapsedTime / time);
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        // Ensure the camera reaches the exact target position
        _cam.transform.position = endPos;
    }
    
    // trigger callbacks. used to see if the camera needs to move up.
    // If a block is inside of the trigger for longer than moveCameraDelay. the camera moves up.
    private void OnTriggerEnter2D(Collider2D col)
    {
        _timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _timer += Time.deltaTime;
    }

    // block left trigger. reset timer.
    private void OnTriggerExit2D(Collider2D other)
    {
        _timer = 0f;
    }
}
