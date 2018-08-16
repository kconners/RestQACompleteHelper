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
    public class QAC_Folders
    {
        string Auth { get; set; }
        int Project { get; set; }
        public QAC_Folders(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }

        public List<Folders> GetFolders(string EntityType, bool isActive = true)
        {
            List<Folders> item = new List<Folders>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/{EntityCode}/folders");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("EntityCode", EntityType);
            request.AddQueryParameter("Filter", "(isActive= "+ isActive + ")");
            request.AddQueryParameter("limit", "2000");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            FoldersReturm ReturnBody = JsonConvert.DeserializeObject<QACDataModel.FoldersReturm>(L);

            return ReturnBody.results;
        }
        public List<Folders> GetFolder(string fullFOlderName, string EntityType)
        {
            List<Folders> item = new List<Folders>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/{EntityCode}/folders");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("EntityCode", EntityType);
            request.AddQueryParameter("Filter", "(fullFolderName  = '" + fullFOlderName + "')");
            request.AddQueryParameter("limit", "2000");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            FoldersReturm ReturnBody = JsonConvert.DeserializeObject<QACDataModel.FoldersReturm>(L);

            return ReturnBody.results;
        }
        public List<Folders> GetFolder(string fullFOlderName, string EntityType, bool isActive = true)
        {
            List<Folders> item = new List<Folders>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/{EntityCode}/folders");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("EntityCode", EntityType);
            request.AddQueryParameter("Filter", "(fullFolderName  = '"+fullFOlderName+ "' and isActive= " + isActive + ")");
            request.AddQueryParameter("limit", "2000");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            FoldersReturm ReturnBody = JsonConvert.DeserializeObject<QACDataModel.FoldersReturm>(L);

            return ReturnBody.results;
        }

        public Folders PostFolder(QACDataModel.Folders NewFolder)
        {

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/{EntityCode}/folders");

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(NewFolder);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("EntityCode", NewFolder.entity_code);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            Folders item = new Folders();
            item = JsonConvert.DeserializeObject<QACDataModel.Folders>(L);

            return item;
        }
    }
}
