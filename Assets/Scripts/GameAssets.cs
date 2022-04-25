using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    // Singleton pattern ++
    private static GameAssets instance;
    public static GameAssets Instance { 
        get {
            if (instance == null)
                instance = Resources.Load<GameAssets>("GameAssets");
            return instance;
        }
    }

    public Transform pf_Enemy;
    public Transform pf_ArrowProjectile;
    public Transform pf_BuildingConstruction;

    public Transform pf_EnemyDieParticles;
    public Transform pf_BuildingPlacedParticles;
    public Transform pf_BuildingDestroyedParticles;
}
