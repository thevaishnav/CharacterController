using Omnix.CCN.Health;
using UnityEngine;

namespace Omnix.CCN.Items
{
    public static class GunMath
    {
        /// <summary> Convert magCount to ammo count. </summary>
        public static int MagCountToAmmo(float magCount, int magSize) => (int)(magCount * magSize);

        /// <summary> Convert ammo count to magCount. </summary>
        public static float AmmoToMagCount(int ammo, int magSize) => ammo / (float)magSize;

        /// <returns> Total available ammo in all the mags </returns>
        public static int TotalAvailableAmmo(int ammoInCurrentMag, float availableMags, int magSize) => Mathf.RoundToInt(ammoInCurrentMag + availableMags * magSize);

        /// <summary> Remove number of bullets from a mag </summary>
        /// <param name="magCount"> Current mag count </param>
        /// <param name="magSize"> Capacity of one mag </param>
        /// <param name="ammoToReduce"> How much ammo to reduce </param>
        /// <returns> New Mag count </returns>
        public static float ReduceAmmoFromMag(float magCount, int magSize, int ammoToReduce) => magCount - AmmoToMagCount(ammoToReduce, magSize);

        /// <summary> Reload a gun. </summary>
        /// <param name="currentAmmo"> Ammo count in current mag before reloading. Function will update this to new count. </param>
        /// <param name="currentMagCount"> Number of mags available for gun. Function will update this to new count. </param>
        /// <param name="magSize"> Capacity of one mag </param>
        /// <param name="reloadType"> Type of reload to perform </param>
        /// <param name="allowReloadIfMagIsFull"> Should the gun reload even if current magazine is fully loaded </param>
        public static bool Reload(ref int currentAmmo, ref float currentMagCount, int magSize, GunReloadType reloadType, bool allowReloadIfMagIsFull)
        {
            if (allowReloadIfMagIsFull == false && currentAmmo == magSize) return false;

            float tolerance = 1f / magSize;
            if (currentMagCount < tolerance) return false;

            switch (reloadType)
            {
                case GunReloadType.FullReload:
                    if (currentMagCount >= 1f)
                    {
                        currentAmmo = magSize;
                        currentMagCount--;
                    }
                    else
                    {
                        currentAmmo = MagCountToAmmo(currentMagCount, magSize); // Number of ammo left in mag 
                        currentMagCount = 0f;
                    }

                    return true;
                case GunReloadType.SemiReload:
                    // how many magazines equivalent ammo we need to completely fill current magazine
                    float magNeeded = AmmoToMagCount(magSize - currentAmmo, magSize);

                    if (magNeeded <= currentMagCount)
                    {
                        currentAmmo = magSize;
                        currentMagCount -= magNeeded;
                    }
                    else
                    {
                        currentAmmo = MagCountToAmmo(currentMagCount, magSize);
                        currentMagCount = 0f;
                    }

                    return true;
                default:
                    Debug.LogError($"Unknown ReloadType: {reloadType}");
                    return false;
            }
        }

        /// <summary> Fire one shot </summary>
        /// <param name="currentAmmo"> Ammo count in current mag before reloading. Function will update this to new count. </param>
        /// <param name="currentMagCount"> Number of mags available for gun. Function will update this to new count. </param>
        /// <param name="magSize"> Capacity of one mag </param>
        /// <param name="allowReloadBeforeShot"> Should the gun be reloaded if current ammo is 0 (before shooting) </param>
        /// <param name="allowReloadAfterShot"> Should the gun be reloaded if current ammo is 0 (after shooting) </param>
        public static ShootStatus Shoot(ref int currentAmmo, ref float currentMagCount, int magSize, bool allowReloadBeforeShot, bool allowReloadAfterShot)
        {
            Debug.Log($"Shooting(currentAmmo: {currentAmmo}, currentMagCount: {currentMagCount}, magSize: {magSize}, allowReloadBeforeShot: {allowReloadBeforeShot}, allowReloadAfterShot: {allowReloadAfterShot})");
            float tolerance = 1f / magSize;
            if (currentAmmo == 0)
            {
                if (currentMagCount < tolerance) return ShootStatus.NotShotOutOfAmmo;
                if (allowReloadBeforeShot == false) return ShootStatus.NotShotNeedReload;

                // Reload & Shoot
                currentAmmo = magSize - 1;
                currentMagCount--;
                return ShootStatus.ReloadedAndShot;
            }

            currentAmmo--;

            if (allowReloadAfterShot && currentAmmo == 0)
            {
                // Reload type shouldn't matter if current ammo is zero
                Reload(ref currentAmmo, ref currentMagCount, magSize, GunReloadType.FullReload, false);
                return ShootStatus.ShotAndReloaded;
            }

            return ShootStatus.Shot;
        }
    }
}