using System.Collections;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Clover
{
    public class CloverSpawner : MonoBehaviour
    {
        public float WaitTimeMin { get; set; } = 90f;
        public float WaitTimeMax { get; set; } = 230f;

        [SerializeField] private RectTransform _cloverHolder;
        [SerializeField] private Clover _clover_PF;
        [SerializeField] private PopUpUI _popUp;
        private void Start()
        {
            StartCoroutine(SendClover());
        }

        private IEnumerator SendClover()
        {
            yield return new WaitForSeconds(Random.Range(WaitTimeMin, WaitTimeMax));
            Vector3 spawnPosition = new(Random.Range(-270, 270), 
                Random.Range(-500, 500), 0);
            Clover clover = Instantiate(_clover_PF, _cloverHolder);
            clover.Initialize(_popUp);
            clover.transform.localPosition = spawnPosition;
            yield return SendClover();
        }
    }
}