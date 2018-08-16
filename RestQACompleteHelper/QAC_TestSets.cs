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
        public TestSets GetTestSet(Int32 testSetID)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets/{testSetID}");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("testSetID", testSetID);
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            return JsonConvert.DeserializeObject<QACDataModel.TestSets>(L);
        }
        public List<TestSets> GetTestSet(string _filter)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("filter", _filter);
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;

            var dd = JsonConvert.DeserializeObject<QACDataModel.TestSetReturn>(L);
            return dd.results;
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
        public List<TestSets> GetTestSets(string LastUpdateDate)
        {
            List<TestSets> item = new List<TestSets>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("limit", "2000");
            request.AddQueryParameter("Filter", "(active=true) AND (DateUpdated >= '" + LastUpdateDate + "')");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            TestSetReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.TestSetReturn>(L);

            return ReturnBody.results;
        }


        public TestSets PostTestSet(TestSets testSets)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets");

            var request = new RestRequest(Method.POST);
            request.AddUrlSegment("projectID", Project);
            request.AddJsonBody(testSets);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;


            return JsonConvert.DeserializeObject<QACDataModel.TestSets>(L);
        }

        public TestSetItems PostTestSetItems(TestSetItems testSetItems)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets/{testSetID}/items");

            var request = new RestRequest(Method.POST);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("testSetID", testSetItems.test_set_id);
            request.AddJsonBody(testSetItems);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;


            return JsonConvert.DeserializeObject<QACDataModel.TestSetItems>(L);
        }
        public void DeleteTestSetItems(TestSetItems testSetItems)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets/{testSetID}/items/{seq}");

            var request = new RestRequest(Method.DELETE);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("testSetID", testSetItems.test_set_id);
            request.AddUrlSegment("seq", testSetItems.seq);

            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
        }
        public TestSetItems GetTestSetItems(Int32 testSetID, Int32 testCaseID)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/TestSets/{testSetID}/items");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("testSetID", testSetID);
            request.AddQueryParameter("limit", "2000");
            request.AddQueryParameter("Filter", "(testID = " + Convert.ToString(testCaseID) + ")");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            TestSetItemsReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.TestSetItemsReturn>(L);

            return ReturnBody.results[0];
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
