using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AverageVelocityForRb : MonoBehaviour {
    public Rigidbody rb;
    public float velocityUpdateInterval = 1f;
    public TextMeshProUGUI velocityText;

    private Queue<Vector3> positions = new Queue<Vector3>();
    private Queue<float> timestamps = new Queue<float>();

    // Update is called once per frame
    void Update() {
        CalculateAverageVelocity();
    }

    void CalculateAverageVelocity() { // made by chatgpt 4o
        // Record the current position and time
        positions.Enqueue(rb.position);
        timestamps.Enqueue(Time.time);

        // Remove old data points that are outside the time window
        while (timestamps.Count > 0 && Time.time - timestamps.Peek() > velocityUpdateInterval) {
            positions.Dequeue();
            timestamps.Dequeue();
        }

        // Calculate the displacement over the time window
        if (positions.Count > 1) {
            Vector3 displacement = rb.position - positions.Peek();
            float timeElapsed = Time.time - timestamps.Peek();

            // Calculate average velocity
            Vector3 averageVelocity = displacement / timeElapsed;

            // Debug log to see the result
            velocityText.text = ((int) averageVelocity.magnitude).ToString();
        }
    }
}
