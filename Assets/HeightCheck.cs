using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HeightCheck : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _cam;
    private Collider2D _triggerbox;
    private float _timer;
    void Start()
    {
        _cam = GetComponentInParent<Camera>();
        _triggerbox = GetComponent<Collider2D>();
        _triggerbox.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _cam.nearClipPlane));
        if (_timer > 0.5f)
        {
            _timer = 0f;
            Vector3 newCenter = new Vector3(center.x, center.y + 2f, center.z);
            // _cam.transform.position += difference;
            StartCoroutine(MoveCameraSmoothly(_cam.transform.position, newCenter, 2f));

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
    
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.name);
        _timer += Time.deltaTime;
        Debug.Log(_timer);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _timer += Time.deltaTime;
        Debug.Log(_timer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("reset timer");
        _timer = 0f;
    }
}
