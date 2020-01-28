using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLoadingSceneController : MonoBehaviour
{
    void TrumpHasFinishedClimbing()
    {
        GameObject fallingTrump = GameObject.Find("FallingTrump");
        GameObject climbingTrump = GameObject.Find("ClimbingTrump");
        GameObject pelosi = GameObject.Find("PelosiMainScreen");

        climbingTrump.SetActive(false);
        climbingTrump.transform.position = new Vector3(climbingTrump.transform.position.x, climbingTrump.transform.position.y, -1.0f);
        fallingTrump.transform.position = new Vector3(fallingTrump.transform.position.x, fallingTrump.transform.position.y, -1.0f);
        pelosi.transform.position = new Vector3(pelosi.transform.position.x, pelosi.transform.position.y, -1.0f);
        StartCoroutine(Wait(10));

        ZoomToStartMenu();
    }

    
    private void ZoomToStartMenu()
    {
        SceneChanger.LoadScene(SceneChanger.USA_SCENE);
    }

    private IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
