namespace GameScene.Game
{
    public partial class TankController
    {
        public EErrorCode LevelUpTank()
        {
            if (TankLevel >= TankMaxLevel)
            {
                return EErrorCode.Tank_MaxLevel;
            }
            
            if (_coin < _tankConfig.levelUpCost)
            {
                return EErrorCode.Tank_NotEnoughCoin;
            }

            _coin -= _tankConfig.levelUpCost;
            TankLevel += 1;
            
            return EErrorCode.OK;
        }
    }
}