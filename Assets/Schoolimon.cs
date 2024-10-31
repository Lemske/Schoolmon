using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Schoolimon : MonoBehaviour
{

    InputAction attackAction;


    // Start is called before the first frame update
    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");

    }

    // Update is called once per frame
    void Update()
    {
        if (attackAction.WasPressedThisFrame())
        {

            foreach (var health in FindObjectsByType<Health>(FindObjectsSortMode.None))
            {
                health.TakeDamage(10);
            }
        }
    }


}