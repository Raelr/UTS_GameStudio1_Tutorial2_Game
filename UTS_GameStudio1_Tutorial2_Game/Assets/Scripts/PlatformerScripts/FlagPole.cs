using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPole : MonoBehaviour
{

    [SerializeField]
    Player player;

    [SerializeField]
    Transform end;

    [SerializeField]
    SpriteRenderer flag;

    [SerializeField]
    Vector3 offset;

    [SerializeField]
    Vector3 mariOffset;

    public void InitEndAnimation() {

        StartCoroutine(PlayEndAnimation());
    }
    
    IEnumerator PlayEndAnimation() {

        player.CanMove = false;

        Debug.Log("End");

        yield return StartCoroutine(MovePlayerDownPole());
    }

    IEnumerator MovePlayerDownPole() {

        Vector3 endpositionMario = end.position + mariOffset;

        Vector3 endpositionFlag = end.position + offset;

        while (!Utilities.VectorEquals(player.transform.position, end.position)) {

            player.transform.position = Vector3.Lerp(player.transform.position , endpositionMario , 1.5f * Time.deltaTime);

            flag.transform.position = Vector3.Lerp(flag.transform.position, endpositionFlag, 0.5f * Time.deltaTime);

            yield return null;
        }
    }
}
