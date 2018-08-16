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
    public class QAC_Releases
    {
        string Auth { get; set; }
        int Project { get; set; }
        QACDataModel.User.AuthenticationData Auuth;
        public QAC_Releases(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
            Auuth = L;
        }
        public Release GetRelease(Int32 ReleaseID)
        {
            Release item = new Release();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/releases/{releaseID}");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("releaseID", ReleaseID);
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            item = JsonConvert.DeserializeObject<QACDataModel.Release>(L);

            return item;
        }
        public List<Release> GetReleases(Int32 ParentID)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/releases");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("Filter", "(ParentID=" + Convert.ToString(ParentID) + ")");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            QACDataModel.ReleasesReturn releasesReturn = JsonConvert.DeserializeObject<QACDataModel.ReleasesReturn>(L);

            return releasesReturn.results;
        }
        public List<Release> GetReleases(string Type)
        {
            string RT = "";
            if (Type.ToLower() == "release")
            {
                RT = "Release";
            }
            else if (Type.ToLower() == "iteration")
            {
                RT = "Iteration";
            }
            else if (Type.ToLower() == "build")
            {
                RT = "Build";
            }

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/releases");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("Filter", "(releaseType='" + RT + "')");
            request.AddQueryParameter("Limit", "1000");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            QACDataModel.ReleasesReturn releasesReturn = JsonConvert.DeserializeObject<QACDataModel.ReleasesReturn>(L);

            return releasesReturn.results;
        }
        public List<Release> GetReleasesSince(string UpdatedDate)
        {
            string RT = "";
            

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/releases");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("Filter", "(dateUpdated >='" + UpdatedDate + "')");
            request.AddQueryParameter("Limit", "1000");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            QACDataModel.ReleasesReturn releasesReturn = JsonConvert.DeserializeObject<QACDataModel.ReleasesReturn>(L);

            return releasesReturn.results;
        }
        public Release PostReleases(Release release)
        {
            release.project_id = Project;
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/releases");

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(release);
            request.AddUrlSegment("projectID", Project);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            Release item = new Release();
            item = JsonConvert.DeserializeObject<QACDataModel.Release>(L);

            return item;
        }
        public Release PutReleases(Release release)
        {
            release.project_id = Project;
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/releases");

            var request = new RestRequest(Method.PUT);
            request.AddJsonBody(release);
            request.AddUrlSegment("projectID", Project);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            Release item = new Release();
            item = JsonConvert.DeserializeObject<QACDataModel.Release>(L);

            return item;
        }
        public void RefReleaseRuns(string ReleaseNumber, string UserName)
        {

            QADataModel.QAData.Client C = new QADataModel.QAData.Client();
            var client = new RestClient(conFIG.QADataRestEndpoint + "api/Clients");
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("parameter", "qac_projectnum");
            request.AddQueryParameter("value", Convert.ToString(Project));
            var response = client.Execute(request);
            string LG = response.Content;

            var resultsJSON = JsonConvert.DeserializeObject<QADataModel.QAData.GetClientsRepo>(LG);
            C = resultsJSON.clients;



            List<QACDataModel.TestRuns> testRuns = new List<TestRuns>();

            QAC_TestRuns qAC_TestRuns = new QAC_TestRuns(Auuth);
            testRuns = qAC_TestRuns.GetTestRuns(Convert.ToInt32(ReleaseNumber));
            
                if (testRuns != null && testRuns.Count() > 1)
                    foreach (var Line in testRuns)
                    {
                        try
                        {
                            QADataModel.qac.Run rn = new QADataModel.qac.Run();
                            rn.Client_IDNumber = C.Client_IDNUM;
                            rn.Client_Short = C.clnt_short;
                            rn.configuration_name = Line.configuration_name;
                            rn.date_finished = Convert.ToString(Line.date_finished);
                            rn.date_started = Convert.ToString(Line.date_started);
                            rn.execution_type = Line.execution_type;
                            rn.ID = Convert.ToString(Line.id);
                            rn.is_sequential = Convert.ToString(Line.is_sequential);
                            rn.nbr_awaiting_run = Convert.ToString(Line.nbr_awaiting_run);
                            rn.nbr_blocked = Convert.ToString(Line.nbr_blocked);
                            rn.nbr_failed = Convert.ToString(Line.nbr_failed);
                            rn.nbr_passed = Convert.ToString(Line.nbr_passed);
                            rn.nbr_tests = Convert.ToString(Line.nbr_tests);
                            rn.proj_id = Convert.ToString(Line.project_id);
                            rn.qac_status = Line.status_code;
                            rn.release_id = Convert.ToString(Line.release_id);
                            rn.run_by_user = Line.run_by_user_name;
                            rn.Run_IDNumber = Guid.NewGuid();
                            rn.run_time = Convert.ToString(Line.run_time);
                            rn.run_time_formated = Line.run_time_formated;
                            rn.Status = 1;
                            rn.testset_id = Convert.ToString(Line.test_set_id);
                            rn.testtype = "";
                            rn.Title = Line.test_set_name;

                            client = new RestClient(conFIG.QADataRestEndpoint + "api/QACRun");
                            request = new RestRequest(Method.POST);

                            request.AddJsonBody(rn);
                            request.AddQueryParameter("loggedInas", UserName);
                            response = client.Execute(request);
                        }
                        catch
                        {
                        }
                    }
          
        }
        public string GetLastUpdatedDate(Guid Client_IDNUM, string EntityType)
        {
            
            var client = new RestClient(conFIG.QADataRestEndpoint + "api/QAComplete/LastUpdateDate");
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("Client_IDNUM", Convert.ToString(Client_IDNUM));
            request.AddQueryParameter("Type", EntityType);
            var response = client.Execute(request);
            string LG = response.Content;
            return LG;
        }
        public void RefReshReleases(string did, string pid, string uid, string pc)
        {
            QADataModel.QAData.Client C = new QADataModel.QAData.Client();
            var client = new RestClient(conFIG.QADataRestEndpoint + "api/Clients");
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("parameter", "qac_projectnum");
            request.AddQueryParameter("value", Convert.ToString(Project));
            var response = client.Execute(request);
            string LG = response.Content;

            var resultsJSON = JsonConvert.DeserializeObject<QADataModel.QAData.GetClientsRepo>(LG);
            C = resultsJSON.clients;


            string Release_ID, Release_Title, Release_Description, QAC_UPDATED, ReleaseType, ParentId, ParentName, SSTatus;
           
                List<Release> Rels = GetReleasesSince(GetLastUpdatedDate(C.Client_IDNUM, "release"));

                //   Rels = R.Releases_LoadByCriteria(L, Criterias, "", PageSize, PageNumber);
                foreach (var Line in Rels)
                {

                    Release_ID = ""; Release_Title = ""; Release_Description = ""; QAC_UPDATED = ""; ReleaseType = ""; ParentId = ""; ParentName = "";

                    Release_ID = Convert.ToString(Line.id);
                    if (Release_ID == null) Release_ID = "";

                    SSTatus = Convert.ToString(Line.status_code);
                    if (SSTatus == null) SSTatus = "";

                    Release_Title = Convert.ToString(Line.title);
                    if (Release_Title == null) Release_Title = "";

                    Release_Description = Convert.ToString(Line.description);
                    if (Release_Description == null) Release_Description = "";

                    QAC_UPDATED = Convert.ToString(Line.date_updated);
                    if (QAC_UPDATED == null) QAC_UPDATED = "";
                    ReleaseType = Convert.ToString(Line.release_type);
                    if (ReleaseType == null) ReleaseType = "";
                    ParentId = Convert.ToString(Line.parent_id);
                    if (ParentId == null) ParentId = "";
                    ParentName = Convert.ToString(Line.parent_name);
                    if (ParentName == null) ParentName = "";

                    try
                    {

                        QADataModel.qac.Release REL = new QADataModel.qac.Release();
                        REL.Client_IDNumber = C.Client_IDNUM;
                        REL.Description = Release_Description;
                        REL.ID = Release_ID;
                        REL.Parent_ID = Convert.ToInt32(ParentId);

                        REL.QAC_Updated_Date = QAC_UPDATED;
                        REL.Release_IDNumber = new Guid();


                        if (SSTatus == "Awaiting Start") REL.Status = 0;
                        else if (SSTatus == "In Progress") REL.Status = 1;
                        else if (SSTatus == "Closed (Cancelled)") REL.Status = -1;
                        else if (SSTatus == "Closed (Completed)") REL.Status = -2;
                        REL.Title = Release_Title;
                        REL.TestingApp_IDNumber = new Guid();
                        REL.Type = ReleaseType;

                        if (Convert.ToInt32(ParentId) >= 1)
                        {
                            QADataModel.qac.Release Parent = new QADataModel.qac.Release();
                            try
                            {
                                var Rclient = new RestClient(conFIG.QADataRestEndpoint + "api/Releases");
                                var Rrequest = new RestRequest(Method.GET);
                                Rrequest.AddQueryParameter("ID", Convert.ToString(ParentId));

                                var Rresponse = Rclient.Execute(Rrequest);
                                string RLG = Rresponse.Content;

                                var RresultsJSON = JsonConvert.DeserializeObject<QADataModel.qac.ReleaseRoot>(RLG);
                                Parent = RresultsJSON.release;
                                REL.Parent_IDNumber = Parent.Release_IDNumber;

                            }
                            catch
                            {

                                REL.Parent_IDNumber = new Guid();
                            }
                        }

                        client = new RestClient(conFIG.QADataRestEndpoint + "api/Releases");
                        request = new RestRequest(Method.POST);

                        request.AddJsonBody(REL);
                        request.AddQueryParameter("loggedInas", "Automation");
                        response = client.Execute(request);

                    }
                    catch
                    {

                    }
                }
                
            }
    }
}
