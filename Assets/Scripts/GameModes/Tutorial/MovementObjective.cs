using UnityEngine;
using System.Collections;

public class MovementObjective : MonoBehaviour
{
    public bool MovementObjectiveTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MovementObjectiveTriggered = true;
            gameObject.SetActive(false);
        }
    }
}
