using HL7Windows.Model;
using System;
using System.Text;
using System.Windows.Forms;

namespace HL7Windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(Object sender, EventArgs e)
        {
            ////url =  http://localhost/HL7Parser/api/patient/3        
        }

        private void comboBox1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            int patientId = cb.SelectedIndex;
            Patient objPatient = new Patient();
            string hl7Message = objPatient.GetHL7Message(patientId);
            textBox1.Text = hl7Message;
            objPatient = objPatient.ParseHL7Message(hl7Message);
            textBox2.Text = FormatPatient(objPatient);
        }

        /// <summary>
        /// Formatting the patient 
        /// </summary>
        /// <param name="objPatient"></param>
        /// <returns></returns>
        private string FormatPatient(Person objPatient)
        {
            StringBuilder objStringBuilder = new StringBuilder();
            objStringBuilder.Append("Patient Details :" + System.Environment.NewLine);
            objStringBuilder.Append("Family Name : " + objPatient.PersonName.FamilyName + System.Environment.NewLine);
            objStringBuilder.Append("Given Name : " + objPatient.PersonName.GivenName + System.Environment.NewLine);
            objStringBuilder.Append("Suffix : " + objPatient.PersonName.Suffix + System.Environment.NewLine);
            objStringBuilder.Append("Sex : " + objPatient.PersonSex + System.Environment.NewLine);
            objStringBuilder.Append(System.Environment.NewLine + "Patient Address : " + System.Environment.NewLine);
            objStringBuilder.Append(objPatient.PersonAddress.StreetAddress + ", " + objPatient.PersonAddress.State + ", " + objPatient.PersonAddress.City + ", " + objPatient.PersonAddress.ZipCode + System.Environment.NewLine);

            foreach (var kin in objPatient.NextOfKin)
            {
                objStringBuilder.Append(System.Environment.NewLine);
                objStringBuilder.Append("Next of Kin Details :" + System.Environment.NewLine);
                objStringBuilder.Append("Family Name : " + kin.PersonName.FamilyName + System.Environment.NewLine);
                objStringBuilder.Append("Given Name : " + kin.PersonName.GivenName + System.Environment.NewLine);
                objStringBuilder.Append("RelationShip : " + kin.Relationship + System.Environment.NewLine);
                objStringBuilder.Append(System.Environment.NewLine + "Kin's Address : " + System.Environment.NewLine);
                objStringBuilder.Append(kin.PersonAddress.StreetAddress + ", " + kin.PersonAddress.State + ", " + kin.PersonAddress.City + ", " + kin.PersonAddress.ZipCode + System.Environment.NewLine);

            }
            return objStringBuilder.ToString();
        }

    }

}

