using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class counter : MonoBehaviour
{
    public AISpawner portal;
    public Text text;

    float timeLeft;
    int timeLeftInt;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = portal.timeNextVague - portal.timeVague;
        timeLeftInt = Mathf.RoundToInt(timeLeft);
        text.text = timeLeftInt.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft = portal.timeNextVague - portal.timeVague;
        timeLeftInt = Mathf.RoundToInt(timeLeft);
        text.text = timeLeftInt.ToString();
    }
}
