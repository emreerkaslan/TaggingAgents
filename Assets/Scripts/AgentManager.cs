using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public GameObject[] agents;

    private void Awake()
    {
        int totalTaggers = 0;
        agents = GameObject.FindGameObjectsWithTag("Agent");
        print("started assigning");
        while (totalTaggers < 10)
        {
            print("total" + totalTaggers.ToString());
            print("started loop");
            int rnd = Random.Range(0, agents.Length - 1);
            print("random" + rnd.ToString());
            Agent agent = agents[rnd].GetComponent<Agent>();
            if (agent != null)
            {
                print("agent found");
                if (agent.agentMode != true)
                {
                    print("created agent");
                    agent.agentMode = true;
                    totalTaggers = totalTaggers + 1;
                }
            }
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

        if (totalRunners <= 10)
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
}
