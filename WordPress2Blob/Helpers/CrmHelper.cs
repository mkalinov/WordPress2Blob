using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;

namespace WordPress2Blob.Helpers
{
    public class CrmHelper
    {

        public Guid? getEntitybyAttr(CrmServiceClient service, string logicalName, string attName, object attrValue) {

            QueryByAttribute querry = new QueryByAttribute(logicalName);
            querry.AddAttributeValue(attName, attrValue);
            querry.ColumnSet = new ColumnSet(new string[] { logicalName + "id", attName });

            EntityCollection ens = service.RetrieveMultiple(querry);

            if (ReferenceEquals(ens, null)
                || ReferenceEquals(ens.Entities, null)
                || ens.Entities.Count == 0) return null;

            else return ens.Entities[0].Id;
        }
    }
}
