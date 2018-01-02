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

    // Parents
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public class Parent: ITestEntity
    {

         public object[] GetKeys() => new object[] { Id };

        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 100)
        public string CreatedBy { get; set; } // CreatedBy (length: 100)
        public System.DateTime CreatedOn { get; set; } // CreatedOn
        public string UpdatedBy { get; set; } // UpdatedBy (length: 100)
        public System.DateTime? UpdatedOn { get; set; } // UpdatedOn
        public byte[] RowVersion { get; private set; } // RowVersion

        // Reverse navigation

        /// <summary>
        /// Child ParentChildrens where [ParentChildren].[ParentId] point to this entity (FK_ParentChildrent_Parent)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ParentChildren> ParentChildrens { get; set; } // ParentChildren.FK_ParentChildrent_Parent

        public Parent()
        {
            ParentChildrens = new System.Collections.Generic.List<ParentChildren>();
        }
    }

}
// </auto-generated>
