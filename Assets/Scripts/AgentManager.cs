using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public GameObject[] agents;
    public int agentCount = 10;
    public int runnerPercentage = 25;
    public float distance = 10;
    private void Awake()
    {
        
        int totalTaggers = 0;
        var agent = GameObject.FindWithTag("Agent");
        var ground = GameObject.FindWithTag("Ground");

        if(ground != null){
            distance = ground.transform.localScale.x;
        }
        var position = ReturnRandomLocationInPlayableArea(new Vector3(1,0,-20),distance,1);

        int runnerCount = (runnerPercentage*agentCount)/100;

        for(int k = 0; k < 4;k++){
      
            Agent duplicate = Instantiate(agent,position,new Quaternion(0,0,0,0)).GetComponent<Agent>();
            print("runner");
            duplicate.agentMode = true;
        }
        agent.GetComponent<Agent>().agentMode = false;
        for(int i = 0; i< 7; i++)
        { 
            agent.GetComponent<Agent>().agentMode = false;  
            print("tagger");
            Agent duplicate = Instantiate(agent,position,new Quaternion(0,0,0,0)).GetComponent<Agent>();
           
        }
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

        if (totalRunners <= 2)
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
