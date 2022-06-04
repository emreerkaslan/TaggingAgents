using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public GameObject[] agents;
    public int agentCount = 10;
    public int runnerPercentage = 10;
    private int runnerCount =0;
    private float distance = 10;
    public GameObject agentPrefab;
    private void Awake()
    {        
        var ground = GameObject.FindWithTag("Ground");

        if(ground != null){
            distance = ground.transform.localScale.x;
        }

        var position = ReturnRandomLocationInPlayableArea(new Vector3(1,0,-20),distance,1);

        runnerCount = (runnerPercentage*agentCount)/100;

        for(int k = 0; k < agentCount-1;k++){
            GameObject newAgent = Instantiate(agentPrefab, position, Quaternion.identity);
            Agent newAgentScript = newAgent.GetComponent<Agent>();
            if (newAgentScript != null)
            {
                newAgentScript.agentMode = true;
            }
        }
        agents = GameObject.FindGameObjectsWithTag("Agent");
    }

    private void FixedUpdate() 
    {
        int totalRunners = 0;
        foreach (GameObject agent in agents)
        {
            Agent enemy = agent.GetComponent<Agent>();
            if (enemy != null)
            {
                if (enemy.agentMode == false)
                {
                    totalRunners = totalRunners + 1;
                }
            }
        }

        if (totalRunners <= runnerCount)
        {
            foreach (GameObject agent in agents)
            {
                Agent enemy = agent.GetComponent<Agent>();
                if (enemy != null)
                {
                    enemy.agentMode = !enemy.agentMode;
                }
            }
        }
    }

    private static Vector3 ReturnRandomLocationInPlayableArea(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;

        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        
        return navHit.position;
    }
}
