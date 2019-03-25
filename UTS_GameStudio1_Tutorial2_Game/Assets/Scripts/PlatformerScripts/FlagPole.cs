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

    [SerializeField]
    Sprite flagPoleSprite;

    public void InitEndAnimation() {

        StartCoroutine(PlayEndAnimation());
    }
    
    IEnumerator PlayEndAnimation() {

        yield return StartCoroutine(MovePlayerDownPole());
    }

    IEnumerator MovePlayerDownPole() {

        Vector3 endpositionFlag = end.position + offset;

        while (!Utilities.VectorEquals(player.transform.position, end.position)) {

            flag.transform.position = Vector3.Lerp(flag.transform.position, endpositionFlag, 0.05f * Time.deltaTime);

            yield return null;
        }
    }
}
