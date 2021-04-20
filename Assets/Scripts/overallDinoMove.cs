using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class overallDinoMove : MonoBehaviour
{

    NavMeshAgent navMeshAgent;
    NavMeshPath path;
    public float timeForNewPath;
    bool inCoRoutine;
    Vector3 target;
    bool validPath;
    public GameObject player;
    public float turnRate;
    public Animator animator;


    // Use this for initialization
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        StartCoroutine(CalcVelocity());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 10);
        List<GameObject> items = new List<GameObject>();
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Player")
            {
                items.Add(hitCollider.gameObject);
            }
        }

        if (items.Count > 0)
        {
            Vector3 targetDelta = player.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, targetDelta);
            Vector3 turnAxis = Vector3.Cross(transform.forward, targetDelta);
            navMeshAgent.SetDestination(this.transform.position);

            transform.RotateAround(transform.position, turnAxis, Time.deltaTime * turnRate * angleToTarget);
        }
        else
        {
            if (!inCoRoutine)
                StartCoroutine(DoSomething());
        }
       
    }

    Vector3 getNewRandomPosition()
    // setting these ranges is vital larger seems better 
    {
        float x = Random.Range(-300, 300);
        //   float y = Random.Range(-20, 20);
        float z = Random.Range(-300, 300);
        Vector3 pos = new Vector3(x, 0, z);
        return pos;

    }

    IEnumerator DoSomething()
    {
        inCoRoutine = true;
        yield return new WaitForSeconds(timeForNewPath);
        GetNewPath();
        validPath = navMeshAgent.CalculatePath(target, path);
        while (!validPath)
        {
            yield return new WaitForSeconds(0.01f);
            GetNewPath();
            validPath = navMeshAgent.CalculatePath(target, path);
        }

        inCoRoutine = false;
    }

    void GetNewPath()
    {
        target = getNewRandomPosition();
        navMeshAgent.SetDestination(target);
    }

    IEnumerator CalcVelocity()
    {
        while(Application.isPlaying)
        {
            Vector3 lastPos = transform.position;
            yield return new WaitForFixedUpdate();
            int movesSpeed = Mathf.RoundToInt(Vector3.Distance(transform.position, lastPos)/Time.fixedDeltaTime);
            animator.speed = movesSpeed/4;
        }
     }
}