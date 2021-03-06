// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace HBD.EntityFramework.TestDal
{
    using HBD.EntityFramework.Core;

    // ParentChildren
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class ParentChildren: IDbEntity
    {

         public object[] GetKeys() => new object[] { ParentId, ChildrenId };

        public int ParentId { get; set; } // ParentId (Primary key)
        public int ChildrenId { get; set; } // ChildrenId (Primary key)
        public string CreatedBy { get; set; } // CreatedBy (length: 100)
        public System.DateTime CreatedOn { get; set; } // CreatedOn
        public string UpdatedBy { get; set; } // UpdatedBy (length: 100)
        public System.DateTime? UpdatedOn { get; set; } // UpdatedOn
        public byte[] RowVersion { get; private set; } // RowVersion

        // Foreign keys

        /// <summary>
        /// Parent Child pointed by [ParentChildren].([ChildrenId]) (FK_ParentChildrent_Childrent)
        /// </summary>
        public virtual Child Child { get; set; } // FK_ParentChildrent_Childrent

        /// <summary>
        /// Parent Parent pointed by [ParentChildren].([ParentId]) (FK_ParentChildrent_Parent)
        /// </summary>
        public virtual Parent Parent { get; set; } // FK_ParentChildrent_Parent
    }

}
// </auto-generated>
