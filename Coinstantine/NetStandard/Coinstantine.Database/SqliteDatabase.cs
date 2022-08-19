using System.Collections.Generic;
using System.Diagnostics;
using Coinstantine.Data;
using SQLite;

namespace Coinstantine.Database
{
    public static class SqliteDatabase
    {
        static readonly object _locker = new object();

        public static void DropAllTables()
        {
            var firstConnection = GetConnectionWithLock(ConnectionType.First);
            var secondConnection = GetConnectionWithLock(ConnectionType.Second);
            var thirdConnection = GetConnectionWithLock(ConnectionType.Third);

            secondConnection.DropTable<UserProfile>();
            secondConnection.DropTable<PhonenumberProfile>();
            secondConnection.DropTable<TelegramProfile>();
            secondConnection.DropTable<TwitterProfile>();
            secondConnection.DropTable<BitcoinTalkProfile>();
            thirdConnection.DropTable<AuthenticationObject>();
            firstConnection.DropTable<Setting>();
            firstConnection.DropTable<Translation>();
            firstConnection.DropTable<BlockchainInfo>();
            firstConnection.DropTable<CoinstantinePriceConfig>();
            firstConnection.DropTable<Price>();
            firstConnection.DropTable<SmartContractDefinition>();
            firstConnection.DropTable<BuyingReceipt>();
            firstConnection.DropTable<BitcoinTalkAirdropRequirement>();
            firstConnection.DropTable<TwitterAirdropRequirement>();
            firstConnection.DropTable<TelegramAirdropRequirement>();
            firstConnection.DropTable<AirdropDefinition>();
            firstConnection.DropTable<UserAirdrop>();
            firstConnection.DropTable<Document>();
        }

        public static void Initialize()
        {
            var firstConnection = GetConnectionWithLock(ConnectionType.First);
            var secondConnection = GetConnectionWithLock(ConnectionType.Second);
            var thirdConnection = GetConnectionWithLock(ConnectionType.Third);

            lock (_locker)
            {
                secondConnection.CreateTable<PhonenumberProfile>();
                secondConnection.CreateTable<TelegramProfile>();
                secondConnection.CreateTable<TwitterProfile>();
                secondConnection.CreateTable<BitcoinTalkProfile>();
                secondConnection.CreateTable<BlockchainInfo>();
                secondConnection.CreateTable<UserProfile>();
                thirdConnection.CreateTable<AuthenticationObject>();
                firstConnection.CreateTable<Setting>();
                firstConnection.CreateTable<Translation>();
                firstConnection.CreateTable<CoinstantinePriceConfig>();
                firstConnection.CreateTable<Price>();
                firstConnection.CreateTable<SmartContractDefinition>();
                firstConnection.CreateTable<BuyingReceipt>();
                firstConnection.CreateTable<BitcoinTalkAirdropRequirement>();
                firstConnection.CreateTable<TwitterAirdropRequirement>();
                firstConnection.CreateTable<TelegramAirdropRequirement>();
                firstConnection.CreateTable<AirdropDefinition>();
                firstConnection.CreateTable<UserAirdrop>();
                firstConnection.CreateTable<Document>();
            }
        }

        private static Dictionary<ConnectionType, SQLiteAsyncConnection> _connections = new Dictionary<ConnectionType, SQLiteAsyncConnection>();
        public static SQLiteAsyncConnection GetAsyncConnection(ConnectionType connectionType)
        {
            lock (_locker)
            {
                if (_connections.ContainsKey(connectionType))
                {
                    return _connections[connectionType];
                }
                var asyncConnection = new SQLiteAsyncConnection(GetFilePath(connectionType), true);
#if DEBUG
                asyncConnection.GetConnection().Tracer = line => Debug.WriteLine(line);
                asyncConnection.GetConnection().TimeExecution = false;
                asyncConnection.GetConnection().Trace = false;
#endif
                _connections.Add(connectionType, asyncConnection);
                return asyncConnection;
            }
        }

        public static SQLiteConnectionWithLock GetConnectionWithLock(ConnectionType connectionType)
        {
            var asyncConnection = GetAsyncConnection(connectionType);
            return asyncConnection.GetConnection();
        }

        private static Dictionary<ConnectionType, string> _filePaths;
        public static void SetFilePath(ConnectionType connectionType, string filePath)
        {
            _filePaths = _filePaths ?? new Dictionary<ConnectionType, string>();
            if (!_filePaths.ContainsKey(connectionType))
            {
                _filePaths.Add(connectionType, filePath);
            }
        }
        public static string GetFilePath(ConnectionType connectionType)
        {
            return _filePaths[connectionType];
        }

        public enum ConnectionType
        {
            First,
            Second,
            Third
        }
    }
}