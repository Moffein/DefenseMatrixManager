using RoR2;
using UnityEngine;

namespace DefenseMatrixManager
{
    [Tooltip("Can be added to a GameObject to manage adding/removing colliders to the Defense Matrix Manager.")]
    [RequireComponent(typeof(TeamFilter))]
    public class DefenseMatrixComponent : MonoBehaviour
    {
        [Tooltip("Automatically add this to the Defense Matrix Manager on Start.")]
        public bool addOnStart = true;
        public DefenseMatrixManager.DefenseMatrixInfo? defenseMatrixInfo = null;

        //Read defenseMatrixInfo in Start so that the teamIndex is actually set.
        public virtual void Start()
        {
            GenerateDefenseMatrixInfo();
            if (addOnStart) AddMatrix();
        }

        public virtual void GenerateDefenseMatrixInfo()
        {
            if (defenseMatrixInfo != null) RemoveMatrix();

            TeamFilter teamFilter = base.GetComponent<TeamFilter>();
            if (!teamFilter) return;

            var allColliders = GetComponentsInChildren<Collider>();
            if (allColliders == null || allColliders.Length == 0) return;

            defenseMatrixInfo = new DefenseMatrixManager.DefenseMatrixInfo(allColliders, teamFilter.teamIndex);
        }

        public virtual void AddMatrix()
        {
            if (defenseMatrixInfo != null) DefenseMatrixManager.AddMatrix(defenseMatrixInfo);
        }

        public virtual void RemoveMatrix()
        {
            if (defenseMatrixInfo != null) DefenseMatrixManager.RemoveMatrix(defenseMatrixInfo);
        }

        public virtual void OnDestroy()
        {
            RemoveMatrix();
        }
    }
}
