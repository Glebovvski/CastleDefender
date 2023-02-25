using System;
using Defendable;
using UnityEngine;
using UnityEngine.AI;

namespace Grid
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private MeshRenderer quadRenderer;
        [SerializeField] private MeshFilter cubeMesh;
        [SerializeField] private Mesh ground;
        [SerializeField] private Mesh grass;
        [SerializeField] private NavMeshSurface surface;
        [SerializeField] private GameObject navVolume;
        [SerializeField] private NavMeshModifier navMeshModifier;

        private int posX;
        private int posY;

        public Defense Defence { get; private set; }
        public bool IsFree => Defence == null;
        public bool IsSet { get; private set; } = false;
        public Vector2Int Pos => new Vector2Int(posX, posY);
        public float Height => transform.localScale.y;
        public bool IsUpper => transform.localScale.y > 1;
        public bool IsSelected => quadRenderer.material.color != defaultCellColor;

        private Color defaultCellColor;

        public event Action OnFreeCell;
        private Action freeCellAction;



        public void SetSelected() => quadRenderer.material.color = Color.green * 10;
        public void Init(int x, int y, Action onFreeCellAction, Transform parent)
        {
            posX = x;
            posY = y;
            IsSet = true;
            this.gameObject.SetActive(true);
            this.transform.SetParent(parent);
            freeCellAction = onFreeCellAction;
            OnFreeCell += freeCellAction;
        }

        public void SetHeight(int value)
        {
            this.transform.localScale = new Vector3(1, value, 1);
            cubeMesh.mesh = value == 1 ? grass : ground;
            CheckNavMeshModifier();

        }
        public void SetDefense(Defense defence)
        {
            if (IsFree)
            {
                ToggleVolume(false);
                Defence = defence;
                Defence.OnDeath += FreeCell;
                defence.DefenseSet();
            }
        }

        public void BuildNavMesh() => surface.BuildNavMesh();

        internal void DeselectCell() => quadRenderer.material.color = defaultCellColor;
        public void FreeCell()
        {
            Defence.OnDeath -= FreeCell;
            Defence = null;
            ToggleVolume(true);
            OnFreeCell?.Invoke();
        }

        private void ToggleVolume(bool active)
        {
            if (!IsUpper)
                navVolume.SetActive(active);
        }

        private void CheckNavMeshModifier()
        {
            if (IsUpper)
            {
                navMeshModifier.ignoreFromBuild = false;
                OnFreeCell?.Invoke();
            }
        }

        public void Reset()
        {
            IsSet = false;
            this.transform.SetParent(null);
            this.transform.localScale = Vector3.one;
            OnFreeCell -= freeCellAction;
            this.gameObject.SetActive(false);
        }
    }
}