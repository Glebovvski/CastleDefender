using System;
using Grid;
using UnityEngine;
using UnityEngine.AI;
using ViewModels;
using Zenject;

namespace Managers
{
    public class PlaneManager : MonoBehaviour
    {
        private MenuViewModel MenuViewModel { get; set; }

        [SerializeField] private GameGrid Grid;
        [SerializeField] private Transform missileCollider;

        private NavMeshSurface surface;

        public bool GridCreated { get; private set; } = false;

        private float planeFactor = 2.5f;

        public event Action OnGridSet;

        public Vector3 Scale => transform.localScale;

        [Inject]
        private void Construct(MenuViewModel menuViewModel)
        {
            MenuViewModel = menuViewModel;
        }

        private void Start()
        {
            Grid.OnGridCreated += SetUpGrid;
            surface = GetComponent<NavMeshSurface>();
        }

        private void SetUpGrid()
        {
            AttachChild(Grid.transform);
            OnGridSet?.Invoke();
            UpdateNavMesh();
        }

        public void UpdateNavMesh() => surface.BuildNavMesh();

        public void AttachChild(Transform child)
        {
            if (child.parent == this.transform) return;

            var scale = child.localScale;
            child.localScale = new Vector3(scale.x / (this.transform.localScale.x * planeFactor), scale.y / (this.transform.localScale.y * planeFactor), scale.z / (this.transform.localScale.z * planeFactor));
            child.SetParent(this.transform);
        }

        private void Update()
        {
            if (GridCreated) return;
            if (MenuViewModel.IsMenuOpen) return;

            Grid.CreateGrid();
            GridCreated = true;
        }
    }
}