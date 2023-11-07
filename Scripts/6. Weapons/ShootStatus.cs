namespace CCN.Health
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
}