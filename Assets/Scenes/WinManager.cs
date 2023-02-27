using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public Vector3 playerSpawnPosition;//setting up where the player should spawn up in the Level 2

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            SceneManager.LoadScene(1); //Loads Win Level


            //assigns player position to PlayerSpawn position
            GameObject playerObject = other.gameObject;
            playerObject.transform.position = playerSpawnPosition;
        }
    }
}
