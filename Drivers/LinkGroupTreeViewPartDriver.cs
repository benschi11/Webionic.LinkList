using System;
using System.Collections.Generic;
using System.Linq;

namespace Webionic.LinkList.Drivers
{
    using Orchard.ContentManagement;
    using Orchard.ContentManagement.Drivers;
    using Orchard.Core.Common.Models;
    using Orchard.Core.Containers.Models;
    using Orchard.Core.Title.Models;
    using Orchard.Forms.Services;
    using Orchard.Projections.Descriptors.Layout;
    using Orchard.Projections.Models;
    using Orchard.Projections.Services;
    using Orchard.Projections.ViewModels;
    using Webionic.LinkList.Models;
    using Webionic.LinkList.ViewModels;

    public class LinkGroupTreeViewPartDriver : ContentPartDriver<LinkGroupTreeViewPart>
    {
        private readonly IContentManager _contentManager;

        private readonly IProjectionManager _projectionManager;

        public LinkGroupTreeViewPartDriver(IContentManager contentManager, IProjectionManager projectionManager)
        {
            _contentManager = contentManager;
            _projectionManager = projectionManager;
        }

        protected override DriverResult Display(LinkGroupTreeViewPart part, string displayType, dynamic shapeHelper)
        {
            var parentContentItems = new List<ParentContentItem>();


            var linkGroups = _projectionManager.GetContentItems(part.SelectedQueryId);


            foreach (var linkGroup in linkGroups)
            {
                var parentContentItem = new ParentContentItem();

                parentContentItem.ParentItem = linkGroup;


                parentContentItem.ChildItems = _contentManager.Query<CommonPart, CommonPartRecord>()
                    .Where(c => c.Container.Id == linkGroup.Id)
                    .List()
                    .Select(item => item.ContentItem)
                    .OrderByDescending(c => c.As<ContainablePart>().Position);

                parentContentItems.Add(parentContentItem);
            }

            part.TreeData = parentContentItems;

            return ContentShape("Parts_LinkGroupTreeView",
                                () => shapeHelper.Parts_LinkGroupTreeView(
                                  TreePart: part,
                                  shapeHelper: shapeHelper
                                        ));
        }

        protected override DriverResult Editor(LinkGroupTreeViewPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_LinkGroupTreeViewPart_Edit",
                             () =>
                             {

                                 var model = new LinkGroupTreeViewPartEditViewModel
                                 {
                                     QueryLayoutRecordId = -1,
                                 };

                                 // concatenated Query and Layout ids for the view
                                 if (part.Record.SelectedQueryId > 0)
                                 {
                                     model.QueryLayoutRecordId = part.Record.SelectedQueryId;
                                 }

                                 // populating the list of queries and layouts
                                 var layouts = _projectionManager.DescribeLayouts().SelectMany(x => x.Descriptors).ToList();
                                 model.QueryRecordEntries = _contentManager.Query<QueryPart>().Join<TitlePartRecord>().OrderBy(x => x.Title).List()
                                     .Select(x => new QueryRecordEntry
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         LayoutRecordEntries = x.Layouts.Select(l => new LayoutRecordEntry
                                         {
                                             Id = l.Id,
                                             Description = GetLayoutDescription(layouts, l)
                                         })
                                     });


                                 return shapeHelper.EditorTemplate(TemplateName: "Parts/LinkGroupTreeViewPart", Model: model);
                             });
        }

        protected override DriverResult Editor(LinkGroupTreeViewPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = new LinkGroupTreeViewPartEditViewModel();
            if (updater.TryUpdateModel(model, string.Empty, null, null))
            {
                part.SelectedQueryId = model.QueryLayoutRecordId;
            }
            return Editor(part, shapeHelper);
        }

        private static string GetLayoutDescription(IEnumerable<LayoutDescriptor> layouts, LayoutRecord l)
        {
            var descriptor = layouts.FirstOrDefault(x => l.Category == x.Category && l.Type == x.Type);
            return String.IsNullOrWhiteSpace(l.Description) ? descriptor.Display(new LayoutContext { State = FormParametersHelper.ToDynamic(l.State) }).Text : l.Description;
        }
    }
}