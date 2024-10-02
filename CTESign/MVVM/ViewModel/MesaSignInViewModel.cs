using CTESign.Core;
using CTESign.MVVM.Model;
using CTESign.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CTESign.MVVM.ViewModel
{

	public class MesaSignInViewModel : Core.ViewModel
    {
        #region Variables
        private string _fullName;
		public string FullName
		{
			get { return _fullName; }
			set { _fullName = value; OnPropertyChanged(); }
		}

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }


        private string _studentNumber;

		public string StudentNumber
		{
			get { return _studentNumber; }
			set { _studentNumber = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
		}

		private string _emailAddress;

		public string EmailAddress
		{
			get { return _emailAddress; }
			set { _emailAddress = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
		}

		private string _major;

		public string Major
		{
			get { return _major; }
			set { _major = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
		}

        private string _selectedPurpose;

        public string? SelectedPurpose
        {
            get { return _selectedPurpose; }
            set { _selectedPurpose = value; OnPropertyChanged(); 
                switch (SelectedPurpose)
                {
                    case "Other":
                        IsOther = true;
                        IsWorkshop = false;
                        break;
                    case "Job Search":
                        IsWorkshop = true;
                        IsOther = false;
                        break;
                    default:
                        IsOther = false;
                        IsWorkshop = false;
                        break;
                }
            }
        }
        #endregion

        private bool _isOther;

        public bool IsOther
        {
            get { return _isOther; }
            set { _isOther = value; OnPropertyChanged(); }
        }

        private string _otherTxt;

        public string OtherTxt
        {
            get { return _otherTxt; }
            set { _otherTxt = value; OnPropertyChanged(); }
        }

        private bool _isWorkshop;

        public bool IsWorkshop
        {
            get { return _isWorkshop; }
            set { _isWorkshop = value; OnPropertyChanged(); }
        }

        private string _workshopTxt;

        public string WorkshopTxt
        {
            get { return _workshopTxt; }
            set { _workshopTxt = value; }
        }


        #region Validation
        private bool HasErrors()
        {
            // Check for errors in properties (like FirstName, LastName, etc.)
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || !IsStudentNumberValid() || string.IsNullOrWhiteSpace(SelectedPurpose) || StudentNumber.Length != 9 && StudentNumber.All(char.IsDigit))
            {
                return true;
            }
            return false;
        }

        private bool IsStudentNumberValid()
        {
            // Example of validation: StudentNumber must be numeric
            return int.TryParse(StudentNumber, out _);
        }

        private bool CanSubmit()
        {
            if (HasErrors()) return false;
            else return true;
        }
        #endregion
        public RelayCommand SubmitCommand { get; set; }

        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        public List<string> PurposeOfStayList { get
            {
                return new List<string>
                {
                    "Studying",
                    "Tutoring",
                    "Workshop",
                    "Mesa Event",
                    "Other",
                };
            } 
        }


        bool hasSubmitted = false;
        public MesaSignInViewModel(INavigationService navService) {
            Navigation = navService;

            IsOther = false;

			SubmitCommand = new RelayCommand(async o =>
			{
                if (!CanSubmit()) return;

                // prevents users from submitting twice when double clicking the button.
                if (hasSubmitted) return;
                hasSubmitted = true;

                string answers = """
                [
                    {
                        "questionId": "r9af638ae1f484598b06d4cfcf1a6a93c",
                        "answer1": "{FIRST_NAME}"
                    },
                    {
                        "questionId": "ra8e2a9f61a1b46e9a1f5c8dd57580fde",
                        "answer1": "{LAST_NAME}"
                    },
                    {
                        "questionId": "r0a8968e2da5e4aa79abe664e37aa4617",
                        "answer1": "{STUDENT_NUMBER}"
                    },
                    {
                        "questionId": "ra13f4319b2224d8d8367895be24eb012",
                        "answer1": "{PURPOSE}"
                    },
                ]
                """;

                answers = answers.Replace("{FIRST_NAME}", FirstName)
                                 .Replace("{LAST_NAME}", LastName)
                                 .Replace("{STUDENT_NUMBER}", StudentNumber);

                if (IsOther)
                    answers = answers.Replace("{PURPOSE}", "Other - " + OtherTxt);
                else if (IsWorkshop)
                {
                    answers = answers.Replace("{PURPOSE}", "Workshop - " + WorkshopTxt);
                }
                else
                   answers = answers.Replace("{PURPOSE}", SelectedPurpose);

                await SubmitForm(answers);


			}, canExecute => true);
		}

        private async Task SubmitForm(string answers)
        {
            var url = "https://forms.office.com/formapi/api/0b71261a-495f-4ea9-9911-da844b9402ef/users/36045fab-0c81-425a-b313-e04b3107eb8f/forms('GiZxC19JqU6ZEdqES5QC76tfBDaBDFpCsxPgSzEH649UMDJOUTVNVlpWSENVUTczVFJLSlA2MFpBVy4u')/responses";
            var payload = new
            {
                answers = answers,
                startDate = DateTime.Now,
                submitDate = DateTime.Now,
            };

            try
            {
                var jsonPayload = JsonSerializer.Serialize(payload);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await HttpClientSingleton.GetHttpClient().PostAsync(url, content);

                var statusCode = (int)response.StatusCode;
                var responseText = await response.Content.ReadAsStringAsync();

                if (statusCode == 201)
                {
                    // Verify the response format and extract the relevant data
                    // For example, if the response is in JSON format:
                    // var responseData = JsonSerializer.Deserialize<ResponseModel>(responseText);
                    // FullName = responseData.FullName;
                    FirstName = "";
                    LastName = "";
                    StudentNumber = "";
                    EmailAddress = "";
                    Major = "";
                    
                    SelectedPurpose = null;
                    
                    IsOther = false;
                    IsWorkshop = false;

                    WorkshopTxt = "";
                    OtherTxt = "";

                    Navigation.NavigateTo<SubmittedViewModel>();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request or response processing
                // Log the exception or display an error message to the user
                MessageBox.Show("An error occured, please call any CTE workers regarding the issue before doing anything. ERROR MESSAGE: " + ex.Message, "Dash Software", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
