using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberDesktop.Model
{
    public class SpouseModel: INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public string Error => string.Empty;

        public int _numErrors;
        public bool HasNoError { get { return _numErrors == 0; } set { } }

        public string this[string columnName]
        {

            get
            {
                string error = string.Empty;

                switch (columnName)
                {

                    case nameof(first_name):
                        if (string.IsNullOrWhiteSpace(first_name) || first_name.Trim().Length < 2)
                            error = "First name cannot be less than two characters.";
                        if (first_name?.Length > 50)
                            error = "The name must be less than 50 characters.";
                        break;

                    case nameof(last_name):
                        if (string.IsNullOrWhiteSpace(last_name) || last_name.Trim().Length < 2)
                            error = "Last name cannot be less than two characters.";
                        break;
       
                }
                if (string.IsNullOrEmpty(error))
                {
                    _numErrors--;
                }
                else
                {
                    _numErrors++;
                }
                _numErrors = 0;
                OnPropertyRaised("HasNoError");

                return error;
            }
        }

        public string MemberTitle
        {
            get
            {
                switch (this.title_db)
                {
                    case "mr":
                        return "Mr.";
                    case "ms":
                        return "Ms.";
                    case "mrs":
                        return "Mrs.";
                    case "dr":
                        return "Dr.";

                    default:
                        return "";
                }
            }
            set { }
        }


        public string title_db { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }

        public string Gender
        {
            get
            {

                switch (this.gender_db)
                {
                    case "M":
                        return "Male";
                    case "F":
                        return "Female";


                    default:
                        return "Gender: <UNKNOWN>";
                }
            }
            set { }

        }
        private string _gender;
        public string gender_db
        {
            get { return _gender; }
            set
            {
                _gender = value;

            }
        }




        public string email { get; set; }

        private string _phone_home = "";
        public string phone_home
        {
            get
            {
                return _phone_home;
            }

            set
            {
                _phone_home = RemovePhoneFormatting(value);
                OnPropertyRaised("phone_home");
                OnPropertyRaised("PhoneHome");
            }
        }

        public string PhoneHome
        {
            get
            {
                return displayFormatPhone(_phone_home)
                    ;
            }
            set { }
        }



        private string _phone_cell = "";
        public string phone_cell
        {
            get

            {

                return _phone_cell;
            }
            set
            {
                _phone_cell = this.RemovePhoneFormatting(value);
                OnPropertyRaised("phone_cell");
                OnPropertyRaised("PhoneCell");
            }
        }

        public string PhoneCell
        {
            get
            {
                return displayFormatPhone(this._phone_cell)
                    ;
            }
            set { }
        }

        private string displayFormatPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return "";
            }

            return Convert.ToInt64(phone).ToString("(###)-###-####");
        }

        public string RemovePhoneFormatting(string phone)
        {

            string phoneDigits = "";
            for (int i = 0; i < phone.Length; i++)
            {
                if (char.IsDigit(phone[i]))
                {
                    phoneDigits += phone[i];
                }
            }
            return phoneDigits;
        }


        public string contact_pref_db { get; set; }

    }
}
