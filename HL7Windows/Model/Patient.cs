using HL7Windows.Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HL7Windows.Model
{

    public class Person
    {

        public Name PersonName { get; set; }

        public Sex PersonSex { get; set; }

        public DateTime DateOfBirth { get; set; }

        public PhoneNumber PersonNumber { get; set; }

        public Address PersonAddress { get; set; }

        public List<Person> NextOfKin { get; set; }

        public string Relationship { get; set; }

    }

    public enum Sex { M, F }

    public class Address
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }
    public class PhoneNumber
    {
        public string HomeNumber { get; set; }
        public string OfficeNumber { get; set; }
    }
    public class Name
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Suffix { get; set; }


    }

    public class Patient : Person
    {
        /// <summary>
        /// Retrieve the HL7 Message from the HL7Parser Service
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public string GetHL7Message(int patientId)
        {
            Uri uri = new Uri("http://localhost/HL7Parser/api/patient/");
            string data = HL7APIClient.GetAsync<string>(uri, patientId.ToString());
            data = data.Replace("\\r", Environment.NewLine);
            data = Regex.Unescape(data);
            data = data.Replace("\"", "");
            return data;
        }
        /// <summary>
        /// Parses the HL7 Message
        /// </summary>
        /// <param name="hl7Message"></param>
        /// <returns></returns>
        public Patient ParseHL7Message(string hl7Message)
        {
            Uri uri = new Uri("http://localhost/HL7Parser/api/patient/");
            Patient data = HL7APIClient.PostAsync<Patient>(uri, "", hl7Message);
            return data;
        }
    }
}
