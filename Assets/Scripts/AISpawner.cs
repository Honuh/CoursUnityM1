using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [Tooltip("Prefab à spawn")]
    public Transform prefabAI;
    [Tooltip("Point de spawn des IA")]
    public Transform spawnPoint;


    float time = 0;
    [Tooltip("Delay before enemy spawn in 1/10 seconds"), Range(1, 50)]
    public float spawnTime = 1;
    private float initDelay;

    private Vector3 lastPichenette;

    // Start is called before the first frame update
    void Start()
    {
        initDelay = Random.Range(0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if (time > spawnTime / 10 + initDelay)
        {
            Transform spawnedAi = Spawn();
            Vector3 pichenette = spawnedAi.forward * -5;
            pichenette.x += Random.Range(-5.0f, 5.0f);
            pichenette.y += Random.Range(0.0f, 2.0f);

            AddPichenette(spawnedAi, pichenette);
            lastPichenette = pichenette;
            time = 0;
            initDelay = 0;
        }

        Debug.Log(time);
    }

    Transform Spawn()
    {
        Transform ai = GameObject.Instantiate<Transform>(prefabAI);
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
