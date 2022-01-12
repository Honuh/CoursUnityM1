using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [Tooltip("Prefab à spawn")]
    public Transform prefabAI;
    [Tooltip("Point de spawn des IA")]
    public Transform spawnPoint;

    [System.Serializable]
    public class Vague
    {
        public int nbSpawn;
        public Transform prefabSpawn;
    }

    [Tooltip("Vagues d'ennemis")]
    public Vague[] vagues = new Vague [0];
    private int currentVague = 0;
    private int nbSpawned = 0;

    private int nb;
    public int nbMax;


    float timeSpawn = 0;
    [Range(1, 50)]
    public float timeNextSpawn = 0;

    public float timeVague = 0;
    [Range(5, 150)]
    public float timeNextVague = 0;

    private float non = 1;

    private Vector3 lastPichenette;

    // Start is called before the first frame update
    void Start()
    {
        non = Random.Range(0f, 1.0f);
        nbSpawned = 0;
        currentVague = 0;

    }

    // Update is called once per frame
    void Update()
    {
        timeVague += Time.deltaTime;
        if(timeVague > timeNextVague)
        {
            timeVague = 0;
            currentVague++;
            nbSpawned = 0;
        }

        if(currentVague < vagues.Length)
        {
            Vague vagueNow = vagues[currentVague];
            int nbToSpawn = vagueNow.nbSpawn;
            if(nbSpawned < nbToSpawn)
            {
                timeSpawn = timeSpawn + Time.deltaTime;
                if (timeSpawn > timeNextSpawn / 10 + non && nb < nbMax)
                {
                    Transform spawnedAi = Spawn(vagueNow.prefabSpawn);
                    nbSpawned++;
                    Vector3 pichenette = spawnedAi.forward * -15;
                    pichenette.x += Random.Range(-1.0f, 1.0f);
                    pichenette.y += Random.Range(0.0f, 1.0f);

                    AddPichenette(spawnedAi, pichenette);
                    lastPichenette = pichenette;
                    timeSpawn = 0;
                    non = 0;
                    nb += 1;
                    
                }
            }
        }
       
    }

    Transform Spawn(Transform prefabAi)
    {
        Transform ai = GameObject.Instantiate<Transform>(prefabAi);
        ai.position = spawnPoint.position;
        ai.rotation = spawnPoint.rotation;

        return ai;
    }

    void AddPichenette(Transform ai, Vector3 pichenette)
    {
        Rigidbody rb = ai.GetComponent<Rigidbody>();
        rb.AddForce(pichenette, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + lastPichenette);
    }
}
