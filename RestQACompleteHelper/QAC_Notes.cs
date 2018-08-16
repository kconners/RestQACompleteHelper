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
    public class QAC_Notes
    {
        string Auth { get; set; }
        int Project { get; set; }
        public QAC_Notes(QACDataModel.User.AuthenticationData L)
        {
            Auth = L.GetAuth();
            Project = L.ProjId();
        }
        public Notes PostNote(Notes NewNotes)
        {
            var client = new RestClient(conFIG.QACompleteEndPoint + "v1/projects/{projectID}/{EntityCode}/{EntityID}/notes");

            var request = new RestRequest(Method.POST);
            request.AddUrlSegment("projectID", Project);
            request.AddUrlSegment("EntityCode", NewNotes.entity_code);
            request.AddUrlSegment("EntityID", NewNotes.entity_id);
            request.AddJsonBody(NewNotes);
            request.AddHeader("Authorization", Auth);
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            string L = response.Content;


            return JsonConvert.DeserializeObject<QACDataModel.Notes>(L);
        }
    }
}
