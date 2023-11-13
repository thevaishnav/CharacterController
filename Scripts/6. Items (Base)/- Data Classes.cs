using System;
using Omnix.CCN.Health;

namespace Omnix.CCN.Items
{
    public enum ShootStatus
    {
        /// <summary> Shot successfully without reloading </summary>
        Shot = 0,

        /// <summary> Reloaded and then Shot successfully </summary>
        ReloadedAndShot = 1,

        /// <summary> Shot successfully and then reloaded </summary>
        ShotAndReloaded = 2,

        /// <summary> Not shot because needs to reload (And can be reloaded) </summary>
        NotShotNeedReload = 3,

        /// <summary> Not shot because the gun needs to reload (But doesn't have enough ammo to reload) </summary>
        NotShotOutOfAmmo = 4,
    }
    
     [Serializable]
    public class AmmoInfo
    {
        public int MagSize;
        public float CurrentMagCount;
        public int CurrentAmmo;
        public bool AllowReloadBeforeShot;
        public bool AllowReloadAfterShot;
        public bool AllowReloadIfMagIsFull;
        public GunReloadType ReloadType;
        
        public AmmoInfo(int magSize, float currentMagCount, int currentAmmo, GunReloadType reloadType, bool allowReloadBeforeShot = false, bool allowReloadAfterShot = true, bool allowReloadIfMagIsFull = false)
        {
            ReloadType = reloadType;
            MagSize = magSize;
            CurrentMagCount = currentMagCount;
            CurrentAmmo = currentAmmo;
            AllowReloadBeforeShot = allowReloadBeforeShot;
            AllowReloadAfterShot = allowReloadAfterShot;
            AllowReloadIfMagIsFull = allowReloadIfMagIsFull;
        }
        
        /// <returns> Total available ammo in all the mags </returns>
        public int TotalAvailableAmmo() => GunMath.TotalAvailableAmmo(CurrentAmmo, CurrentMagCount, MagSize);

        /// <summary> Reload a gun. </summary>
        public bool Reload() => GunMath.Reload(ref CurrentAmmo, ref CurrentMagCount, MagSize, ReloadType, AllowReloadIfMagIsFull);

        /// <summary> Fire one shot & Get ShootStatus </summary>
        public ShootStatus Shoot() => GunMath.Shoot(ref CurrentAmmo, ref CurrentMagCount, MagSize, AllowReloadBeforeShot, AllowReloadAfterShot);

        /// <summary> Fire one shot & Get bool representing whether the gun did shoot or not </summary>
        public bool TryShoot()
        {
            switch (GunMath.Shoot(ref CurrentAmmo, ref CurrentMagCount, MagSize, AllowReloadBeforeShot, AllowReloadAfterShot))
            {
                case ShootStatus.Shot:
                case ShootStatus.ReloadedAndShot:
                case ShootStatus.ShotAndReloaded:
                    return true;
                case ShootStatus.NotShotNeedReload:
                case ShootStatus.NotShotOutOfAmmo:
                    return false;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}