using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Resources.Extensions;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using System.Xml.Linq;
using MemberDesktop.View;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace MemberDesktop.Model
{
    internal class MemberRepository
    {
        public List<MemberModel> memberList { get; set; }

        public List<PersonModel> admins { get; set; }

        public List<PersonModel> reviewers { get; set; }

        public MemberRepository()
        {
            memberList = GetDefaultList();
            admins = GetAdmins();
            reviewers = GetReviewers();
        }

        public List<MemberModel>? GetDefaultList()
        {
            return GetMembers(query: "Select * from member order by updated_on desc Limit 20");
        }


        private List<PersonModel> GetAdmins()
        {
            List<PersonModel> admins = new List<PersonModel>();

            string query = "SELECT * from admin";
            DataTable dt = ExecuteQuery(query);
            foreach (DataRow dr in dt.Rows)
            {
                PersonModel person = new PersonModel();
                person.Id = (int)dr["admin_id"];
                person.last_name = (string)dr["last_name"];
                person.first_name = (string)dr["first_name"];
                admins.Add(person);
            }
            return admins;

        }

        private List<PersonModel> GetReviewers()
        {
            List<PersonModel> reviewers = new List<PersonModel>();

            string query = "SELECT * from reviewer";
            DataTable dt = ExecuteQuery(query);
            PersonModel person = new PersonModel();
            person.Id = 0;
            reviewers.Add(person);

            foreach (DataRow dr in dt.Rows)
            {
                person = new PersonModel();
                person.Id = (int)dr["reviewer_id"];
                person.last_name = (string)dr["last_name"];
                person.first_name = (string)dr["first_name"];
                reviewers.Add(person);
            }
            return reviewers;

        }
        private List<MemberModel>? GetMembers(string query)
        {
            List<MemberModel> memberList = new List<MemberModel>();
            DataTable dt = ExecuteQuery(query);

            foreach (DataRow dr in dt.Rows)
            {
                MemberModel memberModel = new MemberModel();
                memberModel.member_id = (int)dr["member_id"];
                memberModel.family_id = (int)dr["family_id"];
                memberModel.last_name = (string)dr["last_name"];
                memberModel.first_name = (string)dr["first_name"];
                memberModel.middle_name = getString(dr, "middle_name");

                memberModel.title_db = getString(dr, "title");
                memberModel.membership_type_db = getString(dr, "membership_type");
                memberModel.membership_status_db = getString(dr, "membership_status");
                memberModel.membership_status_reason_db = getString(dr, "membership_status_reason");


                memberModel.year_started = (short)dr["year_started"];
                memberModel.membership_year = getShort(dr, "membership_year");

                memberModel.street = getString(dr,"address_street");
                memberModel.city = getString(dr,"address_city");
                memberModel.state = getString(dr,"address_st");
                memberModel.zip = getString(dr,"address_zip");

                memberModel.gender_db = getString(dr,"gender");


                memberModel.email = getString(dr,"email");
                memberModel.phone_home = getString(dr,"phone_home");
                memberModel.phone_cell = getString(dr,"phone_cell");
                memberModel.contact_pref_db = getString(dr,"contact_pref");

                memberModel.application_date = getDate(dr, "application_date");


                memberModel.created_by_db = getUShort(dr, "created_by");
                memberModel.created_on = getDate(dr, "created_on");
                memberModel.updated_by_db = getUShort(dr, "updated_by");
                memberModel.updated_on = getDate(dr, "updated_on");
                memberModel.verified_by_db = getUShort(dr, "verified_by");
                memberModel.verified_on = getDate(dr, "verified_on");
                memberModel.approved_on = getDate(dr, "approved_on");


                memberModel.sponsor_1_db = getUShort(dr, "sponsor_1");
                memberModel.sponsor_2_db = getUShort(dr, "sponsor_2");

                memberModel.comments = getString(dr,"comments");


                memberList.Add(memberModel);
            }


            return memberList;
        }


        private DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();

            string connectionString = App.Current.Properties["DB"].ToString() == "TEST" ? "connectionStringTEST" : "connectionStringPROD";
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                if (conn == null)
                {
                    throw new Exception("Connection String is Null. Set the value of Connection String in App.Config");
                }
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            return dt;
        }

        private string getString(DataRow dr, string column)
        {
            try
            {

                return (string)dr[column];
            }
            catch
            {
                return "";
            }
        }
        private short? getShort(DataRow dr, string column)
        {
            try
            {

                return (short)dr[column];
            }
            catch
            {
                return null;
            }
        }

        private ushort? getUShort(DataRow dr, string column)
        {
            try
            {

                return (ushort)dr[column];
            }
            catch
            {
                return null;
            }
        }

        private DateTime? getDate(DataRow dr, string column)
        {
            try
            {
                return (DateTime)dr[column];
            }
            catch
            {
                return null;
            }
        }

        internal void searchMembers(int? member_id, int? family_id, string first_name, string last_name, string email, string phone, string street, string zip)
        {
            string WHERE = "";
            bool hasCriteria = false;

            if (member_id is not null)
            {
                WHERE += "WHERE member_id = " + member_id;
                
            }

            if (family_id is not null)
            {
                WHERE += (WHERE != "") ? " AND " : "WHERE";


                WHERE += " family_id = " + family_id;

              

            }

            if (!string.IsNullOrEmpty(first_name))
            {
                WHERE += (WHERE != "") ? " AND " : "WHERE";

                WHERE += " first_name LIKE '%" + first_name + "%'";
               
            }

            if (!string.IsNullOrEmpty(last_name))
            {
                WHERE += (WHERE != "") ? " AND " : "WHERE";

                WHERE += " last_name LIKE  '%" + last_name + "%'";
            }

            if (!string.IsNullOrEmpty(email))
            {
                WHERE += (WHERE != "") ? " AND " : "WHERE";

                WHERE += " email = '" + email + "'";
            }

            if (!string.IsNullOrEmpty(phone))
            {
                WHERE += (WHERE != "") ? " AND " : "WHERE";

                WHERE += " phone_home = '" + phone + "' OR phone_cell = '" + phone + "'";
            }

            if (!string.IsNullOrEmpty(street))
            {
                WHERE += (WHERE != "") ? " AND " : "WHERE";

                WHERE += " address_street LIKE  '%" + street + "%'";
            }

            if (!string.IsNullOrEmpty(zip))
            {
                WHERE += (WHERE != "") ? " AND " : "WHERE";

                WHERE += " address_zip = '" + zip + "'";
            }
            if (string.IsNullOrEmpty(WHERE))
            {
                memberList = GetDefaultList();
            }
            else
            {
                string query = "Select * from member " + WHERE + "  order by member_id desc Limit 300";
                memberList = GetMembers(query: query);
            }

        }

        internal void RenewMembership(MembershipRenewal renewal, bool isFamilyMembersip)
        {
            _renewMembership(renewal);
            if (isFamilyMembersip)
            {
                string query = "SELECT member_id FROM member WHERE member_id != " + renewal.MemberID + " AND family_id = " + renewal.FamilyID;
                DataTable dt = ExecuteQuery(query);
                foreach (DataRow dr in dt.Rows)
                {
                    int memberID = (int)dr["member_id"];
                    renewal.MemberID = memberID;
                    _renewMembership(renewal);
                }
            }

        }

        internal void _renewMembership(MembershipRenewal renewal)
        {
            string connectionString = App.Current.Properties["DB"].ToString() == "TEST" ? "connectionStringTEST" : "connectionStringPROD";
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                if (conn == null)
                {
                    throw new Exception("Connection String is Null. Set the value of Connection String in App.Config");
                }
                conn.Open();
                string query = "INSERT INTO yearly (member_id, year, renewed_on, comments, created_by, created_on) ";
                query += "VALUES (@memberID,@year,@applicationDate,@comment,@renewedBy,@renewedOn)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@memberId", renewal.MemberID);
                cmd.Parameters.AddWithValue("@year", renewal.Year);
                cmd.Parameters.AddWithValue("@applicationDate", renewal.ApplicationDate);
                cmd.Parameters.AddWithValue("@comment", renewal.Comment);
                cmd.Parameters.AddWithValue("@renewedBy", renewal.RenewedBy);
                cmd.Parameters.AddWithValue("@renewedOn", DateTime.Today);
                int result = cmd.ExecuteNonQuery();
                if (result != 1)
                {
                    throw new Exception("Renewal Failed");
                }

                query = "UPDATE member SET membership_year=@year,  updated_by=@updatedBy, updated_on=@updatedOn WHERE member_id=@memberID";
                cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@memberId", renewal.MemberID);
                cmd.Parameters.AddWithValue("@year", renewal.Year);
                cmd.Parameters.AddWithValue("@updatedBy", renewal.RenewedBy);
                cmd.Parameters.AddWithValue("@updatedOn", DateTime.Today);

                result = cmd.ExecuteNonQuery();
                if (result != 1)
                {
                    throw new Exception("Renewal Failed");
                }



            }
        }

        internal void UpdateMember(MemberModel member)
        {
            string query = "UPDATE member SET family_id=@familyID, last_name=@lastName, first_name=@firstName,middle_name=@middleName,";
            query += "phone_home=@phoneHome,phone_cell=@phoneCell,email=@email,contact_pref=@contactPref,";
            query += "address_street=@street,address_city=@city,address_st=@state,address_zip=@zip,";
            query += "membership_type=@membershipType,membership_status=@membershipStatus,membership_status_reason=@membershipReason,";
            query += "sponsor_1=@sponsor1,sponsor_2=@sponsor2,reviewer_1=@reviewer1,reviewer_2=@reviewer2,";
            query += "updated_by=@updatedBy,updated_on=@updatedOn,";
            query += "comments=@comment WHERE member_id=@memberID";
            string connectionString = App.Current.Properties["DB"].ToString() == "TEST" ? "connectionStringTEST" : "connectionStringPROD";
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                if (conn == null)
                {
                    throw new Exception("Connection String is Null. Set the value of Connection String in App.Config");
                }
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@memberID", member.member_id);
                cmd.Parameters.AddWithValue("@familyID", member.family_id);
                cmd.Parameters.AddWithValue("@lastName", member.last_name);
                cmd.Parameters.AddWithValue("@firstName", member.first_name);
                cmd.Parameters.AddWithValue("@middleName", member.middle_name);
                cmd.Parameters.AddWithValue("@phoneHome", member.phone_home);
                cmd.Parameters.AddWithValue("@phoneCell", member.phone_cell);
                cmd.Parameters.AddWithValue("@email", member.email);
                cmd.Parameters.AddWithValue("@contactPref", member.contact_pref_db);
                cmd.Parameters.AddWithValue("@street", member.street);
                cmd.Parameters.AddWithValue("@city", member.city);
                cmd.Parameters.AddWithValue("@state", member.state);
                cmd.Parameters.AddWithValue("@zip", member.zip);
                cmd.Parameters.AddWithValue("@membershipType", member.membership_type_db);
                cmd.Parameters.AddWithValue("@membershipStatus", member.membership_status_db);
                cmd.Parameters.AddWithValue("@membershipReason", member.membership_status_reason_db);
                cmd.Parameters.AddWithValue("@sponsor1", member.sponsor_1_db);
                cmd.Parameters.AddWithValue("@sponsor2", member.sponsor_2_db);
                cmd.Parameters.AddWithValue("@reviewer1", member.reviewer_1_db);
                cmd.Parameters.AddWithValue("@reviewer2", member.reviewer_2_db);

                cmd.Parameters.AddWithValue("@updatedBy", member.updated_by_db);
                cmd.Parameters.AddWithValue("@updatedOn", DateTime.Today);
                cmd.Parameters.AddWithValue("@comment", member.comments);
                int result = cmd.ExecuteNonQuery();

            }
        }

        internal void AddMember(MemberModel member, bool hasSpouse)
        {

            string connectionString = App.Current.Properties["DB"].ToString() == "TEST" ? "connectionStringTEST" : "connectionStringPROD";
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString))
            {
                if (conn == null)
                {
                    throw new Exception("Connection String is Null. Set the value of Connection String in App.Config");
                }
                conn.Open();

                string query = "INSERT INTO member ";
                query += "(last_name,first_name,middle_name,title,membership_type,membership_year, membership_status,membership_status_reason,year_started,";
                query += "address_street,address_city,address_st,address_zip,gender,email,phone_home,phone_cell,contact_pref,";
                query += "application_date,comments,created_by,created_on,updated_by,updated_on,sponsor_1,sponsor_2)";
                query += " VALUES ";
                query += "(@lastName,@firstName,@middleName,@title,@membershipType,@membershipYear, @membershipStatus,@membershipReason,@yearStarted,";
                query += "@street,@city,@state,@zip,@gender,@email,@phoneHome,@phoneCell,@contactPref,";
                query += "@applicationDate,@comments,@createdBy,@createdOn,@createdBy,@createdOn,@sponsor1,@sponsor2)";

                
                
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@lastName", member.last_name);
                cmd.Parameters.AddWithValue("@firstName", member.first_name);
                cmd.Parameters.AddWithValue("@middleName", member.middle_name);
                cmd.Parameters.AddWithValue("@title", member.title_db);
                cmd.Parameters.AddWithValue("@membershipType", member.membership_type_db);
                cmd.Parameters.AddWithValue("@membershipYear", member.membership_year);
                cmd.Parameters.AddWithValue("@membershipStatus", member.membership_status_db);
                cmd.Parameters.AddWithValue("@membershipReason", member.membership_status_reason_db);
                cmd.Parameters.AddWithValue("@yearStarted", member.year_started);

                cmd.Parameters.AddWithValue("@street", member.street);
                cmd.Parameters.AddWithValue("@city", member.city);
                cmd.Parameters.AddWithValue("@state", member.state);
                cmd.Parameters.AddWithValue("@zip", member.zip);

                cmd.Parameters.AddWithValue("@gender", member.gender_db);
                cmd.Parameters.AddWithValue("@email", member.email);
                cmd.Parameters.AddWithValue("@phoneHome", member.phone_home);
                cmd.Parameters.AddWithValue("@phoneCell", member.phone_cell);

                cmd.Parameters.AddWithValue("@contactPref", member.contact_pref_db);

                cmd.Parameters.AddWithValue("@applicationDate", member.application_date);
                cmd.Parameters.AddWithValue("@comments", member.comments);
                cmd.Parameters.AddWithValue("@createdBy", member.created_by_db);
                cmd.Parameters.AddWithValue("@createdOn", DateTime.Today);

                cmd.Parameters.AddWithValue("@sponsor1", member.sponsor_1_db);
                cmd.Parameters.AddWithValue("@sponsor2", member.sponsor_2_db);

                int result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new Exception("Add Failed");
                }
                int memberID = (int)cmd.LastInsertedId;
                int familyID = memberID;


                //update family id
                query = "UPDATE member SET family_id=@familyID WHERE member_id=@memberID";
                cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@memberID", memberID);
                cmd.Parameters.AddWithValue("@familyID", familyID);
                result = cmd.ExecuteNonQuery();
                if (result != 1)
                {
                    throw new Exception("Add Failed");
                }

                if (hasSpouse)
                {

                    query = "INSERT INTO member ";
                    query += "(last_name,first_name,middle_name,title,membership_type,membership_year, membership_status,membership_status_reason,year_started,";
                    query += "address_street,address_city,address_st,address_zip,gender,email,phone_home,phone_cell,contact_pref,";
                    query += "application_date,comments,created_by,created_on,updated_by,updated_on,sponsor_1,sponsor_2)";
                    query += " VALUES ";
                    query += "(@lastName,@firstName,@middleName,@title,@membershipType,@membershipYear, @membershipStatus,@membershipReason,@yearStarted,";
                    query += "@street,@city,@state,@zip,@gender,@email,@phoneHome,@phoneCell,@contactPref,";
                    query += "@applicationDate,@comments,@createdBy,@createdOn,@createdBy,@createdOn,@sponsor1,@sponsor2)";

                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@lastName", member.spouse.last_name);
                    cmd.Parameters.AddWithValue("@firstName", member.spouse.first_name);
                    cmd.Parameters.AddWithValue("@middleName", member.spouse.middle_name);
                    cmd.Parameters.AddWithValue("@title", member.spouse.title_db);
                    cmd.Parameters.AddWithValue("@membershipType", member.membership_type_db);
                    cmd.Parameters.AddWithValue("@membershipYear", member.membership_year);
                    cmd.Parameters.AddWithValue("@membershipStatus", member.membership_status_db);
                    cmd.Parameters.AddWithValue("@membershipReason", member.membership_status_reason_db);
                    cmd.Parameters.AddWithValue("@yearStarted", member.year_started);

                    cmd.Parameters.AddWithValue("@street", member.street);
                    cmd.Parameters.AddWithValue("@city", member.city);
                    cmd.Parameters.AddWithValue("@state", member.state);
                    cmd.Parameters.AddWithValue("@zip", member.zip);

                    cmd.Parameters.AddWithValue("@gender", member.spouse.gender_db);
                    cmd.Parameters.AddWithValue("@email", member.spouse.email);
                    cmd.Parameters.AddWithValue("@phoneHome", member.spouse.phone_home);
                    cmd.Parameters.AddWithValue("@phoneCell", member.spouse.phone_cell);

                    cmd.Parameters.AddWithValue("@contactPref", member.spouse.contact_pref_db);

                    cmd.Parameters.AddWithValue("@applicationDate", member.application_date);
                    cmd.Parameters.AddWithValue("@comments", member.comments);
                    cmd.Parameters.AddWithValue("@createdBy", member.created_by_db);
                    cmd.Parameters.AddWithValue("@createdOn", DateTime.Today);

                    cmd.Parameters.AddWithValue("@sponsor1", member.sponsor_1_db);
                    cmd.Parameters.AddWithValue("@sponsor2", member.sponsor_2_db);

                    result = cmd.ExecuteNonQuery();

                    if (result != 1)
                    {
                        throw new Exception("Add Failed");
                    }
                    memberID = (int)cmd.LastInsertedId;
                    query = "UPDATE member SET family_id=@familyID WHERE member_id=@memberID";
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@memberID", memberID);
                    cmd.Parameters.AddWithValue("@familyID", familyID);
                    result = cmd.ExecuteNonQuery();
                    if (result != 1)
                    {
                        throw new Exception("Add Failed");
                    }
                }

            }
        }
    
        internal DataTable GetAuditHx(int memberID)
        {
            string query = "SELECT * from member_history WHERE member_id = " + memberID;
            return ExecuteQuery(query);
        }

        internal DataTable GetYearlyMembership(int memberID)
        {
            string query = "SELECT * from yearly where member_id = " + memberID;
            return ExecuteQuery(query);
        }
    }
}
