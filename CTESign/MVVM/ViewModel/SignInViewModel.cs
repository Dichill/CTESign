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

	public class SignInViewModel : Core.ViewModel
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
            set { _selectedPurpose = value; OnPropertyChanged(); if (SelectedPurpose == "Other") IsOther = true; else IsOther = false; }
        }

        private bool _needJob;

		public bool NeedJob
		{
			get { return _needJob; }
			set { _needJob = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
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



        #region Validation
        private bool HasErrors()
        {
            // Check for errors in properties (like FirstName, LastName, etc.)
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || !IsStudentNumberValid() || string.IsNullOrWhiteSpace(SelectedPurpose))
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
                    "Career Counseling",
                    "Career Assessment",
                    "Career Exploration",
                    "Career Workshop",
                    "Resume Building",
                    "Job Search",
                    "Study Lab",
                    "Other"
                };
            } 
        }

        public SignInViewModel(INavigationService navService) {
            Navigation = navService;

			NeedJob = true;
            IsOther = false;

			SubmitCommand = new RelayCommand(async o =>
			{
                if (!CanSubmit()) return;

                string answers = """
                [
                    {
                        "questionId": "re9b2f4a2ad0a4db6a31a41e53a7bb26c",
                        "answer1": "{FIRST_NAME}"
                    },
                    {
                        "questionId": "rb35e18bd679a4465a7f73b11ba824f0d",
                        "answer1": "{LAST_NAME}"
                    },
                    {
                        "questionId": "r2f15542143844d358460a2bcf4566466",
                        "answer1": "{STUDENT_NUMBER}"
                    },
                    {
                        "questionId": "rc274a6b2dcbe4205910bccab5283eca1",
                        "answer1": "{PURPOSE}"
                    },
                ]
                """;

                answers = answers.Replace("{FIRST_NAME}", FirstName)
                                 .Replace("{LAST_NAME}", LastName)
                                 .Replace("{STUDENT_NUMBER}", StudentNumber);

                if (IsOther)
                    answers = answers.Replace("{PURPOSE}", OtherTxt);
                else
                   answers = answers.Replace("{PURPOSE}", SelectedPurpose);


                //if (NeedJob)
                //{
                //    answers = answers.Replace("{NEED_JOB}", "Yes");
                //}
                //else
                //{
                //    answers = answers.Replace("{NEED_JOB}", "No");
                //}

                await SubmitForm(answers);


			}, canExecute => true);
		}

        private async Task SubmitForm(string answers)
        {
            var url = "https://forms.office.com/formapi/api/0b71261a-495f-4ea9-9911-da844b9402ef/users/ff189b0f-f895-44d7-b908-26ade3b3cdd4/forms('GiZxC19JqU6ZEdqES5QC7w-bGP-V-NdEuQgmreOzzdRUQzFZTDRFODJFSlRHRDhBQzFZTlk0QTFCVi4u')/responses";
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
                    NeedJob = true;
                    IsOther = false;
                    OtherTxt = "";

                    Navigation.NavigateTo<SubmittedViewModel>();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request or response processing
                // Log the exception or display an error message to the user
            }
        }
    }
}
