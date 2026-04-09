using UnityEngine;

public class FPCamera : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationDuration = 0.2f; // How long the rotation animation lasts
    public float rotationAmount = 15f;    // Degrees per click

    private bool isRotating = false;
    private float targetRotation = 0f;
    private float startRotation = 0f;
    private float elapsedTime = 0f;

    // Call this from the Left button OnClick
    public void RotateLeft()
    {
        if (!isRotating)
        {
            StartRotation(-rotationAmount);
        }
    }

    // Call this from the Right button OnClick
    public void RotateRight()
    {
        if (!isRotating)
        {
            StartRotation(rotationAmount);
        }
    }

    private void StartRotation(float amount)
    {
        isRotating = true;
        elapsedTime = 0f;
        startRotation = transform.eulerAngles.y;
        targetRotation = startRotation + amount;
    }

    void Update()
    {
        if (isRotating)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);
            float yRotation = Mathf.LerpAngle(startRotation, targetRotation, t);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            if (t >= 1f)
            {
                isRotating = false;
            }
        }
    }
}