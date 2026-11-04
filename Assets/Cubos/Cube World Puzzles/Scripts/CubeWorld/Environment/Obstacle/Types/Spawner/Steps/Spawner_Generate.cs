using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Spawner_Generate : ISpawnerBase
    {
        public Spawner_Generate(Spawner controller) : base(SpawnStates.Generate, controller)
        {
        }
        public Transform transform => Controller.transform;
        private bool isLock = false;
        public override void Generate() { }

        public override void OnStepFinish()
        {
            isLock = false;
        }

        public override void OnStepStart()
        {
            if (!isLock && Controller.CanGenerate())
            {
                isLock = true;
                generate();
            }
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null) Controller.obstacle.terrainManager.PlaySFX(State);
            ChangeState(SpawnStates.CoolDown);
        }
        private void generate()
        {
            switch (Settings.PrefabOrderType)
            {
                case RandomInOrderEnum.InOrder:
                    buffer.objectsIndex = (buffer.objectsIndex + 1) % Settings.HeroPrefabs.Length;
                    break;
                case RandomInOrderEnum.Random:
                    buffer.objectsIndex = UnityEngine.Random.Range(0, 10 * (Settings.HeroPrefabs.Length)) % Settings.HeroPrefabs.Length;
                    break;
                default: break;
            }
            var node = GameObject.Instantiate<GameObject>(Settings.HeroPrefabs[buffer.objectsIndex]);
            if (initHero(node))
            {
                buffer.GenerateCount++;
                buffer.objects.Add(node);
                ChangeState(SpawnStates.CoolDown);
                return;
            }

            node.transform.position = transform.position + Vector3.up * 2;
            buffer.objects.Add(node);
        }
        private bool initHero(GameObject obj)
        {
            if (!obj) return false;
            HeroController hero = obj.GetComponent<HeroController>();
            if (!hero) return false;
            hero.isRespawnable = false;
            if (Settings.PrefabAutoConfig)
            {
                hero.inputController.controllerType = InputManager.ControllerType.CPU;
            }
            hero.UpdateHeroPositionFromObstacle(Controller.obstacle);
            return true;
        }
    }
}