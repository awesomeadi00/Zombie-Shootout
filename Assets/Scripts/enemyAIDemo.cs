using UnityEngine;
using System.Collections;

public class enemyAIDemo : MonoBehaviour
{
    [HideInInspector] public IdleStateDemo    idleState;
    [HideInInspector] public AlertStateDemo   alertState;
    [HideInInspector] public IEnemyStateDemo  currentState;

    public Material myMaterial;

    void Start()
    {
        // AI States.
        idleState = new IdleStateDemo(this);
        alertState  = new AlertStateDemo(this);

        currentState = idleState;
    }
    
    void Update()
    {

    // Since our states don't inherit from
    // MonoBehaviour, its update is not called
    // automatically, and we'll take care of it
    // us to call it every frame.
        currentState.UpdateState();
    }

    // Since our states don't inherit from
    // MonoBehaviour, we'll have to let them know
    // when something enters, stays,  or leaves our
    // trigger.
    void OnTriggerEnter(Collider col)
    {
        currentState.OnTriggerEnter(col);
    }

    void OnTriggerStay(Collider col)
    {
        currentState.OnTriggerStay(col);
    }

    void OnTriggerExit(Collider col)
    {
        currentState.OnTriggerExit(col);
    }
}
