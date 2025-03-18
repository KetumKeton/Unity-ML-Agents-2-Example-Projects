using UnityEngine;
using Unity.MLAgents;
public class ball : MonoBehaviour
{
    public MoveToGoalAgent agent1; // Agent 1
    public MoveToGoalAgent agent2; // Agent 2
    private Vector3 ballpos;

/*public override void Initialize()
{
    startposball = ballTransform.localPosition;
}*/

   /* public override void OnEpisodeBegin()
    {
        transform.localPosition = startposball;
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal2")) // Kendi kalene girdiğinde
        {
            agent1.SetReward(-1.0f); // Agent 1 cezalandırılır
            agent2.SetReward(1.0f); // Rakip ödüllendirilir
            Debug.Log("Gol: Rakip kazandı!");
            agent1.EndEpisode(); // Yeni episode başlat
            agent2.EndEpisode(); // Yeni episode başlat
        }
        else if (other.CompareTag("Goal1")) // Rakip kaleye girdiğinde
        {
            agent1.SetReward(1.0f); // Agent 1 ödüllendirilir
            agent2.SetReward(-1.0f); // Rakip cezalandırılır
            Debug.Log("Gol: Agent 1 kazandı!");
            agent1.EndEpisode(); // Yeni episode başlat
            agent2.EndEpisode(); // Yeni episode başlat
        }
    }
}
