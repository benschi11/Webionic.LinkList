using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webionic.LinkList.Models
{
    using Orchard.ContentManagement;

    public class LinkGroupTreeViewPart : ContentPart<LinkGroupTreeViewPartRecord>
    {
        public int SelectedQueryId
        {
            get
            {
                return Record.SelectedQueryId;
            }
            set
            {
                Record.SelectedQueryId = value;
            }
        }
        public IEnumerable<ParentContentItem> TreeData { get; set; }
    }
}