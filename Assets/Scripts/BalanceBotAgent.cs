using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BalanceBotAgent : Agent
{
    [Header("Agent Settings")]
    public Transform target;
    public float forceMultiplier = 10f;
    
    private Rigidbody rBody;
    private Vector3 startPosition;

    // Initialize is called once when the agent is created
    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;
    }

    // Called at the start of each episode
    public override void OnEpisodeBegin()
    {
        // Reset agent position and physics
        transform.localPosition = startPosition;
        rBody.angularVelocity = Vector3.zero;
        rBody.linearVelocity = Vector3.zero;

        // Randomize target position on the platform
        target.localPosition = new Vector3(
            Random.Range(-4f, 4f), 
            0.5f, 
            Random.Range(-4f, 4f)
        );
    }

    // Collect observations about the environment
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target position (3 values)
        sensor.AddObservation(target.localPosition);
        
        // Agent position (3 values)
        sensor.AddObservation(transform.localPosition);
        
        // Agent velocity (2 values - only X and Z matter for movement)
        sensor.AddObservation(rBody.linearVelocity.x);
        sensor.AddObservation(rBody.linearVelocity.z);
        
        // Total: 8 observations
    }

    // Execute actions from the neural network
   public override void OnActionReceived(ActionBuffers actions)
{
    // Apply movement forces
    float moveX = actions.ContinuousActions[0];
    float moveZ = actions.ContinuousActions[1];
    rBody.AddForce(new Vector3(moveX, 0, moveZ) * forceMultiplier);
    
    // Calculate distance to target
    float distanceToTarget = Vector3.Distance(transform.position, target.position);
    
    // ✅ CORRECT REWARD STRUCTURE:
    
    // 1. Small distance penalty (encourages moving closer)
    AddReward(-distanceToTarget * 0.001f);  // Very small penalty
    
    // 2. BIG success reward when reaching target
    if (distanceToTarget < 1.5f)
    {
        SetReward(1.0f);  // Clear previous rewards, give +1
        EndEpisode();
    }
    
    // 3. Penalty for falling off (if you have boundaries)
    if (transform.position.y < 0)
    {
        SetReward(-1.0f);
        EndEpisode();
    }
    
    // 4. Very small time penalty (optional)
    AddReward(-0.0001f);  // Much smaller than before
}

    // Manual control for testing (use arrow keys/WASD)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    // Optional: Visualize the path to target in Scene view
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere(target.position, 1.42f);
        }
    }
}