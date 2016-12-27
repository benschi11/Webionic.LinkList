using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Webionic.LinkList
{
    using Webionic.LinkList.Models;

    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {

            ContentDefinitionManager.AlterPartDefinition("LinkGroupPart", builder =>
                builder.WithField("OrderNumber", cfg =>
                    cfg.OfType("NumericField")
                    .WithDisplayName("Ordnungszahl")
                    .WithSetting("NumericFieldSettings.Required", "True")
                    .WithSetting("NumericFieldSettings.Hint", "Je hoeher diese Zahl ist, desto frueher wird diese Link Gruppe angezeigt (Reihenfolge).")));


            ContentDefinitionManager.AlterTypeDefinition("LinkGroup", builder =>
                builder.WithPart("CommonPart")
                .WithPart("ContainerPart", part => 
                    part.WithSetting("ContainerTypePartSettings.RestrictedItemContentTypes","Link")
                    .WithSetting("ContainerTypePartSettings.RestrictItemContentTypes", "True"))
                .WithPart("TitlePart")
                .WithPart("IdentityPart")
                .WithPart("LinkGroupPart")
                .Creatable()
                .Draftable());

            ContentDefinitionManager.AlterPartDefinition("LinkPart", builder =>
                builder.Attachable()
                .WithField("Description", field =>
                    field.OfType("TextField").WithDisplayName("Beschreibung"))
                .WithField("Url", field =>
                    field.OfType("LinkField").WithDisplayName("Url")
                        .WithSetting("LinkFieldSettings.Required", "True")
                        .WithSetting("LinkFieldSettings.TargetMode", "NewWindow")
                 ));

            ContentDefinitionManager.AlterTypeDefinition("Link", builder =>
                builder.WithPart("CommonPart")
                .WithPart("IdentityPart")
                .WithPart("LinkPart")
                .WithPart("ContainablePart")
                .Creatable()
                .Draftable()
                );

            ContentDefinitionManager.AlterPartDefinition(
              typeof(LinkGroupTreeViewPart).Name, cfg => cfg
                  .Attachable()
                  .WithDescription("Displays the LinkGroups and Links in a TreeView"));

            return 1;
        }

        public int UpdateFrom1() 
        {
            ContentDefinitionManager.AlterTypeDefinition("LinkPage", builder=>
                builder.WithPart("TitlePart")
                .WithPart("IdentityPart")
                .WithPart("LinkGroupTreeViewPart")
                .WithPart("AutoRoutePart")
                .WithPart("CommonPart")
                .Creatable()
                );

            return 2;
        }

        public int UpdateFrom2()
        {
            SchemaBuilder.CreateTable(
                "LinkGroupTreeViewPartRecord",
                table => table.ContentPartRecord().Column<int>("SelectedQueryId"));
            return 3;
        }

        public int UpdateFrom3()
        {
            ContentDefinitionManager.AlterTypeDefinition(
                "LinkGroup",
                builder => builder.WithPart("TitlePart", p => p.WithSetting("TitlePart.Required", "True")));
            return 4;
        }

        public int UpdateFrom4()
        {
            ContentDefinitionManager.AlterPartDefinition(
                "LinkPart",
                builder =>
                builder.WithField(
                    "Description",
                    field =>
                    field.OfType("TextField")
                        .WithDisplayName("Beschreibung")
                        .WithSetting("TextFieldSettings.Flavor", "Textarea")));
            return 5;
        }

    }
}