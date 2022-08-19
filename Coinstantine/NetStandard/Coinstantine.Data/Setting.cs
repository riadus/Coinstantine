using SQLite;

namespace Coinstantine.Data
{
    public class Setting : IEntity
    {
		[PrimaryKey, AutoIncrement]
        public int Id { get; set; }

		public string Key { get; set; }
		public string Value { get; set; }
		public SettingScope Scope { get; set; }
		public string UserProfileEmail { get; set; }
    }
}
