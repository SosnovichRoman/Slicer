using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using DG.Tweening;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private GameObject grassBlockPrefab;
    [SerializeField]
    private Material crossSectionMaterial;
    [SerializeField]
    private List<GameObject> grassMeshes;
    [SerializeField]
    private List<GameObject> initialGrassMeshes;
    [SerializeField]
    private float refreshDelay;

    private int maxHealth = 2;
    private int currentHealth;
    private Vector3 offset = new Vector3(0, 0.25f, 0);

    private void Start()
    {
        currentHealth = maxHealth;
        Physics.IgnoreLayerCollision(3, 6);
    }

    public void Cut()
    {

        if (currentHealth > 0)
        {
            for (int i = 0; i < grassMeshes.Count; i++)
            {

                //Cut every grass
                GameObject[] pieces = grassMeshes[i].SliceInstantiate(transform.position + offset * currentHealth, Vector3.up, new TextureRegion(0, 0, 1, 1), crossSectionMaterial);
                foreach (var piece in pieces)
                {
                    piece.transform.SetParent(transform);
                    piece.transform.localPosition = Vector3.zero;
                }
                //Cutted grass jumps 
                Vector3 targetPosition = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                pieces[0].transform.DOLocalJump(targetPosition, 1f, 1, 1f).OnComplete(() => Destroy(pieces[0]));

                if (currentHealth == maxHealth) { initialGrassMeshes[i] = grassMeshes[i]; initialGrassMeshes[i].SetActive(false); }
                else Destroy(grassMeshes[i]);
                grassMeshes[i] = pieces[1];

            }
            currentHealth--;
            Instantiate(grassBlockPrefab, transform.position, grassBlockPrefab.transform.rotation, transform);
        }
        else if (currentHealth == 0)
        {
            for (int i = 0; i < grassMeshes.Count; i++)
            {
                Destroy(grassMeshes[i]);
                gameObject.GetComponent<Collider>().enabled = false;
            }
            Instantiate(grassBlockPrefab, transform.position, grassBlockPrefab.transform.rotation, transform);
            StartCoroutine(RefreshGrassWithDelay());
        }
    } 



    private IEnumerator RefreshGrassWithDelay()
    {
        yield return new WaitForSeconds(refreshDelay);
        for (int i = 0; i < initialGrassMeshes.Count; i++)
        {
            initialGrassMeshes[i].SetActive(true);
            grassMeshes[i] = initialGrassMeshes[i];
        }
        currentHealth = maxHealth;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
