using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{

    [SerializeField]
    GameObject agentPrefab;

    [SerializeField]
    int numBoids = 10;

    Agent[] agents;

    [SerializeField]
    float agentRadius = 2.0f;

    [SerializeField]
    float separationWeight = 1.0f, cohesionWeight = 1.0f, alignmentWeight = 1.0f;

    private void Awake()
    {
        List<Agent> agentlist = new List<Agent>();

        for(int i = 0; i<numBoids; i++)
        {
            Vector3 position = Vector3.up * Random.Range(0, 10)
                + Vector3.right * Random.Range(0, 10) + Vector3.forward * Random.Range(0, 10);
            agentlist.Add(Instantiate(agentPrefab, position, Quaternion.identity).GetComponent<Agent>());
        }
        agents = agentlist.ToArray();
    }

    void Update()
    {
        foreach (Agent a in agents)
        {
            a.velocity = Vector3.zero;
            a.checkNeightbours();
            calculateSeparation(a);
            calculateAlignment(a);
            calculateCohesion(a);
            a.updateAgent();
            a.neightbours.Clear();
        }
    }

    void calculateSeparation(Agent a)
    {
        //a.addForce(Vector3.up, Agent.DEBUGforceType.SEPARATION);

        float dist = 0f;

        foreach (Agent agent in a.neightbours)
        {
            dist = Vector3.Distance(transform.position, agent.transform.position);
        }

        dist /= agentRadius;
        dist = 1 - dist;

        Vector3 separationForce = dist * (a.transform.position - transform.position) * separationWeight;

        a.addForce(separationForce);
    }

    void calculateCohesion(Agent a)
    {
        //a.addForce(Vector3.forward, Agent.DEBUGforceType.COHESION);

        Vector3 centralPosition = Vector3.zero;

        foreach (Agent agent in a.neightbours)
        {
            centralPosition += agent.transform.position;
        }

        centralPosition += a.transform.position;
        centralPosition /= a.neightbours.Count + 1;

        Vector3 cohesionForce = (centralPosition - a.transform.position) * cohesionWeight;

        a.addForce(cohesionForce);
    }

    void calculateAlignment(Agent a)
    {
        //a.addForce(Vector3.right, Agent.DEBUGforceType.ALIGNMENT);

        Vector3 direction = Vector3.zero;

        foreach (Agent agent in a.neightbours)
        {
            direction += agent.velocity;
        }

        direction += a.velocity;
        direction /= a.neightbours.Count + 1;

        a.addForce(direction);
    }
}