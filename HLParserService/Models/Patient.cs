using HLParserService.Service;
using NHapi.Base.Parser;
using NHapi.Model.V23.Message;
using NHapi.Model.V23.Segment;
using System;
using System.Collections.Generic;

namespace HLParserService.Models
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

    public class Patient : Person, IHL7Parser<Patient>
    {
        /// <summary>
        /// Creates a HL Message out of an object model
        /// </summary>
        /// <param name="objToEncode"></param>
        /// <returns></returns>
        public String Encode(Patient objToEncode)
        {
            var patient = objToEncode;
            PipeParser parser = new PipeParser();

            // HL7 Message Header Settings

            ADT_A04 qry = new ADT_A04();
            qry.MSH.MessageType.MessageType.Value = "ADT";
            qry.MSH.MessageType.TriggerEvent.Value = "A01";
            qry.MSH.FieldSeparator.Value = "|";
            qry.MSH.VersionID.Value = "2.3";
            qry.MSH.SendingApplication.NamespaceID.Value = "CohieCentral";
            qry.MSH.SendingFacility.NamespaceID.Value = "COHIE";
            qry.MSH.ReceivingApplication.NamespaceID.Value = "Clinical Data Provider";
            qry.MSH.EncodingCharacters.Value = @"^~\&";
            qry.MSH.DateTimeOfMessage.TimeOfAnEvent.SetLongDate(DateTime.Now);
            qry.MSH.ProcessingID.ProcessingID.Value = "P";
            qry.PID.SetIDPatientID.Value = "77291";
            qry.PID.PatientIDExternalID.CheckDigit.Value = "9";
            qry.PID.PatientIDExternalID.ID.Value = "PATID1234";
            qry.PID.PatientIDExternalID.CodeIdentifyingTheCheckDigitSchemeEmployed.Value = "55A";

            // Patient Name
            qry.PID.GetPatientName(0).GivenName.Value = patient.PersonName.GivenName;
            qry.PID.GetPatientName(0).FamilyName.Value = patient.PersonName.FamilyName;
            qry.PID.GetPatientName(0).SuffixEgJRorIII.Value = patient.PersonName.Suffix;
            qry.PID.Sex.Value = patient.PersonSex.ToString();
            qry.PID.GetPhoneNumberHome(0).PhoneNumber.Value = patient.PersonNumber.HomeNumber;
            qry.PID.DateOfBirth.TimeOfAnEvent.Value = "19680219";


            // Patient Address
            qry.PID.GetPatientAddress(0).StreetAddress.Value = patient.PersonAddress.StreetAddress;
            qry.PID.GetPatientAddress(0).City.Value = patient.PersonAddress.City;
            qry.PID.GetPatientAddress(0).StateOrProvince.Value = patient.PersonAddress.State;
            qry.PID.GetPatientAddress(0).ZipOrPostalCode.Value = patient.PersonAddress.ZipCode;


            // Kin Details

            qry.AddNK1();
            NK1 objNextOfKin = qry.GetNK1();
            Person objKin = patient.NextOfKin[0];
            objNextOfKin.GetName(0).GivenName.Value = objKin.PersonName.GivenName;
            objNextOfKin.GetName(0).FamilyName.Value = objKin.PersonName.FamilyName;

            objNextOfKin.Relationship.Identifier.Value = objKin.Relationship;
            objNextOfKin.GetAddress(0).StreetAddress.Value = objKin.PersonAddress.StreetAddress;
            objNextOfKin.GetAddress(0).City.Value = objKin.PersonAddress.City;
            objNextOfKin.GetAddress(0).StateOrProvince.Value = objKin.PersonAddress.State;
            objNextOfKin.GetAddress(0).ZipOrPostalCode.Value = objKin.PersonAddress.ZipCode;

            objNextOfKin.GetPhoneNumber(0).PhoneNumber.Value = objKin.PersonNumber.HomeNumber;
            objNextOfKin.GetBusinessPhoneNumber(0).PhoneNumber.Value = objKin.PersonNumber.OfficeNumber;


            var message = parser.Encode(qry);
            return message;
        }

        /// <summary>
        /// Parses the HL7 Message and transforms it to an object model
        /// </summary>
        /// <param name="hl7Message"></param>
        /// <returns></returns>
        public Patient Parse(String hl7Message)
        {
            Patient objPatient = new Patient();

            PipeParser parser = new PipeParser();

            var parsedMessage = new ADT_A01();
            parsedMessage = (ADT_A01)parser.Parse(hl7Message);

            // Patient Name Details
            objPatient.PersonName = new Name();
            objPatient.PersonName.GivenName = parsedMessage.PID.GetPatientName(0).GivenName.Value;
            objPatient.PersonName.FamilyName = parsedMessage.PID.GetPatientName(0).FamilyName.Value;
            objPatient.PersonName.Suffix = parsedMessage.PID.GetPatientName(0).SuffixEgJRorIII.Value;


            objPatient.PersonSex = (Sex)Enum.Parse(typeof(Sex), parsedMessage.PID.Sex.Value);

            // Phone Number
            objPatient.PersonNumber = new PhoneNumber();
            objPatient.PersonNumber.HomeNumber = parsedMessage.PID.GetPhoneNumberHome(0).PhoneNumber.Value;

            objPatient.PersonAddress = new Address();

            objPatient.PersonAddress.StreetAddress = parsedMessage.PID.GetPatientAddress(0).StreetAddress.Value;
            objPatient.PersonAddress.City = parsedMessage.PID.GetPatientAddress(0).City.Value;
            objPatient.PersonAddress.State = parsedMessage.PID.GetPatientAddress(0).StateOrProvince.Value;
            objPatient.PersonAddress.ZipCode = parsedMessage.PID.GetPatientAddress(0).ZipOrPostalCode.Value;


            // Kin Details

            objPatient.NextOfKin = new List<Person>();
            Person objKin = new Person();

            objKin.PersonName = new Name();
            objKin.PersonName.GivenName = parsedMessage.GetNK1(0).GetName(0).GivenName.Value;
            objKin.PersonName.FamilyName = parsedMessage.GetNK1(0).GetName(0).FamilyName.Value;

            objKin.Relationship = parsedMessage.GetNK1(0).Relationship.Identifier.Value;

            objKin.PersonAddress = new Address();
            objKin.PersonAddress.StreetAddress = parsedMessage.GetNK1(0).GetAddress(0).StreetAddress.Value;
            objKin.PersonAddress.City = parsedMessage.GetNK1(0).GetAddress(0).City.Value;
            objKin.PersonAddress.State = parsedMessage.GetNK1(0).GetAddress(0).StateOrProvince.Value;
            objKin.PersonAddress.ZipCode = parsedMessage.GetNK1(0).GetAddress(0).ZipOrPostalCode.Value;

            objKin.PersonNumber = new PhoneNumber();
            objKin.PersonNumber.HomeNumber = parsedMessage.GetNK1(0).GetPhoneNumber(0).PhoneNumber.Value;
            objKin.PersonNumber.OfficeNumber = parsedMessage.GetNK1(0).GetBusinessPhoneNumber(0).PhoneNumber.Value;

            objPatient.NextOfKin.Add(objKin);

            return objPatient;
        }
    }
}