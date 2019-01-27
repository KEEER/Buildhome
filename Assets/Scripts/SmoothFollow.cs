using UnityEngine;

public class SmoothFollow : MonoBehaviour {

    Vector3 targetPosition;
    public float smootheness = 0.5f;
    public Vector3 offset;
    public Camera self;
    float size;
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 destinationPosition = targetPosition + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, destinationPosition, smootheness);
        transform.position = smoothedPosition;
        float smoothedSize = self.orthographicSize * (1 - smootheness) +
                            size * smootheness;
        self.orthographicSize = smoothedSize;
    }
    public void updateSize(int value)
    {
        size = value;
    }

    public float getSize()
    {
        return size;
    }
    public void updateTargetPosition(Vector2 newPosition)
    {
        targetPosition = newPosition;
    }
}
