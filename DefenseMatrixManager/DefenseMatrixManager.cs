using EntityStates;
using MonoMod.RuntimeDetour;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DefenseMatrixManager
{
    public static class DefenseMatrixManager
    {
        private static List<DefenseMatrixInfo> activeDefenseMatrices = new List<DefenseMatrixInfo>();

        public static DefenseMatrixInfo AddMatrix(DefenseMatrixInfo defenseMatrixInfo)
        {
            if (activeDefenseMatrices.Contains(defenseMatrixInfo)) return defenseMatrixInfo;

            if (defenseMatrixInfo != null && defenseMatrixInfo.colliders != null && defenseMatrixInfo.colliders.Length > 0)
            {
                activeDefenseMatrices.Add(defenseMatrixInfo);
                return defenseMatrixInfo;
            }
            return null;
        }

        public static void RemoveMatrix(DefenseMatrixInfo defenseMatrixInfo)
        {
            activeDefenseMatrices.Remove(defenseMatrixInfo);
        }

        public static void EnableMatrices(TeamIndex attackerTeam)
        {
            foreach (DefenseMatrixInfo dmi in activeDefenseMatrices)
            {
                if (dmi.teamIndex != attackerTeam)
                {
                    dmi.EnableColliders();
                }
            }
        }
        public static void DisableMatrices(TeamIndex attackerTeam)
        {
            foreach (DefenseMatrixInfo dmi in activeDefenseMatrices)
            {
                if (dmi.teamIndex != attackerTeam)
                {
                    dmi.DisableColliders();
                }
            }
        }

        public class DefenseMatrixInfo
        {
            public Collider[] colliders;
            public TeamIndex teamIndex;

            public DefenseMatrixInfo(Collider[] colliders, TeamIndex teamIndex)
            {
                this.colliders = colliders;
                this.teamIndex = teamIndex;
            }

            public void EnableColliders()
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] != null) colliders[i].enabled = true;
                }
            }

            public void DisableColliders()
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] != null) colliders[i].enabled = false;
                }
            }
        }

        internal static void Init()
        {
            RoR2.Stage.onStageStartGlobal += ClearDefenseMatrices;

            //Main case
            On.RoR2.BulletAttack.Fire += BulletAttack_CheckDefenseMatrix;

            //Not actually bulletattacks
            On.EntityStates.GolemMonster.FireLaser.OnEnter += FireLaser_OnEnter;
            On.EntityStates.FalseSon.LaserFatherBurst.FireBurstLaser += LaserFatherBurst_FireBurstLaser;
            On.EntityStates.Halcyonite.TriLaser.FireTriLaser += TriLaser_FireTriLaser;

            //Laser chargeups and VFX
            On.EntityStates.GolemMonster.ChargeLaser.Update += ChargeLaser_Update;
            On.EntityStates.TitanMonster.ChargeMegaLaser.FixedUpdate += ChargeMegaLaser_FixedUpdate;
            On.EntityStates.TitanMonster.FireMegaLaser.FixedUpdate += FireMegaLaser_FixedUpdate;
            On.EntityStates.EngiTurret.EngiTurretWeapon.FireBeam.GetBeamEndPoint += FireBeam_GetBeamEndPoint;
            On.EntityStates.Halcyonite.ChargeTriLaser.Update += ChargeTriLaser_Update;
            On.EntityStates.FalseSon.LaserFatherCharged.FixedUpdate += LaserFatherCharged_FixedUpdate;
            On.EntityStates.FalseSonBoss.LunarGazeCharge.Update += LunarGazeCharge_Update;
            On.EntityStates.FalseSonBoss.LunarGazeFire.FixedUpdate += LunarGazeFire_FixedUpdate;
        }
        private static void LaserFatherCharged_FixedUpdate(On.EntityStates.FalseSon.LaserFatherCharged.orig_FixedUpdate orig, EntityStates.FalseSon.LaserFatherCharged self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void LunarGazeFire_FixedUpdate(On.EntityStates.FalseSonBoss.LunarGazeFire.orig_FixedUpdate orig, EntityStates.FalseSonBoss.LunarGazeFire self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void LunarGazeCharge_Update(On.EntityStates.FalseSonBoss.LunarGazeCharge.orig_Update orig, EntityStates.FalseSonBoss.LunarGazeCharge self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void LaserFatherBurst_FireBurstLaser(On.EntityStates.FalseSon.LaserFatherBurst.orig_FireBurstLaser orig, EntityStates.FalseSon.LaserFatherBurst self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void TriLaser_FireTriLaser(On.EntityStates.Halcyonite.TriLaser.orig_FireTriLaser orig, EntityStates.Halcyonite.TriLaser self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void ChargeTriLaser_Update(On.EntityStates.Halcyonite.ChargeTriLaser.orig_Update orig, EntityStates.Halcyonite.ChargeTriLaser self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static Vector3 FireBeam_GetBeamEndPoint(On.EntityStates.EngiTurret.EngiTurretWeapon.FireBeam.orig_GetBeamEndPoint orig, EntityStates.EngiTurret.EngiTurretWeapon.FireBeam self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            var ret = orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
            return ret;
        }

        private static void FireMegaLaser_FixedUpdate(On.EntityStates.TitanMonster.FireMegaLaser.orig_FixedUpdate orig, EntityStates.TitanMonster.FireMegaLaser self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void ChargeMegaLaser_FixedUpdate(On.EntityStates.TitanMonster.ChargeMegaLaser.orig_FixedUpdate orig, EntityStates.TitanMonster.ChargeMegaLaser self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void ChargeLaser_Update(On.EntityStates.GolemMonster.ChargeLaser.orig_Update orig, EntityStates.GolemMonster.ChargeLaser self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void FireLaser_OnEnter(On.EntityStates.GolemMonster.FireLaser.orig_OnEnter orig, EntityStates.GolemMonster.FireLaser self)
        {
            TeamIndex teamIndex = self.GetTeam();
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void BulletAttack_CheckDefenseMatrix(On.RoR2.BulletAttack.orig_Fire orig, BulletAttack self)
        {
            TeamIndex teamIndex = TeamIndex.None;
            if (self.owner)
            {
                TeamComponent tc = self.owner.GetComponent<TeamComponent>();
                if (tc) teamIndex = tc.teamIndex;
            }
            DefenseMatrixManager.EnableMatrices(teamIndex);
            orig(self);
            DefenseMatrixManager.DisableMatrices(teamIndex);
        }

        private static void ClearDefenseMatrices(Stage obj)
        {
            activeDefenseMatrices.Clear();
        }
    }
}
