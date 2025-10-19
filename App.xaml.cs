using Microsoft.Data.Sqlite;
using System.Windows;

namespace CarDealerApp
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            SqliteConnection.ClearAllPools();

            base.OnExit(e);
        }
    }
}
