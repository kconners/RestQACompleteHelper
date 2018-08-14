using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using QACDataModel;

namespace RestQACompleteHelper
{
    public class QAC_Tests
    {
        string Auth { get; set; }
        int Project { get; set; }
        public QAC_Tests(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }
        public Tests GetTests(Int32 TestID)
        {
            Tests item = new Tests();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/tests/{testID}");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("testID", TestID);
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            item = JsonConvert.DeserializeObject<QACDataModel.Tests>(L);

            return item;
        }
        public Tests PostTests(Tests test)
        {
            test.project_id = Project;
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/tests");

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(test);
            request.AddUrlSegment("projectID", Project);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            Tests item = new Tests();
            item = JsonConvert.DeserializeObject<QACDataModel.Tests>(L);

            return item;
        }
        public Tests PutTest(Tests test)
        {
            int TestID = test.id;
            test.project_id = Project;
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/tests/{testID}");

            var request = new RestRequest(Method.PUT);
            request.AddJsonBody(test);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("testID", TestID);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            Tests item = new Tests();
            item = JsonConvert.DeserializeObject<QACDataModel.Tests>(L);

            return item;
        }


        public void SyncTestFromQACtoBPTestingApp( string Client_ShortName, string LastUpdateDate)
        {
            string Failed = "";
            List<QACDataModel.Tests> tests = new List<QACDataModel.Tests>();


            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/tests");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("Filter", "(active=true) AND (DateUpdated >= '" + LastUpdateDate + "')");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            QACDataModel.TestsReturn testsReturn = JsonConvert.DeserializeObject<QACDataModel.TestsReturn>(L);

            string TC_IDNum, Title, Status, Desc, TestCase_Folder, TestCase_Folder_ID, TestCase_UpDated_Date;

            foreach (var Line in testsReturn.results)
            {
                TC_IDNum = " "; Title = " "; Status = " "; Desc = " "; TestCase_Folder = " "; TestCase_Folder_ID = " "; TestCase_UpDated_Date = " ";
                int ID = Line.id;
                TC_IDNum = Convert.ToString(Line.id);
                Title = Convert.ToString(Line.title);
                Status = Convert.ToString(Line.status);
                Desc = Convert.ToString(Line.description);
                TestCase_Folder = Convert.ToString(Line.folder_name);
                if (TestCase_Folder == null) TestCase_Folder = "";
                if (Desc == null) Desc = "";

                TestCase_Folder_ID = Convert.ToString(Line.folder_id);
                if (TestCase_Folder_ID == string.Empty || TestCase_Folder_ID == "") TestCase_Folder_ID = " ";
                TestCase_UpDated_Date = Convert.ToString(Line.date_updated);
                try
                {
                    QADataModel.qac.TestCase TC = new QADataModel.qac.TestCase();
                    TC.Description = Desc;
                    TC.Folder = TestCase_Folder;
                    try { TC.Folder_ID = Line.folder_id; } catch { }
                    TC.ID = Convert.ToString(ID);
                    TC.Client_Short = Client_ShortName;
                    TC.QAC_Updated_Date = TestCase_UpDated_Date;
                    TC.Status = 1;
                    TC.TestCase_IDNumber = Guid.NewGuid();
                    TC.Title = Title;
                    var QADATAclient = new RestClient(conFIG.QADataRestEndpoint + "api/QAComplete/TestCase");
                    var QADATArequest = new RestRequest(Method.POST);
                    QADATArequest.AddQueryParameter("loggedInas", "kconners");
                    QADATArequest.AddJsonBody(TC);
                    var QADATAresponse = QADATAclient.Execute(QADATArequest);
                }
                catch { Failed = Failed + "," + TC_IDNum; }
            }
            
        }
    }
}
