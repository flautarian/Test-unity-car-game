using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.1F;
    public float rotationSpeed = 3f;

    private Vector3 velocity = Vector3.zero;
    float distance;
    Vector3 position;
    Vector3 newPos;
    Quaternion rotation;
    Quaternion newRot;

    void Start()
    {
        rotation = Quaternion.Euler(new Vector3(45, target.rotation.eulerAngles.y, 0f));
    }

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 10, -10));
        newRot = Quaternion.Euler(new Vector3(45f, target.rotation.eulerAngles.y, 0f));
        rotation = Quaternion.Lerp(rotation, newRot, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
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
