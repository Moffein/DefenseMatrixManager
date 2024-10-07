using BepInEx;
using System;

namespace DefenseMatrixManager
{
    [BepInPlugin("com.Moffein.DefenseMatrixManager", "DefenseMatrixManager", "1.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            DefenseMatrixManager.Init();
        }
    }
}
