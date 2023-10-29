using UnityEngine;

namespace CCN.Health
{
    public static class GunCalculations
    {
        /// <summary> Convert magCount to ammo count. </summary>
        public static int MagCountToAmmo(float magCount, int magSize) => (int)(magCount * magSize);

        /// <summary> Convert ammo count to magCount. </summary>
        public static float AmmoToMagCount(int ammo, int magSize) => ammo / (float)magSize;

        /// <returns> Total available ammo in all the mags </returns>
        public static int TotalAvailableAmmo(int ammoInCurrentMag, float availableMags, int magSize) => (int)(ammoInCurrentMag + availableMags * magSize);
        
        /// <summary> Remove ammo from mag </summary>
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
        public static void Reload(ref int currentAmmo, ref float currentMagCount, int magSize, GunReloadType reloadType)
        {
            float tollarance = 1f / magSize;
            if (currentMagCount < tollarance) return;

            switch (reloadType)
            {
                case GunReloadType.FullReload:
                    // if gun doesn't have 1 full magazine
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
                    break;
                case GunReloadType.SemiReload:
                    // how many magazines we need to completely fill current magazine
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
                    break;
                default:
                    Debug.LogError($"Unknown ReloadType: {reloadType}");
                    break;
            }
        }

        /// <summary> Fire one shot </summary>
        /// <param name="currentAmmo"> Ammo count in current mag before reloading. Function will update this to new count. </param>
        /// <param name="currentMagCount"> Number of mags available for gun. Function will update this to new count. </param>
        /// <param name="magSize"> Capacity of one mag </param>
        /// <param name="reloadIfHasZeroAmmo"> Should the gun be reloaded if current ammo is 0 (before shooting) </param>
        /// <param name="reloadIfReachesZeroAmmo"> Should the gun be reloaded if current ammo is 0 (after shooting) </param>
        /// <returns> Was the shot successful </returns>
        public static bool Shoot(ref int currentAmmo, ref float currentMagCount, int magSize, bool reloadIfHasZeroAmmo, bool reloadIfReachesZeroAmmo)
        {
            if (currentAmmo == 0)
            {
                if (reloadIfHasZeroAmmo == false) return false;
                
                Reload(ref currentAmmo, ref currentMagCount, magSize, GunReloadType.FullReload);        // Reload type shouldn't matter if current ammo is zero
                
                // possible if user had zero magazines and zero ammo
                if (currentAmmo == 0) return false;
            }

            currentAmmo--;

            if (reloadIfReachesZeroAmmo && currentAmmo == 0)
            {
                Reload(ref currentAmmo, ref currentMagCount, magSize, GunReloadType.FullReload);        // Reload type shouldn't matter if current ammo is zero
            }
            return true;
        }
    }
}