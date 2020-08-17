using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeRegistrationApp.Models
{
    public partial class EmployeeDetails
    {

        [DisplayName("Employee Id")]
        [Required]
        public int EmpID { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please give the First Name:")]
        public string Firstname { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please give the Last Name")]
        public string Lastname { get; set; }

        [DisplayName("Date Of Birth")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Dob { get; set; }

        [DisplayName("Contact No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Please provide the Contact Mobile No:")]
        public long Contactno { get; set; }

        [DisplayName("Address Line1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line2")]
        public string AddressLine2 { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Pincode")]
        [RegularExpression(@"^(\d{6})$", ErrorMessage = "Enter Valid Pincode")]
        public Nullable<long> Pincode { get; set; }

        [DisplayName("Email-ID")]
        public string Email { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }


        public System.DateTime CreatedOn { get; set; }

        public System.DateTime ModifiedOn { get; set; }


    }
}