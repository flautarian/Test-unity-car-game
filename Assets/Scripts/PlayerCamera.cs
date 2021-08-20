using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.5F;
    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.transform.position + cameraOffset;
        // Smoothly move the camera towards that target position
        transform.position = Vector3.Slerp(transform.position, targetPosition, smoothTime);
        transform.LookAt(target);
    }

    public IEnumerator PlayCameraShakeAnimation(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
