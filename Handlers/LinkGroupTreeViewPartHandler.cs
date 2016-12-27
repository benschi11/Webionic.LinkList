using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webionic.LinkList.Handlers
{
    using Orchard.ContentManagement.Handlers;
    using Orchard.Data;

    using Webionic.LinkList.Models;

    public class LinkGroupTreeViewPartHandler : ContentHandler
    {
        public LinkGroupTreeViewPartHandler(IRepository<LinkGroupTreeViewPartRecord> repository )
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}