using Ei.Agents.Core;
using Ei.Agents.Core.Behaviours;
using Ei.Agents.Sims;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SimSpawn : MonoBehaviour, IUpdates
{
    public enum Renderer
    {
        SimRenderer,
        FastRenderer
    }

    public enum AgentType
    {
        RandomWalker,
        Sim
    }

    public float minX;
    public float minZ;
    public float maxX;
    public float maxZ;
    private int spawned;


    public int MinAgentCount;
    public int MaxAgentCount;
    public int SpawnPeriodMs;
    public float MinSpeed;
    public float MaxSpeed;
    public float MinDecayRatePerSecond;
    public float MaxDecayRatePerSecond;
    public float MinInitialDecay;
    public float MaxInitialDecay;
    public Renderer AgentRenderer;
    public AgentType Type;

    public GameObject simAgent;
    public GameObject simObject;

    public float DayLengthInSeconds {
        get { return BehaviourConfiguration.DayLengthInSeconds; }
        set { BehaviourConfiguration.DayLengthInSeconds = value; }
    }

    public List<SeedInfo> SimObjects;

    public void Init() {
        this.SimObjects = new List<SeedInfo>();
        this.MinDecayRatePerSecond = 0.3f;
        this.MaxDecayRatePerSecond = 1f;
        this.MinInitialDecay = -50;
        this.MaxInitialDecay = 50;
        this.AgentRenderer = Renderer.SimRenderer;
        this.Type = AgentType.RandomWalker;
    }


    public void Start() {

        // init sim objects
        this.SpawnObjects();

        // init agents
        this.SpawnAgents();
    }

    public void Update() {
        // we will randomly put objects on the canvas
        foreach (var obj in this.SimObjects) {
            if (obj.SeedPeriodInMinutes > 0 && obj.NextSeed < Time.time) {
                var count = UnityEngine.Random.Range(obj.MinSeedCount, obj.MaxSeedCount);
                for (var i = 0; i < count; i++) {
                    this.SpawnObject(obj.gameObject, i);
                }
                obj.NextSeed += obj.SeedPeriodInMinutes * BehaviourConfiguration.SimulatedMinutesToReal;
            }

        }
    }

    void SpawnAgents() {
        // add required statistics
        for (int i = 0; i < this.MinAgentCount; i++) {
            this.SpawnAgent();
        }

        if (this.MaxAgentCount > this.MinAgentCount) {
            var timer = new System.Timers.Timer();
            timer.Interval = this.SpawnPeriodMs;
            timer.AutoReset = true;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => {
                this.SpawnAgent();
                if (this.spawned == this.MaxAgentCount) {
                    timer.Stop();
                }
            };
            timer.Start();
        }
    }


    void SpawnAgent() {
        

        // set transform

        var position = new Vector3(
                UnityEngine.Random.Range(minX, maxX),
                 0,
                 UnityEngine.Random.Range(minZ, maxZ));
        var agent = GameObject.Instantiate(this.simAgent, position, Quaternion.identity);

        agent.name = this.spawned.ToString();

        // add navigation components
        var nav = agent.GetComponent<NavMeshAgent>();
        nav.speed = Random.Range(MinSpeed, MaxSpeed);

        // create agent specific functionality
        switch (this.Type) {
            case AgentType.RandomWalker:
                this.SpawnRandomWalker(agent);
                break;
            case AgentType.Sim:
                this.SpawnSim(agent);
                break;
        }

        // add renderer
        // we only allow sim renderer for sims

        this.spawned++;
    }

    void SpawnRandomWalker(GameObject agent) {
        //var rnav = agent.AddComponent<RandomNavigation>();
        //rnav.Width = (float) this.maxX - this.minX;
        //rnav.Height = (float)this.maxZ - this.minZ;
    }

    void SpawnObjects() {
        foreach (var obj in this.SimObjects) {
            for (var k = 0; k < obj.Count; k++) {
                this.SpawnObject(obj.gameObject, k++);
            }
        }
    }

    void SpawnObject(GameObject obj, int k) {
       

        // add position
        var position = new Vector3(
            UnityEngine.Random.Range(minX, maxX),
            0,
            UnityEngine.Random.Range(minZ, maxZ));
        var agent = GameObject.Instantiate(obj, position, Quaternion.identity);
        agent.name = obj.name + "_" + k;

        // add advertisement
        //var adv = agent.AddComponent<Ei.Agents.Sims.SimObject>();
        //adv.Actions = obj.Actions.Select(s => new SimAction(
        //    s.Name,
        //    s.Uses,
        //    s.Modifiers.Select(m => new ModifierAdvertisement(m.Delta, m.Type, m.PersonalityModifiers)).ToArray(),
        //    s.DurationInMinutes,
        //    s.Plan.ToArray())).ToArray();
        //adv.Icon = obj.Icon;
        //adv.Name = obj.Name;
    }

    // different agents


    void SpawnSim(GameObject agent) {
        var reasoner = agent.AddComponent<Sim3DReasoner>();

        // add sims stuff
        var sim = agent.AddComponent<Sim>();
        foreach (var modifier in sim.modifiers) {
            modifier.DecayRatePerSecond = UnityEngine.Random.Range(MinDecayRatePerSecond, MaxDecayRatePerSecond);
            modifier.XValue = UnityEngine.Random.Range(MinInitialDecay, MaxInitialDecay);
        }
    }

}

