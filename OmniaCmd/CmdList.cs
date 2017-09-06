﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpOmniaBridge
{
    public class CommandList
    {
        #region System Commands
        public void CloseApplication()
        {
            var cmd = new Command("close_app", "System", "CloseApplication");
            cmd.Send();
        }

        public void CreateSessionKey()
        {
            var cmd = new Command("create_key", "System", "CreateSessionKey");
            cmd.Send();
        }

        public void GetCurrentUser()
        {
            var cmd = new Command("current_user", "System", "GetCurrentUser");
            cmd.Send();
        }

        public void GetPermissionLevel()
        {
            var cmd = new Command("perm_level", "System", "GetPermissionLevel");
            cmd.Send();
        }

        public Command Login(string username, string password)
        {
            var cmd = new Command("login", "System", "Login");
            string[] parNames = new string[] { "Username", "Password" };
            string[] parValues = new string[] { username, password};
            cmd.AddParams(parNames, parValues);
            cmd.Send();

            return cmd;
        }

        public void Logout()
        {
            var cmd = new Command("logout", "System", "Logout");
            cmd.Send();
        }
        #endregion

        #region Archive Commands

        public bool ChangeSubject(Guid recordID, string[] subjNames, string[] subjValues)
        {
            var cmd = new Command("change_subj", "Archive", "ChangeSubject");
            List<string> listNames = subjNames.ToList();
            List<string> listValues = subjValues.ToList();
            listNames.Insert(0,"RecordID");
            listValues.Insert(0, recordID.ToString());
            string[] parNames = listNames.ToArray();
            string[] parValues = listValues.ToArray();
            cmd.AddParams(parNames, parValues);
            cmd.Send();

            string[] resultKeys = new string[] { "Result" };
            var answer = new ReadCommands("change_subj", "Archive", "ChangeSubject");
            return answer.ResultValues[0] == "ACK";
        }

        public Command SelectCreateSubject(string[] subjNames, string[] subjValues)
        {
            var cmd = new Command("select_create_subj", "Archive", "SelectCreateSubject");
            cmd.AddParams(subjNames, subjValues);
            cmd.Send();

            return cmd;
        }

        public Command GetSubjectVisitList(string[] subjNames, string[] subjValues)
        {
            var cmd = new Command("list_visit_card", "Archive", "GetSubjectVisitList", true, 0);
            cmd.AddParams(subjNames, subjValues);
            cmd.Send();

            return cmd;

            /*var answer = new ReadCommands("list_visit_card", "Archive", "GetSubjectVisitList");
            List<string> result = new List<string> { };
            result.AddRange(answer.ResultKeys);
            result.AddRange(answer.ResultValues);
            return result;*/
        }

        public string CreateVisit(string[] subjNames, string[] subjValues)
        {
            var cmd = new Command("create_visit_card", "Archive", "CreateVisit", true, 0);
            cmd.AddParams(subjNames, subjValues);
            cmd.Send();

            var answer = new ReadCommands("create_visit_card", "Archive", "CreateVisit");
            return answer.ResultValues[0];
        }

        public bool SelectVisit(string[] subjNames, string[] subjValues)
        {
            var cmd = new Command("select_visit_card", "Archive", "SelectVisit", true, 0);
            cmd.AddParams(subjNames, subjValues);
            cmd.Send();

            var answer = new ReadCommands("select_visit_card", "Archive", "SelectVisit");
            return answer.ResultValues[0] == "ACK";
        }

        public bool ExportData(string[] subjNames, string[] subjValues)
        {
            var cmd = new Command("export_data", "Archive", "ExportData", true, 0);
            cmd.AddParams(subjNames, subjValues);
            cmd.Send();

            var answer = new ReadCommands("export_data", "Archive", "ExportData");
            return answer.ResultValues[0] == "ACK";
        }

        public bool ExportReport(string[] subjNames, string[] subjValues)
        {
            var cmd = new Command("export_report", "Archive", "ExportReport", true, 0);
            cmd.AddParams(subjNames, subjValues);
            cmd.Send();

            var answer = new ReadCommands("export_report", "Archive", "ExportReport");
            return answer.ResultValues[0] == "ACK";
        }



        #endregion
    }
}
