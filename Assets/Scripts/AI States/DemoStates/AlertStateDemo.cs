using UnityEngine;
using System.Collections;

public class AlertStateDemo : IEnemyStateDemo
{
    enemyAIDemo myEnemy;

    // When we call the constructor, we save
    // a reference to our enemy's AI
    public AlertStateDemo(enemyAIDemo enemy)
    {
        myEnemy = enemy;
    }

    // Here goes all the functionality that we want
    // what the enemy does when he is in this
    // state.
    public void UpdateState()
    {
        myEnemy.myMaterial.color = Color.red;
    }

    // Since we are already in the Alert state, 
    // we will never call this function from
    // This state
    public void GoToAlertState() { }

    public void GoToIdleState()
    {
        myEnemy.currentState = myEnemy.idleState;
    }


    // In this state the player is already inside, so we will ignore it.
    public void OnTriggerEnter(Collider col) { }

    // We will orient the enemy always looking at the
    // player while we attack him/her
    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            // We always look at the player.
            Vector3 lookDirection = col.transform.position -
                                myEnemy.transform.position;

            // Rotating only on the Y axis
            myEnemy.transform.rotation =
                Quaternion.FromToRotation(Vector3.forward,
                                            new Vector3(lookDirection.x, 0, lookDirection.z));
        }
    }

    // If they player is outside the enemy radius, the enemy changes to Idle State.
    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
            GoToIdleState();
    }
}

