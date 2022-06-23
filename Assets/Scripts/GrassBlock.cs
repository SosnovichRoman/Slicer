using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrassBlock : MonoBehaviour
{
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float initialJumpRandomRadius;

    private Collider col;
    private Sequence jump;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void Start()
    {
        Vector3 targetPosition = new Vector3(Random.Range(-initialJumpRandomRadius, initialJumpRandomRadius), 0.2f, Random.Range(-initialJumpRandomRadius, initialJumpRandomRadius));
        transform.DOLocalJump(targetPosition, 1f, 1, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<GrassBag>(out GrassBag grassBag))
        {
            Vector3 offset = grassBag.AddGrass();
            if(offset != Vector3.down)
            {
                transform.SetParent(grassBag.grassBag.transform);
                jump = transform.DOLocalJump(offset, jumpPower, 1, 1f, false).OnComplete(() =>
                {
                    transform.DOLocalMove(offset, 0.1f).OnComplete(() => transform.rotation = grassBag.transform.rotation);
                });
                transform.DOScale(0.1f, 1f);
                col.enabled = false;
                grassBag.grassBlocks.Add(gameObject);
            }
        }
    }

}
