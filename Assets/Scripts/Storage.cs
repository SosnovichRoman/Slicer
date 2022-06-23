using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Storage : MonoBehaviour
{
    [SerializeField]
    private float grassBlockDelay;
    [SerializeField]
    private float coinSpeed;
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private GameObject coinIcon;
    [SerializeField]
    private CountText countText;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<GrassBag>(out GrassBag grassBag))
        {
            if (grassBag.TakeGrass() == true)
            {
                StartCoroutine(JumpIntoStorage(grassBag));
            }
        }
    }

    private IEnumerator JumpIntoStorage(GrassBag grassBag)
    {
        Sequence jumps = DOTween.Sequence();

        foreach (var grassBlock in grassBag.grassBlocks)
        {
            grassBlock.transform.SetParent(transform);
            jumps.Join(grassBlock.transform.DOJump(transform.position, 3, 1, 2f).OnComplete(SpawnCoin));
            yield return new WaitForSeconds(grassBlockDelay);
        }
        grassBag.ClearBag();
    }

    private void SpawnCoin()
    {
        GameObject created = Instantiate(coinPrefab, Camera.main.WorldToScreenPoint(transform.position), coinPrefab.transform.rotation, coinIcon.transform);
        created.transform.DOMove(coinIcon.transform.position, coinSpeed).
            OnComplete(() => {
                countText.count++;
                countText.UpdateText();
            });
    }
}
