using InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class CellContent
    {
        public CellContentType Type { get; private set; }
        public Block Block { get; private set; }
        private List<ExitBlock> _exits = new List<ExitBlock>();

        public void SetBlock(Block block)
        {
            Block = block;
            UpdateType();
        }

        public void SetExit(ExitBlock exit)
        {
            if (exit != null)
            {
                _exits.Add(exit);
            }
            UpdateType();
        }

        public List<ExitBlock> GetExits()
        {
            return _exits;
        }

        public ExitBlock GetExitWithDirection(int direction)
        {
            return _exits.Find(exit => exit.GetDirection() == direction);
        }

        private void UpdateType()
        {
            if (Block != null && _exits.Count > 0)
                Type = CellContentType.BlockAndExit;
            else if (Block != null)
                Type = CellContentType.Block;
            else if (_exits.Count > 0)
                Type = CellContentType.Exit;
            else
                Type = CellContentType.Empty;
        }

        public void ClearExits()
        {
            _exits.Clear();
            UpdateType();
        }
    }
}
