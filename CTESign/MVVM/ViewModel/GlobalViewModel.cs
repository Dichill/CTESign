using CTESign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTESign.MVVM.ViewModel
{
    public class GlobalViewModel : ObservableObject
    {
        public static GlobalViewModel Instance { get; } = new GlobalViewModel();

		private string? _afkTitle;
		public string? AfkTitle
		{
			get { return _afkTitle; }
			set { _afkTitle = value; OnPropertyChanged(); }
		}

		private string? _afkSubtitle;
		public string? AFKSubtitle
		{
			get { return _afkSubtitle; }
			set { _afkSubtitle = value; OnPropertyChanged(); }
		}



		private string? _logoImage;
		public string? LogoImage
		{
			get { return _logoImage; }
			set { _logoImage = value; OnPropertyChanged(); }
		}

		private int _logoSize;
		public int LogoSize
		{
			get { return _logoSize; }
			set { _logoSize = value; OnPropertyChanged(); }
		}


		private string? _afkBackgroundImage;
		public string? AFKBackgroundImage
		{
			get { return _afkBackgroundImage; }
			set { _afkBackgroundImage = value; OnPropertyChanged(); }
		}
	}
}
