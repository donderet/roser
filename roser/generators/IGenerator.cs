using roser.gameobjects;

namespace roser.generators
{
	internal interface ILevelGenerator
	{
		Brick?[,] Generate();

		float GetBrickSize();
	}
}
