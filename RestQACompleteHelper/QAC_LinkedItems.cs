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
   public class QAC_LinkedItems
    {
        string Auth { get; set; }
        int Project { get; set; }
        public QAC_LinkedItems(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }

        public List<LinkedItems> GetReleasesTestSets(int releaseID)
        {
            List<LinkedItems> item = new List<LinkedItems>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/Releases/{releaseID}/linkeditems");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("releaseID", releaseID);
            request.AddQueryParameter("limit", "1000");
            request.AddQueryParameter("Filter", "(linkedEntityCode = 'TestSets')");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            LinkedItemsReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.LinkedItemsReturn>(L);

            return ReturnBody.results;
        }
        public LinkedItems PostNewLink(QACDataModel.LinkedItems linked)
        {
            
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/Releases/{releaseID}/linkeditems");

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(linked);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("releaseID", linked.entity_id);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            LinkedItems item = new LinkedItems();
            item = JsonConvert.DeserializeObject<QACDataModel.LinkedItems>(L);

            return item;
        }
    }
}
