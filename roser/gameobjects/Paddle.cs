using Windows.Devices.Radios;

namespace roser.gameobjects
{
    internal class Paddle(Arena arena)
	{
		public double X { get; set; } = Arena.SimulationWidth / 2;
		public double Y { get; set; } = Arena.SimulationWidth / 4;
	}
}
