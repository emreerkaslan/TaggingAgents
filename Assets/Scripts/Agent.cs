using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public GameObject[] agents;
    public GameObject[] boss;
    public bool agentMode;
    public bool isBoss;
    public Transform goal;
    private Component script;
    private Rigidbody rb;
    public Material runnerMaterial;
    public Material taggerMaterial;
    public Material bossMaterial;

    void Start()
    {
        runnerMaterial = Resources.Load("Materials/blue", typeof(Material)) as Material;
        taggerMaterial = Resources.Load("Materials/red", typeof(Material)) as Material;
        bossMaterial =   Resources.Load("Materials/Materials/pacman", typeof(Material)) as Material;
        agents = GameObject.FindGameObjectsWithTag("Agent");
        boss = GameObject.FindGameObjectsWithTag("Boss");

        rb = this.GetComponent<Rigidbody>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if(isBoss && agentMode){
                rend.material = bossMaterial;
               
            }
            else if (!isBoss && agentMode)
            {
                rend.material = taggerMaterial;
            }
            else
            {
                rend.material = runnerMaterial;
            }
        }

        if(findClosestEnemy() != null )
        {
            agent.destination = findClosestEnemy().position;
        }

        foreach(var item in boss){
         item.SetActive(false);
        }

    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        NavMeshAgent thisAgent = GetComponent<NavMeshAgent>();
          print(isBoss);
        if (agentMode && !isBoss)
        {
            //tagging
            print("tagged");
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = taggerMaterial;
            }
            thisAgent.destination = findClosestEnemy().position;
        }
        else if (agentMode && isBoss)
        {
            //boss
            print("boss");
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = bossMaterial;
            }

            thisAgent.destination = findClosestEnemy().position;
        }
        else
        {
            print("runner");
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
    }

    void OnCollisionEnter(Collision collision)
    {
        Agent otherAgent = collision.gameObject.GetComponent<Agent>();

        if(otherAgent != null){
            if(isBoss && otherAgent.agentMode){
                print("boss contacted runner");
                otherAgent.agentMode = false;
            }
            if(agentMode && !isBoss && !otherAgent.agentMode){
               print("runner contacted tagged");
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
                if (enemy.agentMode != this.agentMode && !this.isBoss)
                {
                    Transform location = enemy.transform;
                    float distance = Mathf.Sqrt(Mathf.Pow(location.position.x - transform.position.x, 2f) + Mathf.Pow(location.position.z - transform.position.z, 2f));
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        tempGoal = enemy.transform;
                    }
                }else if(this.isBoss){
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
