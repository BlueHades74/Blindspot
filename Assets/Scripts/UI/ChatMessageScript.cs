using System.Collections;
using UnityEngine;

public class ChatMessageScript : MonoBehaviour
{
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        if (transform.parent.transform.childCount > 6)
        {
            Destroy(transform.parent.transform.GetChild(0).gameObject);
        }
        StartCoroutine(Timeout());
    }

    private IEnumerator Timeout()
    {
        yield return new WaitForSeconds(15);

        Destroy(gameObject);
    }
}
