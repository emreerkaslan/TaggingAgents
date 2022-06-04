using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public GameObject[] agents;
    public bool agentMode;
    public Transform goal;
    private Component script;
    private Rigidbody rb;
    public Material runnerMaterial;
    public Material taggerMaterial;
    int fpsCount  = 0;
    void Start()
    {
        runnerMaterial = Resources.Load("Materials/blue", typeof(Material)) as Material;
        taggerMaterial = Resources.Load("Materials/red", typeof(Material)) as Material;
        agents = GameObject.FindGameObjectsWithTag("Agent");
        rb = this.GetComponent<Rigidbody>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (agentMode)
            {
                rend.material = taggerMaterial;
            }
            else
            {
                rend.material = runnerMaterial;
            }
        }

        if(findClosestEnemy() != null)
        {
            agent.destination = findClosestEnemy().position;
        }
    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
    
        if(fpsCount == 5){

        NavMeshAgent thisAgent = GetComponent<NavMeshAgent>();
        if (agentMode)
        {
            //tagging
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = taggerMaterial;
            }
            thisAgent.destination = findClosestEnemy().position;
        }
        else
        {
            //running
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = runnerMaterial;
            }
            Vector3 direction = thisAgent.transform.position - findClosestEnemy().position;
            direction.Normalize();
            NavMeshHit hit;
            if(!NavMesh.Raycast(thisAgent.transform.position, (Vector3)thisAgent.transform.position + (direction * 2), out hit, NavMesh.AllAreas))
            {
                thisAgent.destination = (Vector3)thisAgent.transform.position + (direction * 2);
            }
            else
            {
                thisAgent.destination = ReturnRandomLocationInPlayableArea(thisAgent.transform.position, 5, -1);
            }
        }
        fpsCount = 0;
        }
        fpsCount++;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (agentMode && collision.gameObject.tag == "Agent" && collision.gameObject.GetComponent<Agent>() != null)
        {
            Agent otherAgent = collision.gameObject.GetComponent<Agent>();
            if (!otherAgent.agentMode)
            {
                otherAgent.agentMode = true;
            }
        }
    }

    public void setAgentMode(bool mode)
    {
        this.agentMode = mode;
    }

    public bool getAgentMode()
    {
        return this.agentMode;
    }

    private Transform findClosestEnemy()
    {
        float closestDistance = float.MaxValue;
        Transform tempGoal = null;
        foreach (GameObject agent in agents)
        {
            Agent enemy = agent.GetComponent<Agent>();
            if(enemy != null)
            {
                if (enemy.agentMode != this.agentMode)
                {
                    Transform location = enemy.transform;
                    float distance = Mathf.Sqrt(Mathf.Pow(location.position.x - transform.position.x, 2f) + Mathf.Pow(location.position.z - transform.position.z, 2f));
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        tempGoal = enemy.transform;
                    }
                }
            }
        }
        return tempGoal;
    }

    private static Vector3 ReturnRandomLocationInPlayableArea(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        
        return navHit.position;
    }

    private bool CheckIfPointInPlayableArea(Vector3 destination)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 1f, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }
}
