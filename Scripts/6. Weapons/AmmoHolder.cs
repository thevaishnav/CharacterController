using System;

namespace CCN.Health
{
    [Serializable]
    public class AmmoHolder
    {
        public int MagSize;
        public float CurrentMagCount;
        public int CurrentAmmo;
        public bool AllowReloadBeforeShot;
        public bool AllowReloadAfterShot;
        public bool AllowReloadIfMagIsFull;
        public GunReloadType ReloadType;
        
        public AmmoHolder(int magSize, float currentMagCount, int currentAmmo, GunReloadType reloadType, bool allowReloadBeforeShot = false, bool allowReloadAfterShot = true, bool allowReloadIfMagIsFull = false)
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