using UnityEngine;

namespace InGame
{
    [CreateAssetMenu(fileName = "BlockTypes", menuName = "ColorBlock/BlockTypes")]
    public class BlockTypes : ScriptableObject
    {
        [Header("Block Prefabs")]
        public GameObject[] BlockPrefabs;
    }
}