using HLParserService.Helper;
using HLParserService.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace HLParserService.Controllers
{

    public class PatientController : ApiController
    {
        List<Patient> objPersonList = PatientHelper.CreateListOfPatient();
        // GET api/patient/5
        /// <summary>
        /// Retrieves the patient record and emits a HL7 Message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Get(int id)
        {
            Patient objPatient = objPersonList[id];
            return objPatient.Encode(objPatient);
        }

        public Patient Post([FromBody] string hl7Message)
        {
            Patient objPatient = new Patient();
            return objPatient.Parse(hl7Message);
        }

    }
}
