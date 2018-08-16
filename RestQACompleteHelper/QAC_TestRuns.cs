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
   public class QAC_TestRuns
    {
        string Auth { get; set; }
        int Project { get; set; }
        public QAC_TestRuns(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }

        public List<TestRuns> GetTestRuns(int releaseID)
        {
            List<TestRuns> item = new List<TestRuns>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("limit", "1000");
            request.AddQueryParameter("Filter", "(releaseId = " + Convert.ToString(releaseID) + ")");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            TestRunReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.TestRunReturn>(L);

            return ReturnBody.results;
        }
        public TestRuns GetTestRun(int runID)
        {
            TestRuns item = new TestRuns();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns/{runID}");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("runID", runID);
            request.AddQueryParameter("limit", "10");
            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            item = JsonConvert.DeserializeObject<QACDataModel.TestRuns>(L);

            return item;
        }
        public List<TestRuns> GetTestRuns(int releaseID, int testsetID, bool OnlyGiveAvailableRuns = false)
        {
            List<TestRuns> item = new List<TestRuns>();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddQueryParameter("limit", "1000");

            if (OnlyGiveAvailableRuns == false)
            {
                request.AddQueryParameter("Filter", "(releaseId = " + Convert.ToString(releaseID) + " and testSetId = " + Convert.ToString(testsetID) + ")");
            }

            else if (OnlyGiveAvailableRuns == true)
            {
                request.AddQueryParameter("Filter", "(releaseId = " + Convert.ToString(releaseID) + " and testSetId = " + Convert.ToString(testsetID) + ")");
            }

            request.AddHeader("Authorization", Auth);

            var response = client.Execute(request);
            string L = response.Content;


            TestRunReturn ReturnBody = JsonConvert.DeserializeObject<QACDataModel.TestRunReturn>(L);

            return ReturnBody.results;
        }
        public TestRuns PostNewRun(QACDataModel.TestRuns run)
        {

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns");

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(run);
            request.AddUrlSegment("projectID", Project);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            TestRuns item = new TestRuns();
            item = JsonConvert.DeserializeObject<QACDataModel.TestRuns>(L);

            return item;
        }

        //public TestRuns PatchRun(QACDataModel.TestRuns run)
        //{

        //    var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns/{testRunID}");

        //    var request = new RestRequest(Method.PATCH);
        //    request.AddJsonBody(run);
        //    request.AddUrlSegment("projectID", Project);
        //    request.AddUrlSegment("testRunID", run.id);
        //    request.AddHeader("Authorization", Auth);
        //    request.AddHeader("Content-Type", "application/json");

        //    var response = client.Execute(request);
        //    string L = response.Content;

        //    TestRuns item = new TestRuns();
        //    item = JsonConvert.DeserializeObject<QACDataModel.TestRuns>(L);

        //    return item;
        //}


        //public TestRuns SetRunTime(TestRuns run)
        //{
        //    List<TestRunItems> testRunItems = GetRunItems(run.id);
        //    int runtime = 0;
        //    foreach (var i in testRunItems)
        //    {
        //        runtime = runtime + i.run_time;
        //    }

        //    run.run_time = runtime;
        //    return PatchRun(run);
        //}

        public List<TestRunItems> GetRunItems(Int32 RunID)
        {

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns/{runID}/items");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("runID", RunID);
            request.AddUrlSegment("projectID", Project);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            TestRunItemsReturn item = new TestRunItemsReturn();
            item = JsonConvert.DeserializeObject<QACDataModel.TestRunItemsReturn>(L);

            return item.results;
        }

        public TestRunItems GetRunItem(Int32 RunID,Int32 RunItemID)
        {

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns/{runID}/items/{runItemID}");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("runID", RunID);
            request.AddUrlSegment("runItemID", RunItemID);
            request.AddUrlSegment("projectID", Project);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            TestRunItems item = new TestRunItems();
            item = JsonConvert.DeserializeObject<QACDataModel.TestRunItems>(L);

            return item;
        }
        public List<TestRunItems> GetRunItemByTestCaseID(Int32 RunID, Int32 testsID)
        {

            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns/{runID}/items");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("runID", RunID);
            request.AddQueryParameter("filter", "testId=" + Convert.ToString(testsID) + "");
            request.AddUrlSegment("projectID", Project);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            TestRunItemsReturn item = new TestRunItemsReturn();
            item = JsonConvert.DeserializeObject<QACDataModel.TestRunItemsReturn>(L);

            return item.results;
        }
        public class ItemUpdate
        {
            
                public int id { get; set; }
                public int test_set_id { get; set; }
                public int project_id { get; set; }
                public int test_run_id { get; set; }
                public int test_id { get; set; }
                public string test_name { get; set; }
                public string status_code { get; set; }
                public int run_by_user_id { get; set; }
                public int run_time { get; set; }
                public string last_run_by_name { get; set; }
                public int release_id { get; set; }
                public string release_name { get; set; }
                public string test_set_name { get; set; }
                public bool automated { get; set; }
                public string configuration_name { get; set; }
                public string test_host { get; set; }
                public List<TestRunResult> steps { get; set; }
                

        }
        public TestRunItems PostRunItem(TestRunItems testRunItems)
        {
            ItemUpdate itemUpdate = new ItemUpdate();
            itemUpdate.automated = testRunItems.automated;
            itemUpdate.configuration_name = testRunItems.configuration_name;
            itemUpdate.id = testRunItems.id;
            itemUpdate.last_run_by_name = testRunItems.last_run_by_name;
            itemUpdate.project_id = testRunItems.project_id;
            itemUpdate.release_id = testRunItems.release_id;
            itemUpdate.release_name = testRunItems.release_name;
            itemUpdate.run_by_user_id = testRunItems.run_by_user_id;

            itemUpdate.status_code = testRunItems.status_code;
            itemUpdate.test_host = testRunItems.test_host;
            itemUpdate.test_id = testRunItems.test_id;
            itemUpdate.test_name = testRunItems.test_name;
            itemUpdate.test_run_id = testRunItems.test_run_id;
            itemUpdate.test_set_id = testRunItems.test_set_id;
            itemUpdate.test_set_name = testRunItems.test_set_name;
            itemUpdate.steps = testRunItems.test_run_results;

            foreach (var i in itemUpdate.steps)
            {
                i.step_name = i.step;
                i.actual_result = testRunItems.actualResult;
                i.status_code = testRunItems.status_code;
            }

            itemUpdate.run_time = testRunItems.run_time;


            var client = new RestClient(conFIG.QACompleteEndPoint + "v2/projects/{projectID}/testruns/{runID}/items/{runItemID}");

            var request = new RestRequest(Method.PATCH);
            request.AddUrlSegment("runID", testRunItems.test_run_id);
            request.AddUrlSegment("runItemID", testRunItems.id);
            request.AddUrlSegment("projectID", Project);
            request.AddJsonBody(itemUpdate);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;

            TestRunItems item = new TestRunItems();
            item = JsonConvert.DeserializeObject<QACDataModel.TestRunItems>(L);

            return item;
        }
    }
}
