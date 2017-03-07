using FluentAssertions;
using HLParserService.Helper;
using HLParserService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace HL7Tests
{
    [TestClass]
    public class HL7ServiceTests
    {
        [TestMethod]
        public void CreateHL7MessageAndVerify()
        {
            List<Patient> objPatientList = PatientHelper.CreateListOfPatient();
            foreach (var objPatient in objPatientList)
            {
                string hl7Message = objPatient.Encode(objPatient);
                Patient parsedObj = objPatient.Parse(hl7Message);
                objPatient.ShouldBeEquivalentTo(parsedObj);
            }

        }

        [TestMethod]
        public void ParseHL7Message()
        {
            string hl7Message = PatientHelper.CreateSampleHL7Message();
            List<Patient> objPatientList = PatientHelper.CreateListOfPatient();
            Patient objPatientToValidate = objPatientList[0];
            Patient parsedObj = objPatientToValidate.Parse(hl7Message);
            parsedObj.ShouldBeEquivalentTo(objPatientToValidate);

        }


    }
}
