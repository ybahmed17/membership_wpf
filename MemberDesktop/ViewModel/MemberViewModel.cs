using MemberDesktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemberDesktop.ViewModel
{

    public class MemberViewModel
    {
        public ObservableCollection<MemberModel> members { get; set; }

        public ObservableCollection<PersonModel> admins { get; set; }
       
        public ObservableCollection<PersonModel> reviewers { get; set; }
        private MemberRepository memberRepository { get; set; }

        public ObservableCollection<Category> membershipType { get; set; }

        public ObservableCollection<Category> membershipStatus { get; set; }    

        public ObservableCollection<Category> membershipReason { get; set; }        

        public ObservableCollection<Category> contactPreference { get; set; }

        public ObservableCollection<Category> gender { get; set; }

        public ObservableCollection<Category> title { get; set; }

        public ObservableCollection<Category> renewal_year { get; set; }
        public MemberViewModel()
        {
            memberRepository = new MemberRepository();
            members = new ObservableCollection<MemberModel>(memberRepository.memberList);
            members.CollectionChanged += Members_CollectionChanged;       // Event Handler for change in collection
            admins = new ObservableCollection<PersonModel>(memberRepository.admins);
            reviewers = new ObservableCollection<PersonModel>(memberRepository.reviewers);
            InitializeCategories();
        }

        private void InitializeCategories()
        {
           membershipType = new ObservableCollection<Category>();
            membershipType.Add(new Category("", ""));
            membershipType.Add(new Category("life", "Life"));
            membershipType.Add(new Category("single","Single"));
            membershipType.Add(new Category("family", "Family"));

            membershipStatus = new ObservableCollection<Category>();
            membershipStatus.Add(new Category("", ""));
            membershipStatus.Add(new Category("active", "Active"));
            membershipStatus.Add(new Category("inactive", "Inactive"));
            

            membershipReason = new ObservableCollection<Category>();
            membershipReason.Add(new Category("", ""));
            membershipReason.Add(new Category("moved", "Moved"));
            membershipReason.Add(new Category("deceased", "Deceased"));
            membershipReason.Add(new Category("removed", "Removed"));

            contactPreference = new ObservableCollection<Category>();
            contactPreference.Add(new Category("", ""));
            contactPreference.Add(new Category("email", "Email"));
            contactPreference.Add(new Category("sms", "SMS"));
            contactPreference.Add(new Category("phone", "Phone"));


            gender = new ObservableCollection<Category>();
            gender.Add(new Category("", ""));
            gender.Add(new Category("M", "Male"));
            gender.Add(new Category("F", "Female"));
            gender.Add(new Category("U", "Unknown"));


            title = new ObservableCollection<Category>();
            title.Add(new Category("", ""));
            title.Add(new Category("mr", "Mr."));
            title.Add(new Category("mrs", "Mrs."));
            title.Add(new Category("ms", "Ms."));
            title.Add(new Category("dr", "Dr."));

            renewal_year = new ObservableCollection<Category>();
            renewal_year.Add(new Category("2023", "2023"));
            renewal_year.Add(new Category("2024", "2024"));
            renewal_year.Add(new Category("2025", "2025"));



        }

        public void searchMembers(int? member_id, int? family_id, string first_name, string last_name, string email, string phone, string street, string zip)
        {
           
            memberRepository.searchMembers(member_id, family_id, first_name, last_name, email,phone, street, zip);
            RefreshMembers();


        }

        public DataTable GetAuditHx(int memberID)
        {
            return memberRepository.GetAuditHx(memberID);
        }

        public DataTable GetYearlyMembership(int memberID)
        {
            return memberRepository.GetYearlyMembership(memberID);
        }


        private void RefreshMembers()
        {

            members.Clear();
            foreach (var elm in memberRepository.memberList)
            {
                members.Add(elm);
            }
        }

        /*
         * Function: Search for the query string in Movies Collection
         * Saves time and resources by searching in Collection in memory
         * rather than in database
         */
        public List<MemberModel> searchRepo(int searchQuery)
        {
            
            List<MemberModel> MemberList =                // Temporary list for storing results returned from search query
                (from tempMember in members
                 where tempMember.member_id == searchQuery
                 select tempMember).ToList();
            return MemberList;
        }

        /*
         * Function: Add Record to Collection and Database
         */
        public void AddRecordToRepo(MemberModel member)
        {
            if (member == null)
                throw new ArgumentNullException("Error: The argument is Null");
            members.Add(member);
        }

        /*
         * Function: Delete Records from Collection and Database
         */
        public void DeleteRecordFromRepo(int memberID)
        {
            if (memberID < 0)
                throw new Exception("Record ID must be non-negative");

            int index = 0;
            while (index < members.Count)
            {
                if (members[index].member_id == memberID)
                {
                    members.RemoveAt(index);
                    break;
                }
                index++;
            }
        }

        /*
         * Function: Updates the Object in Collection
         * with refernce to the id
         */
        public void UpdateRecordInRepo(MemberModel member)
        {
            if (member.member_id < 0)
                throw new Exception("Error: ID cannot be negative");

            int index = 0;
            while (index < members.Count)
            {
                if (members[index].member_id == member.member_id)
                {
                    members[index] = member;
                    break;
                }
                index++;
            }
        }

        /*
         * Event Handler: Handles the CollectionChanged event of ObservableCollection
         * Updates the Database if any change is made to the Movies Collection
         * Thus removes unncecessary burden of accessing Database
         */
      
 

        private void Members_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int newIndex = e.NewStartingIndex;
                // memberRepository.addNewRecord(members[newIndex]);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                List<MemberModel> tempListOfRemovedItems = e.OldItems.OfType<MemberModel>().ToList();
                // memberRepository.DelRecord(tempListOfRemovedItems[0].Id);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                List<MemberModel> tempListOfMembers = e.NewItems.OfType<MemberModel>().ToList();
               // memberRepository.UpdateRecord(tempListOfMembers[0]);      // As the IDs are unique, only one row will be effected hence first index only
            }
        }

        internal void RenewMembership(MembershipRenewal renewal,bool isFamilyMembersip)
        {
            memberRepository.RenewMembership(renewal,isFamilyMembersip);
        }

        internal void UpdateMember(MemberModel member)
        {
            memberRepository.UpdateMember(member);
        }

        internal void AddMember(MemberModel member, bool hasSpouse)
        {
            memberRepository.AddMember(member, hasSpouse);
        }
    }
}
