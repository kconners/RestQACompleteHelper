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
    public class QAC_Requirements
    {
        string Auth { get; set; }
        int Project { get; set; }
        public QAC_Requirements(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }
        public Requirements GetRequirements(Int32 ID)
        {
            Requirements item = new Requirements();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/requirements/{requirementID}");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("requirementID", ID);
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            item = JsonConvert.DeserializeObject<QACDataModel.Requirements>(L);

            return item;
        }
        public List<Requirements> GetRequirements()
        {
            List<Requirements> item = new List<Requirements>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/requirements");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("limit", "1000");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            RequirementsReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.RequirementsReturn>(L);

            return ReturnBody.results;
        }
    }
}
