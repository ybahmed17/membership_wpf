
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MemberDesktop.Model


{
    //public enum Title
    //{
    //    MR,
    //    MRS,
    //    MISS,
    //    DR,
    //    NA
    //}

    //public enum MembershipType
    //{
    //    LIFE,
    //    SINGLE,
    //    FAMILY
    //}

    //public enum MemberStatus
    //{
    //    ACTIVE,
    //    INACTIVE,
    //    PENDING,
    //    NA
    //}


    //public enum MemberStatusReason
    //{

    //    MOVED,
    //    DECEASED,
    //    NA
    //}


    public class MemberModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public int member_id { get; set; }
        public int family_id { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }


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



        public string MembershipType
        {
            get
            {
                switch (this.membership_type_db)
                {
                    case "life":
                        return "Life";
                    case "single":
                        return "Single";
                    case "family":
                        return "Family";

                    default:
                        return "(UNKNOWN)";
                }
            }
            set { }
        }
        public string membership_type_db { get; set; }

        public string MembershipStatus
        {
            get
            {
                if (this.membership_type_db == "life")
                {
                    if (this.membership_status_db == "inactive")
                    {
                        if (this.membership_status_reason_db == "moved")
                        {
                            return "Inactive - Moved";
                        }
                        else if (this.membership_status_reason_db.Equals("deceased"))
                        {
                            return "Inactive - Deceased";
                        }

                    }
                    else
                    {
                        return "Active";
                    }
                }
                else
                {
                    return this.membership_year == null ? "XXXX" : this.membership_year.ToString();
                }
                return "XXX";
            }
        }

        //public MemberStatus membership_status { get; set; }
        public string membership_status_db { get; set; }

        //public MemberStatusReason membership_status_reason { get; set; }
        public string membership_status_reason_db { get; set; }

        public short year_started { get; set; }

        public short? membership_year { get; set; }

        public string street { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }

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
        public string gender_db { get { return _gender; } set
            {
                _gender = value;
               
            }
        }

        public SpouseModel? spouse;


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

        public string ContactPreference
        {
            get
            {
                switch (this.contact_pref_db)
                {
                    case "sms":
                        return "SMS";
                    case "email":
                        return "Eamil";
                    case "phone":
                        return "Phone";
                    case "":
                        return "-";

                    default:
                        return "";
                }
            }
            set { }
        }

        public DateTime? application_date { get; set; }


        public string created_by { get; set; }
        public ushort? created_by_db { get; set; }

        public DateTime? created_on { get; set; }

        public string updated_by { get; set; }
        public ushort? updated_by_db { get; set; }

        public DateTime? updated_on { get; set; }

        public string verified_by { get; set; }
        public ushort? verified_by_db { get; set; }

        public DateTime? verified_on { get; set; }


        public DateTime? approved_on { get; set; }

       
        public ushort? sponsor_1_db { get; set; }
        public string sponsor_1
        {
            get
            {
                if (sponsor_1_db==null)
                {
                    return "";
                }
                else
                {
                    return sponsor_1_db.ToString();
                }
                
            }
            set
            {
                ushort x;
                if (ushort.TryParse(value, out x))
                {
                    sponsor_1_db = x;
                }
                else
                {
                    sponsor_1_db = null;
                }
            }
        }

        public ushort? sponsor_2_db { get; set; }

        public string sponsor_2
        {
            get
            {
                if (sponsor_2_db == null)
                {
                    return "";
                }
                else
                {
                    return sponsor_2_db.ToString();
                }

            }
            set
            {
                ushort x;
                if (ushort.TryParse(value, out x))
                {
                    sponsor_2_db = x;
                }
                else
                {
                    sponsor_2_db = null;
                }
            }
        }

        public string comments { get; set; }

        public ushort? reviewer_1_db { get; set; }

        public ushort? reviewer_2_db { get; set; }

        public string Error => string.Empty;

        public int _numErrors;
        public bool HasNoError { get { return _numErrors == 0; } set {  } }

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
                    case nameof(sponsor_1):
                        if (string.IsNullOrEmpty(sponsor_1))
                            error = "Sponsor required";
                        break;
                    case nameof(sponsor_2):
                        if (string.IsNullOrEmpty(sponsor_2))
                            error = "Sponsor required";
                        break;
                }
               if  (string.IsNullOrEmpty(error))
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
    }
}
