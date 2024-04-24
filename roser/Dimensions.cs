namespace roser
{
	internal class Dimensions
	{
		private int bigSide = 0;

		private int _wndWidth = 0;

		private int _wndHeight = 0;

		public int WndWidth {
			get {
				return _wndWidth;
			}
			set {
				if (bigSide < value)
					bigSide = value;
				_wndWidth = value;
			} 
		}

		public int WndHeight
		{
			get
			{
				return _wndHeight;
			}
			set
			{
				if (bigSide < value)
					bigSide = value;
				_wndHeight = value;
			}
		}


	}
}
