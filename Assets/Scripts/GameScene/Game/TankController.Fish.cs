using System.Collections.Generic;
using Config;
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
        
        private List<Fish> _fishes = new();
        private NativeList<FishMoveData> _fishMoveData = new(Allocator.Persistent);
        
        public EErrorCode SpawnFish(int fishID)
        {
            //Check space
            if (_fishes.Count >= _tankCapacity)
            {
                return EErrorCode.Fish_TankIsFull;
            }
            
            //Check price
            FishConfig fishConfig = GameConfig.Instance.GetFishConfig(fishID);
            if (Coin < fishConfig.price)
            {
                return EErrorCode.Fish_NotEnoughCoin;
            }
            
            //Spawn
            Fish newFish = FishPooling.Instance.Get(fishID);
            if (!newFish)
            {
                return EErrorCode.InternalError;
            }
            Coin -= fishConfig.price;
            
            //move fish
            Vector3 target = RandomPositionInTank(newFish.transform.position);
            FishMoveData moveData =
                new FishMoveData(newFish.transform.position, target, Random.Range(1, 3f), GameController.GameTimer);
            newFish.SetSide(target);
            
            Exp += fishConfig.exp;
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
            }

            job.fishCompleteMove.Dispose();
            job.fishPositions.Dispose();
        }

        private Vector3 RandomPositionInTank(Vector3 currentPosition)
        {
            return new Vector3(
                Random.Range(_limitLeft.position.x, _limitRight.position.x),
                Mathf.Clamp(Random.Range(currentPosition.y + 2, currentPosition.y - 2), _limitDown.position.y, _limitUp.position.y),
                0
            );
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