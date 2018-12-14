using Knights.Core.Common;
using Knights.Core.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace BetterAfrica.Shared
{
    public abstract class CRow : IImportExport, IToNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Marked true when a row is deleted, allowing us to preserve the original data in case we need to recover the row.</summary>
        public bool IsDeleted { get; set; }

        public CNode ToNode(string nickname = null)
        {
            var node = new CNode(nickname ?? this.ToNickname());
            Export(node);
            return node;
        }

        public virtual void Export(CNode node, CCreator creator = null)
        {
            this.ExportProperties(node);
            this.ExportChildren(node, creator);
        }

        protected virtual void ExportChildren(CNode node, CCreator creator = null)
        {
        }

        public virtual void Import(CNode node, CCreator creator = null)
        {
            this.ImportProperties(node);
            foreach (var child in node.Children)
            {
                this.ImportChild(node, creator);
            }
        }

        protected virtual void ImportChild(CNode node, CCreator creator = null)

        {
        }

        private List<PropertyInfo> ErrorProperties { get; } = new List<PropertyInfo>();

        //private List<PropertyInfo> ComplexProperties { get; } = new List<PropertyInfo>();

        public CRow()
        {
            var type = this.GetType();
            var properties = type.GetPublicProperties();
            foreach (var p in properties)
            {
                if (p.CanRead && p.Name.EndsWith("Error") && p.PropertyType == typeof(string))
                {
                    ErrorProperties.Add(p);
                }
                //else if (p.PropertyType is object)
                //{
                //    // Classes need to be examined to see if they are valid
                //    ComplexProperties.Add(p);
                //}
            }
        }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
            private set { }
        }

        [NotMapped]
        public EntityErrors Errors
        {
            get
            {
                var errors = new EntityErrors();
                BeforeSave(errors);
                return errors;
            }
        }

        public void BeforeSave(EntityErrors errors)
        {
            BeforeSaveOverride(errors);
        }

        /// <summary>
        ///     By default, this checks all properties ending with the word Error and adds any that
        ///     are not null to the <paramref name="errors"/> list. Create a public property for any
        ///     property that you want to validate.
        /// </summary>
        /// <param name="errors"></param>
        protected virtual void BeforeSaveOverride(EntityErrors errors)
        {
            foreach (var p in ErrorProperties)
            {
                var message = (string)p.GetValue(this);
                if (message != null) errors.Add(p.Name, message);
            }
            //foreach (var cp in ComplexProperties)
            //{
            //}
        }
    }
}