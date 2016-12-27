using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webionic.LinkList.Models
{
    using Orchard.ContentManagement.Records;

    public class LinkGroupTreeViewPartRecord : ContentPartRecord
    {
        public virtual int SelectedQueryId { get; set; }
    }
}