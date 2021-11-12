namespace Niravadea.Projects.Orchid.Core.Options
{
    public class ForumsClientConfiguration
    {
        private string bbuserid { get; set; }
        public string UserId => bbuserid;

        private string bbpassword { get; set; }
        public string HashedUserPassword => bbpassword;

        public string SessionHash { get; set; }

        public string SessionId { get; set; }
    }
}
