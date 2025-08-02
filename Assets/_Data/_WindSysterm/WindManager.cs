using System.Collections.Generic;
using UnityEngine;

public class WindManager : PirateSingleton<WindManager>
{
    [Header("Wind Settings")]
    public float windStrength = 700f;
    public float windChangeInterval = 10f;

    public Vector3 currentWindForce;
    private float windTimer;

    private List<IWindAffectAble> windAffectAbleObjects = new List<IWindAffectAble>();
    

    protected override void Start()
    {
        windTimer = windChangeInterval;
        GenerateNewWindForce();
    }

    void Update()
    {
        windTimer -= Time.deltaTime;
        if (windTimer <= 0)
        {
            GenerateNewWindForce();
            windTimer = windChangeInterval;
        }
    }

    void FixedUpdate()
    {
        foreach (IWindAffectAble obj in windAffectAbleObjects)
        {
            obj.ApplyWindForce(currentWindForce);
        }
    }

    void GenerateNewWindForce()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        currentWindForce = randomDirection * windStrength;

        Debug.Log("Hướng gió mới: " + currentWindForce);
    }

    public void Register(IWindAffectAble obj)
    {
        if (!windAffectAbleObjects.Contains(obj))
        {
            windAffectAbleObjects.Add(obj);
        }
    }

    public void Unregister(IWindAffectAble obj)
    {
        if (windAffectAbleObjects.Contains(obj))
        {
            windAffectAbleObjects.Remove(obj);
        }
    }
}