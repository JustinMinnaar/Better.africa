//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Better.Benefits.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Policy_Receipt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Policy_Receipt()
        {
            this.Row = new Row();
        }
    
        public string SourceCode { get; set; }
        public System.DateTime SourceDate { get; set; }
        public decimal AmountExcl { get; set; }
        public decimal AmountVat { get; set; }
        public System.Guid Id { get; set; }
        public System.Guid PolicyId { get; set; }
    
        public Row Row { get; set; }
    
        public virtual Policy Policy { get; set; }
    }
}
