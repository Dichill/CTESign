using CTESign.Core;
using CTESign.Services;
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

        public JsonConfigService? ConfigService { get; set; }

        private string? _department;

		public string? Department
		{
			get { return _department; }
			set { _department = value; OnPropertyChanged(); }
		}

		private bool _inAdminmode;
		public bool InAdminMode
		{
			get { return _inAdminmode; }
			set { _inAdminmode = value; OnPropertyChanged(); }
		}


		private bool _isConfiged;
		public bool IsConfiged
		{
			get { return _isConfiged; }
			set { _isConfiged = value; OnPropertyChanged(); }
		}

		private string? _currentAdmin;
		public string? CurrentAdmin
		{
			get { return _currentAdmin; }
			set { _currentAdmin = value; OnPropertyChanged(); }
		}


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
