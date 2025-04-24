using Microsoft.EntityFrameworkCore.Storage;
using Oracle.EntityFrameworkCore.Storage.Internal;
using System.Text;

namespace UdemyReact
{

    /// <summary>A replacement for <see cref="OracleSqlGenerationHelper"/>
    /// to convert PascalCaseCsharpyIdentifiers to all upper casenames.
    /// Credit to nachonachoman </summary>
    public class OracleSqlGenerationCasingHelper : OracleSqlGenerationHelper
    {
        //Don't lowercase ef's migration table
        const string dontAlter = "__EFMigrationsHistory";
        static string Customize(string input) => input == dontAlter ? input : input.ToUpper();
        public OracleSqlGenerationCasingHelper(RelationalSqlGenerationHelperDependencies dependencies)
            : base(dependencies) { }
        public override string DelimitIdentifier(string identifier)
            => base.DelimitIdentifier(Customize(identifier));
        public override void DelimitIdentifier(StringBuilder builder, string identifier)
            => base.DelimitIdentifier(builder, Customize(identifier));
    }
}
