﻿using Biseth.Net.Settee.Http;

namespace Biseth.Net.Settee.Couch.Api.Elements
{
    public class CouchApiDesignDoc : CouchApiDb
    {
        public CouchApiDesignDoc(IRequestClient requestClient)
            : base(requestClient)
        {
        }
    }
}