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
    public class QAC_TestSets
    {
        string Auth { get; set; }
        int Project { get; set; }
        public QAC_TestSets(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }

        public List<TestSets> GetTestSets()
        {
            List<TestSets> item = new List<TestSets>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("limit", "2000");
            request.AddQueryParameter("Filter", "(isActive = true)");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            TestSetReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.TestSetReturn>(L);

            return ReturnBody.results;
        }

        public List<TestSets> GetTestSets(Int32 releaseID)
        {
            List<TestSets> item = new List<TestSets>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("limit", "2000");
            request.AddQueryParameter("Filter", "(isActive = true)");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            TestSetReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.TestSetReturn>(L);

            return ReturnBody.results;
        }


        public List<TestSetItems> GetTestSetItems(Int32 testSetID)
        {
            List<TestSetItems> item = new List<TestSetItems>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets/{testSetID}/items");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("testSetID", testSetID);
            request.AddQueryParameter("limit", "2000");
            
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            TestSetItemsReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.TestSetItemsReturn>(L);

            return ReturnBody.results;
        }
    }
}
