using HLParserService.Models;
using System;
using System.Collections.Generic;

namespace HLParserService.Helper
{
    public static class PatientHelper
    {


        public static Patient CreatePatient(string name)
        {

            var objPerson = new Patient();
            // Patient Name 
            objPerson.PersonName = new Name();
            objPerson.PersonName.GivenName = name;
            objPerson.PersonName.FamilyName = "Willian";
            objPerson.PersonName.Suffix = "III";


            objPerson.PersonSex = (Sex)Enum.Parse(typeof(Sex), "M");

            // Phone Number
            objPerson.PersonNumber = new PhoneNumber();
            objPerson.PersonNumber.HomeNumber = "99160234242";

            objPerson.PersonAddress = new Address();

            objPerson.PersonAddress.StreetAddress = "2222 HOME STREET";
            objPerson.PersonAddress.City = "ISHPEMING";
            objPerson.PersonAddress.State = "MI";
            objPerson.PersonAddress.ZipCode = "49849";


            objPerson.NextOfKin = new List<Person>();
            Person objKin = new Person();

            objKin.PersonName = new Name();
            objKin.PersonName.GivenName = "NELDA";
            objKin.PersonName.FamilyName = "REDWOOD";

            objKin.Relationship = "SPOUSE";

            objKin.PersonAddress = new Address();
            objKin.PersonAddress.StreetAddress = "6666 HOME STREET";
            objKin.PersonAddress.City = "ISHPEMING";
            objKin.PersonAddress.State = "MI";
            objKin.PersonAddress.ZipCode = "49849";

            objKin.PersonNumber = new PhoneNumber();
            objKin.PersonNumber.HomeNumber = "555-555-5001";
            objKin.PersonNumber.OfficeNumber = "444-555-5001";
            objPerson.NextOfKin.Add(objKin);

            return objPerson;

        }

        public static List<Patient> CreateListOfPatient()
        {
            List<Patient> objPatientList = new List<Patient>();

            for (int i = 0; i <= 5; i++)
            {
                objPatientList.Add(CreatePatient("Smith" + i));
            }

            return objPatientList;
        }

        public static string CreateSampleHL7Message()
        {
            return "MSH|^~&|CohieCentral|COHIE|Clinical Data Provider||201703071246||ADT^A01||P|2.3" +                "PID | 77291 | PATID1234 ^ 9 ^ 55A ||| Willian ^ Smith0 ^^ III || 19680219 | M ||| 2222 HOME STREET^^ ISHPEMING ^ MI ^ 49849 ||^^^^^^ 99160234242" +                "NK1 || REDWOOD ^ NELDA | SPOUSE | 6666 HOME STREET^^ ISHPEMING ^ MI ^ 49849 |^^^^^^ 555 - 555 - 5001 |^^^^^^ 444 - 555 - 5001";
        }
    }
}