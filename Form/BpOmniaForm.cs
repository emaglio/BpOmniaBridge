﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BPS;
using BpOmniaBridge.CommandList;
using BpOmniaBridge.Utility;
using System.Runtime.InteropServices;
using BpOmniaBridge.CommandUtility;

namespace BpOmniaBridge
{
    public partial class BpOmniaForm : Form
    {
        public BPS.BPDevice app;
        private IPatient patient;
        private ISpiro spiro;
        private ITest currentTest;
       
        public BpOmniaForm()
        {
            InitializeComponent();
            Utility.Utility.CreateUtilityFolders();
            Utility.Utility.CreateLogFile();
            bool omnia = Utility.Utility.RunOmnia();
            if (omnia)
                try
                {
                    Type type = Type.GetTypeFromProgID("BPS.BPDevice");
                    app = (BPS.BPDevice)Activator.CreateInstance(type);
                }
                catch (COMException)
                {
                    try
                    {
                        Type type = Type.GetTypeFromProgID("BPS.BPDevice");
                        app = new BPS.BPDevice();// (BPDevice.Device.BPDevice)Activator.CreateInstance(type);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + Environment.NewLine + ex.InnerException.Message + " BPDeviceStart");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace, "Bp Issue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            if (app != null)
            {
                Utility.Utility.Log("BP instance created");
                app.eOnNewTest += new BPS.BPDevice.DeviceEventHandler(app_eOnNewTest);
             
            }
            else
                closeApp();


        }

        private void BpOmniaForm_Load(object sender, EventArgs e)
        {

        }

        private void statusBar_TextChanged(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            closeApp();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            app.NewTest(1, 1, 4);
        }

        #region Method
        public void closeApp()
        {
            this.Close();
            Utility.Utility.Log("Close bridge");
        }

        private void app_eOnNewTest()
        {
            Utility.Utility.Log("BP => new_test");
            currentTest = app.CurrentTest;
            patient = currentTest.Patient;
            CreateSelectSubjectAndVistCard();
        }

        private bool CreateSelectSubjectAndVistCard()
        {
            string[] prmNames = { "ID", "FirstName", "MiddleName", "LastName", "DayOfBirth", "Gender", "EthnicGroup", "Height", "Weight" };
            var id = patient.InternalId.ToString();
            var name = patient.Name.First;
            var middlename = patient.Name.Middle;
            var lastname = patient.Name.Last;
            var dob = patient.DOB.ToString("yyyyMMdd");
            var gender = patient.Gender.ToString();
            var ethnicity = "Caucasian";
            var height = patient.Height.ToString();
            var weight = patient.Weight.ToString();
            //string[] prmValues = {id, name, lastname, dob, gender, ethnicity, height, weight };
            string[] prmValues = { "", "SUBJECT", "", "DEMO", "19670304", "Male", ethnicity, "180", "80"};

            //check if user is preset in DB
            Archive archive = new Archive(prmNames, prmValues);
            string subjectID = archive.CreateSubject();
            bool done = false;
            if (subjectID != "NAK")
            {
                archive.SetRecordID(subjectID);
                string visitID = archive.TodayVisitCard();

                if (visitID != "NAK")
                {
                    //Select visit card
                    string[] key = new string[] { "RecordID" };
                    string[] value = new string[] { visitID };
                    done = new CommandList.CommandList().SelectVisit(key, value);
                }
            }

            return done;
        }

        #endregion

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
