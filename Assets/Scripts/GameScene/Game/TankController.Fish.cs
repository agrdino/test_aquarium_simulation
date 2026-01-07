using System.Collections.Generic;
using Config;
using Data;
using Fx;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScene.Game
{
    public partial class TankController
    {
        public const float MaxDistanceCanMoveInOneSecond = 0.5f;
        public const int RandomFishCost = 10;
        
        private List<Fish> _fishes = new();
        private NativeList<FishMoveData> _fishMoveData;

        public EErrorCode RandomFish()
        {
            if (_coin < RandomFishCost)
            {
                return EErrorCode.Fish_NotEnoughCoin;
            }

            int randomFish = UserData.Instance.UnlockFish[Random.Range(0, UserData.Instance.UnlockFish.Length)];
            FishConfig fishConfig = GameConfig.Instance.GetFishConfig(randomFish);
            EErrorCode result = SpawnFish(fishConfig);
            if (result == EErrorCode.OK)
            {
                _coin -= RandomFishCost;
                _userExp += fishConfig.exp;
            }
            return result;
        }

        public EErrorCode BuyFish(int fishID)
        {
            FishConfig fishConfig = GameConfig.Instance.GetFishConfig(fishID);
            if (_coin < fishConfig.price)
            {
                return EErrorCode.Fish_NotEnoughCoin;
            }
            
            EErrorCode result = SpawnFish(fishConfig);
            if (result == EErrorCode.OK)
            {
                _coin -= fishConfig.price;
                _userExp += fishConfig.exp;
            }
            
            return result;
        }
        
        public EErrorCode SpawnFish(FishConfig fishConfig)
        {
            if (_fishes.Count >= _tankCapacity)
            {
                return EErrorCode.Fish_TankIsFull;
            }
            
            //Spawn
            Fish newFish = FishPooling.Instance.Get(fishConfig.id);
            
            //move fish
            Vector3 target = RandomPositionInTank(newFish.transform.position);
            FishMoveData moveData =
                new FishMoveData(newFish.transform.position, target, Random.Range(1, 3f), GameController.GameTimer);
            newFish.InitStat(fishConfig);
            newFish.SetSide(target);
            newFish.ResetIncomeTimer(GameController.GameTimer);
            newFish.gameObject.SetActive(true);
            
            FishCount += 1;
            
            _fishes.Add(newFish);
            _fishMoveData.Add(moveData);
            
            return EErrorCode.OK;
        }

        private void Update()
        {
            if (_fishes.Count == 0)
            {
                return;
            }
            FishMoveJob job = new FishMoveJob()
            {
                gameTimer = GameController.GameTimer,
                fishMoveDatas = _fishMoveData,
                fishPositions = new (_fishes.Count, Allocator.TempJob),
                fishCompleteMove = new (_fishes.Count, Allocator.TempJob)
            };
            
            JobHandle handle = job.Schedule(_fishes.Count, 32);
            handle.Complete();
            for (var i = 0; i < _fishes.Count; i++)
            {
                _fishes[i].UpdatePosition(job.fishPositions[i]);
                if (job.fishCompleteMove[i])
                {
                    Vector3 target = RandomPositionInTank(job.fishPositions[i]);
                    _fishes[i].SetSide(target);
                        
                    _fishMoveData[i] = new FishMoveData(_fishMoveData[i].target, target,
                        Random.Range(1, 3f), GameController.GameTimer);
                }

                if (GameController.GameTimer >= _fishes[i].NextTimeIncomeReady)
                {
                    //collect
                    CollectFishIncome(_fishes[i]);
                    _fishes[i].ResetIncomeTimer(GameController.GameTimer);
                }
            }

            job.fishCompleteMove.Dispose();
            job.fishPositions.Dispose();
        }

        private Vector3 RandomPositionInTank(Vector3 currentPosition)
        {
            return new Vector3(
                Random.Range(_limitLeft.position.x, _limitRight.position.x),
                Mathf.Clamp(Random.Range(currentPosition.y + 2, currentPosition.y - 2), _limitDown.position.y, _limitUp.position.y),
                Random.Range(0, 1f)
            );
        }

        private void CollectFishIncome(Fish fish)
        {
            _coin += fish.Income;
            FxController.Instance.ShowCollectCoinFx(fish.transform.position);
        }

        private void OnDestroy()
        {
            _fishMoveData.Dispose();
        }
    }
    
    public struct FishMoveData
    {
        public Vector3 origin;
        public Vector3 target;
        public float moveTime;
        public float startTime;

        public FishMoveData(Vector3 origin, Vector3 target, float speed, float startTime)
        {
            this.origin = origin;
            this.target = target;
            moveTime = Vector2.Distance(origin, target) * speed / TankController.MaxDistanceCanMoveInOneSecond;
            this.startTime = startTime;
        }
    }
    
    [BurstCompile]
    public struct FishMoveJob : IJobParallelFor
    {
        [ReadOnly] public NativeList<FishMoveData> fishMoveDatas;
        public NativeArray<Vector3> fishPositions;
        public NativeArray<bool>  fishCompleteMove;
        public float gameTimer;

        public void Execute(int index)
        {
            FishMoveData temp = fishMoveDatas[index];
            float moveTime = gameTimer - temp.startTime;
            fishCompleteMove[index] = moveTime >= temp.moveTime;
            fishPositions[index] = Vector3.Lerp(temp.origin, temp.target, moveTime/temp.moveTime);
        }
    }
}