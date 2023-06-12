using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingDetector : MonoBehaviour
{
    private ObjectFader fader;
    private LayerMask objectlayer = 9;

    void Start()
    {
        fader = GetComponent<ObjectFader>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, objectlayer))
            {
                if (fader != null)
                {
                    fader.SetDoFade(false);
                }
                fader = hit.collider.gameObject.GetComponent<ObjectFader>();
                if (fader != null)
                {
                    Debug.Log("fader not null last stage");
                    fader.SetDoFade(true);
                }
            }


        }
    }
}

