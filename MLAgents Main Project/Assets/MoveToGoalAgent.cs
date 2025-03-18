using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
[SerializeField] private Transform targetTransform;
[SerializeField] private Material winMaterial;
[SerializeField] private Material loseMaterial;
[SerializeField] private MeshRenderer floorMeshRenderer;
    public override void OnEpisodeBegin()
    {
      //  base.OnEpisodeBegin();
      transform.localPosition = Vector3.zero;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
       // base.CollectObservations(sensor);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
       // base.OnActionReceived(actions);
       //Debug.Log(actions.ContinuousActions[0]);
       float moveX = actions.ContinuousActions[0];
       float moveZ = actions.ContinuousActions[1];

       float moveSpeed = 3f;
       transform.localPosition += new Vector3(moveX,0,moveZ) * Time.deltaTime * moveSpeed;

       SetReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other){
            if(other.TryGetComponent<Goal>(out Goal goal)){
            SetReward(+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
            }
            if(other.TryGetComponent<Wall>(out Wall wall)){
            SetReward(-0.25f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
            }
        }

}
