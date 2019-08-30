namespace Dot.Net.Mongo.Migrate.Templates
{
    /// <summary>
    /// Exends functionality to the Migration t4 template
    /// </summary>
    public partial class MigrationTemplate
    { 
        private string _className;

        public MigrationTemplate(string className)
        {
            _className = className;
        }
    }
}
