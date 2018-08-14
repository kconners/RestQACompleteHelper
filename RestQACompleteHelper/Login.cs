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
    public class Login
    {
        string Auth { get; set; }
        int Project { get; set; }
        public Login(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }

        public bool CheckAuth()
        {

            Release item = new Release();

            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/releases/{releaseID}");

            var request = new RestRequest(Method.GET);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("releaseID", 1);
            request.AddHeader("Authorization", Auth);
            
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }
            else { return true; }

        }
    }
}
