using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] private float speedOfSoul = 1;
    private bool isReloadedScene = false;

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + speedOfSoul * Time.deltaTime);

        if (!isReloadedScene)
            StartCoroutine(SceneReload());
    }

    private IEnumerator SceneReload()
    {
        isReloadedScene = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

}
