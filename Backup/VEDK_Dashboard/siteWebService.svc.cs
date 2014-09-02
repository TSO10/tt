using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Data.Layer;
using System.Data.SqlClient;
using System.Data;

namespace VEDK_Dashboard
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [System.Web.Script.Services.ScriptService]
    public class siteWebService
    {

        [OperationContract]
        [WebInvoke(Method = "POST")]
        public Agreement GetNewAgreement(long time)
        {
            DateTime datetime = (new DateTime(1970, 1, 1).AddMilliseconds(time)).ToLocalTime();
            return AgreementsRepository.GetLastByTime(datetime);
        }

        [WebInvoke(Method = "GET",
           BodyStyle = WebMessageBodyStyle.WrappedRequest,
           ResponseFormat = WebMessageFormat.Json)]
        public CustomMillLog[] GetData(string gsrn)
        {
            return CustomMillLog.GetList(gsrn, 1000);
        }

        [OperationContract]
        [WebInvoke(Method = "POST")]
        public List<CustomEnerginetData> GetEnerginetByDate(int year, int month, int day)
        {
            return EnerginetDKDataRepository.GetListByDate(year, month, day);
        }


    }
}
