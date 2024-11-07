
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Core
{
    public class MoneyController : MonoBehaviour, IPoolable
    {
        public void Active()
        {

        }

        public void Create()
        {

        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void Inactive()
        {

        }

        public Tweener Move(Vector3 targetPosition, Vector3 createPosition,Vector3 scale, int i)
        {
            transform.position = createPosition;
            var t=transform.DOMove(createPosition + new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), 0), 0.1f).OnComplete(() =>
            {
                transform.DOJump(targetPosition, 1, 1, 1).SetDelay(0.35f);
                transform.DOScale(scale,1).SetDelay(0.35f); ;
            }).SetDelay(i*0.025f);

            return t;
        }
    }
}
