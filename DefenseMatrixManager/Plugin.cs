using BepInEx;
using System;

namespace DefenseMatrixManager
{
    [BepInPlugin("com.Moffein.DefenseMatrixManager", "DefenseMatrixManager", "1.1.3")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            DefenseMatrixManager.Init();
        }
    }
}
