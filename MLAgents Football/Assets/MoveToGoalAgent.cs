using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
[SerializeField] private Transform ballTransform;
[SerializeField] private Transform goalTransform;
[SerializeField] private Transform opponentTransform; // Rakibin Transform'u

//[SerializeField] private Material winMaterial;
//[SerializeField] private Material loseMaterial;
//[SerializeField] private MeshRenderer floorMeshRenderer;

[SerializeField] private int teamID; // 1: Kırmızı, 2: Mavi

[SerializeField] private Rigidbody agentRb;
[SerializeField] private Rigidbody ballRb;

    private Vector3 startPos;
    private Vector3 startposball;
    private Vector3 startdusman;

public override void Initialize()
{
    startPos = transform.localPosition;  // Sadece bir kez başlatılır.
    startposball = ballTransform.localPosition;
    startdusman = opponentTransform.localPosition;
    //Debug.Log(startPos);
    //Debug.Log(startposball);
    //Debug.Log(startdusman);
}

    public override void OnEpisodeBegin()
    {
      //  base.OnEpisodeBegin();
      //transform.localPosition = startPos;
      agentRb.velocity = Vector3.zero;
      transform.localPosition = startPos;//Vector3.zero;

       ballTransform.localPosition = startposball;
       ballRb.velocity = Vector3.zero;

       opponentTransform.localPosition = startdusman;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
       // base.CollectObservations(sensor);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(ballTransform.localPosition);
        sensor.AddObservation(goalTransform.localPosition);
        sensor.AddObservation(agentRb.velocity);
        sensor.AddObservation(ballRb.velocity);
        sensor.AddObservation(opponentTransform.localPosition);
        sensor.AddObservation(opponentTransform.GetComponent<Rigidbody>().velocity);
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
       // base.OnActionReceived(actions);
       //Debug.Log(actions.ContinuousActions[0]);
       float moveX = actions.ContinuousActions[0];
       float moveZ = actions.ContinuousActions[1];

       float moveSpeed =0.1f;
       agentRb.AddForce(new Vector3(moveX, 0, moveZ) * moveSpeed, ForceMode.VelocityChange);
        // Eğer ajan topa yaklaşamıyorsa küçük bir ceza ver
        float distanceToBall = Vector3.Distance(transform.localPosition, ballTransform.localPosition);
        SetReward(-0.001f * distanceToBall);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other){
        //    if(other.TryGetComponent<Goal>(out Goal goal)){
              if(other.CompareTag("Ball")){
            SetReward(+0.1f);
          //  floorMeshRenderer.material = winMaterial;
         //   EndEpisode();
            }
            
        /*if (other.CompareTag("Goal1") && teamID == 1)
        {
            // Eğer Mavi takım, Kırmızı takımın kalesine gol atarsa
            SetReward(1f);
            EndEpisode();
        }
        if (other.CompareTag("Goal2") && teamID == 2)
        {
            // Eğer Mavi takım, Kırmızı takımın kalesine gol atarsa
            SetReward(1f);
            EndEpisode();
        }*/
    }
}
