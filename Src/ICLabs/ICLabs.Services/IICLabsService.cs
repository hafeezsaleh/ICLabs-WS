using ICLabs.Model;
using ICLabs.ModelV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICLabs.Services
{
    public interface IICLabsService
    {
        ClsApplication IsAuthenticated(string clientId);

        string PostOrder(ClsOrder objOrder);

        IList<ClsOrderStatus> GetOrderStatus(ClsGetOrderStatus OrderStatus);

        IList<ClsResultsStatus> GetResultStatus(ClsGetResultStatus ResultStatus);

        IList<ClsResult> GetResult(ClsGetResult GetResult);

        ClsResultFile GetResultFile(ClsGetResultFile GetResultFile);

        void AddLog(String AppId, String recordType, String eventData);

        string PostResult(ClsPostResult result);
    }
}
